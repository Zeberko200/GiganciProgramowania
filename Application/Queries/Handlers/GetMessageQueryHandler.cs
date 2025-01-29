using Application.Queries.Requests;
using Domain.Aggregates.MessageAggregate;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Queries.Handlers;

public class GetMessageQueryHandler(IMessageRepository<AppDbContext> messageRepository) : IRequestHandler<GetMessageQuery, string>
{
    public async Task<string> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        var message = await messageRepository.FindWhereAsync(p => p.Id == request.MessageId && p.UserIpAddress == request.UserIpAddress);
        if(message is null)
        {
            throw new ArgumentException("Message not found");
        }

        return message.Content;
    }
}