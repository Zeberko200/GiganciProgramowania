using System.Runtime.CompilerServices;
using Application.Commands.Requests;
using Application.DTOs;
using Domain.Aggregates.MessageAggregate;
using Domain.Interfaces;
using Infrastructure.Persistence;
using MediatR;
using Shared;

namespace Application.Commands.Handlers;

public sealed class GetResponseCommandHandler(
    IChatbotService chatbotService, 
    IMessageRepository<AppDbContext> messageRepository
) : IStreamRequestHandler<GetResponseCommand, SrStreamMessage>
{
    public async IAsyncEnumerable<SrStreamMessage> Handle(GetResponseCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var promptMessage = await messageRepository.FindAsync(request.MessageId);
        if (promptMessage is null)
        {
            throw new KeyNotFoundException($"Message {request.MessageId} not found.");
        }

        //  Initialize message.
        var sentResponse = string.Empty;
        var responseSentDate = DateTime.Now;
        var responseMessage = new Message(Guid.Empty, MessageSender.Chatbot, responseSentDate, sentResponse, promptMessage.UserIpAddress);
        await messageRepository.AddAsync(responseMessage);
        await messageRepository.SaveChangesAsync();

        // Utworzenie DTO.
        var responseMessageDto = MessageDto.FromMessage(responseMessage);
        yield return new SrStreamMessage("init", responseMessageDto.ToCamelJson());

        // Stream...
        try
        {
            await foreach (var chunk in chatbotService.GetResponseAsync(promptMessage.Content).WithCancellation(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                sentResponse += $"{chunk} ";

                yield return new SrStreamMessage("data", chunk);
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
        yield return new SrStreamMessage("complete", responseMessageDto.ToCamelJson());
    }
}