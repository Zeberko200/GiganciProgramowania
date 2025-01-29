using MediatR;

namespace Application.Queries.Requests;

public record GetMessageStream(Guid MessageId, string UserIpAddress) : IStreamRequest<string>;
public record GetMessageQuery(Guid MessageId, string UserIpAddress) : IRequest<string>;