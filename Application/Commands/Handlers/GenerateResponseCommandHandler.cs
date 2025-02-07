using System.Runtime.CompilerServices;
using Application.Commands.Requests;
using Application.DTOs;
using Application.Interfaces;
using Domain.Aggregates.MessageAggregate;
using Domain.Interfaces;
using Infrastructure.Persistence;
using MediatR;
using Shared;

namespace Application.Commands.Handlers;

public sealed class GetResponseCommandHandler(IChatbotService chatbotService, IStreamTextService streamTextService, IMessageRepository<AppDbContext> messageRepository) : IStreamRequestHandler<GetResponseCommand, string>
{
    public async IAsyncEnumerable<string> Handle(GetResponseCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var promptMessage = await messageRepository.FindAsync(request.MessageId);
        if(promptMessage is null)
        {
            throw new KeyNotFoundException($"Message {request.MessageId} not found.");
        }

        // DTO for last response.
        var sentResponse = string.Empty;
        var responseSentDate = DateTime.Now;

        // Initialize message.
        var responseMessage = new Message(Guid.Empty, MessageSender.Chatbot, responseSentDate, sentResponse, promptMessage.UserIpAddress);
        await messageRepository.AddAsync(responseMessage);
        await messageRepository.SaveChangesAsync();

        // Create DTO.
        var responseMessageDto = MessageDto.FromMessage(responseMessage);

        // Stream...
        yield return $"event: message-sending-start\ndata: {responseMessageDto.ToCamelJson()}\n\n";
        try
        {
            await foreach (var chunk in chatbotService.GetResponseAsync(promptMessage.Content).WithCancellation(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                sentResponse += $@"{chunk} ";
        
                yield return $"data: {chunk}\n\n";
            }
        }
        finally
        {
            // Save in database & create DTO.
            responseMessage.SetContent(sentResponse);
            await messageRepository.SaveChangesAsync();

            // Update DTO.
            responseMessageDto = MessageDto.FromMessage(responseMessage);
        }

        // Send final DTO.
        yield return $"event: message-sending-end\ndata: {responseMessageDto.ToCamelJson()}\n\n";
    }
}