using Application.DTOs;
using Application.Queries.Requests;
using Domain.Aggregates.MessageAggregate;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Queries.Handlers;

public class GetMessagesQueryHandler(IMessageRepository<AppDbContext> messageRepository) : IRequestHandler<GetMessagesQuery, GetMessageResponse>
{
    public async Task<GetMessageResponse> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await messageRepository.FindAllWhereAsync(p => p.UserIpAddress == request.UserIpAddress);
        return new GetMessageResponse(messages.Select(MessageDto.FromMessage));
    }
}