using Application.Commands.Requests;
using Domain.Aggregates.MessageAggregate;
using Domain.Interfaces;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Commands.Handlers;

public sealed class SendMessageCommandHandler(IChatbotService chatbotService, IMessageRepository<AppDbContext> messageRepository) : IRequestHandler<SendMessageCommand, Guid>
{
    public async Task<Guid> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Create prompt message.
        var promptMessage = new Message(Guid.Empty, MessageSender.User, DateTime.Now, request.Prompt, request.UserIpAddress);

        // Generate response.
        var response = await chatbotService.SendMessageAsync(request.Prompt);

        // Create response message.
        var responseMessage = new Message(Guid.Empty, MessageSender.Chatbot, DateTime.Now, response, request.UserIpAddress);

        // Add & save all.
        await using var transaction = await messageRepository.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            messageRepository.AddRange(promptMessage, responseMessage);
            await messageRepository.SaveChangesAsync();

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        // Return response id.
        return responseMessage.Id;
    }
}