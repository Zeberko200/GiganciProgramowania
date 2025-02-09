using Application.DTOs;
using MediatR;

namespace Application.Commands.Requests;

public record GetResponseCommand(Guid MessageId) : IStreamRequest<SrStreamMessage>{}