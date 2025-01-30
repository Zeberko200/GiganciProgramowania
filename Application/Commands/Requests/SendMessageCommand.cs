using Application.DTOs;
using MediatR;

namespace Application.Commands.Requests;

public record SendMessageCommand(string Prompt, string UserIpAddress) : IRequest<MessageDto>;