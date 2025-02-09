using Application.Commands.Requests;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public sealed class ChatbotHub(IMediator mediator) : Hub
{
    public IAsyncEnumerable<SrStreamMessage> GenerateResponse(Guid messageId)
    {
        var stream = mediator.CreateStream(new GetResponseCommand(messageId));
        return stream;
    }
}