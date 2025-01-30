using Application.Commands.Requests;
using Domain.Aggregates.MessageAggregate;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Commands.Handlers;

public sealed class RateMessageCommandHandler(IMessageRepository<AppDbContext> messageRepository) : IRequestHandler<RateMessageCommand>
{
    public async Task Handle(RateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await messageRepository.FindAsync(request.Id);
        if (message is null)
        {
            throw new KeyNotFoundException($"Message {request.Id} not found.");
        }

        message.SetRate(request.Rate);
        await messageRepository.SaveChangesAsync();
    }
}