using MediatR;

namespace Application.Commands.Requests;

public record RateMessageCommand(Guid Id, int Rate) : IRequest;