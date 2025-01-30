using Application.Commands.Requests;
using Application.DTOs;
using Domain.Aggregates.MessageAggregate;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Commands.Handlers;

public sealed class SendMessageCommandHandler(IMessageRepository<AppDbContext> messageRepository) : IRequestHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Message
        var promptMessage = new Message(Guid.Empty, MessageSender.User, DateTime.Now, request.Prompt, request.UserIpAddress);

        // Save prompt in database.
        await messageRepository.AddAsync(promptMessage);
        await messageRepository.SaveChangesAsync();

        return MessageDto.FromMessage(promptMessage);
    }
}