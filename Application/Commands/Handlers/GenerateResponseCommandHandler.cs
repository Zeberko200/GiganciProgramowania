using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Application.Commands.Requests;
using Application.DTOs;
using Application.Interfaces;
using Domain.Aggregates.MessageAggregate;
using Domain.Interfaces;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Commands.Handlers;

public sealed class GetResponseCommandHandler(IChatbotService chatbotService, IStreamTextService streamTextService, IMessageRepository<AppDbContext> messageRepository) : IStreamRequestHandler<GetResponseCommand, string>
{
    public async IAsyncEnumerable<string> Handle(GetResponseCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var message = await messageRepository.FindAsync(request.MessageId);
        if(message is null)
        {
            throw new KeyNotFoundException($"Message {request.MessageId} not found.");
        }

        // Generate response.
        var fullResponse = await chatbotService.SendMessageAsync(message.Content);
        
        // DTO for last response.
        MessageDto? messageDto = null;
        var sentResponse = string.Empty;
        var responseSentDate = DateTime.Now;

        yield return "event: message-sending-start\ndata: \n\n";
        
        try
        {
            // Stream message.
            await foreach (var chunk in streamTextService.GetTextStreamAsync(fullResponse, 300).WithCancellation(cancellationToken))
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
            var responseMessage = new Message(Guid.Empty, MessageSender.Chatbot, responseSentDate, sentResponse, message.UserIpAddress);
            await messageRepository.AddAsync(responseMessage);
            await messageRepository.SaveChangesAsync();

            messageDto = MessageDto.FromMessage(responseMessage);
        }
        
        // Send DTO.
        var messageDtoJson = JsonSerializer.Serialize(messageDto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        yield return $"event: message-sending-end\ndata: {messageDtoJson}\n\n";
    }
}