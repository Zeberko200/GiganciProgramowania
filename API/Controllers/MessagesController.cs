using Application.Commands.Requests;
using Application.DTOs;
using Application.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public sealed class MessagesController(IMediator mediator) : BaseController
{
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var command = new SendMessageCommand(request.Message, await GetIpAddress());

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("get-messages")]
    public async Task<IActionResult> GetMessageQuery()
    {
        var query = new GetMessagesQuery(await GetIpAddress());

        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("rate-message")]
    public async Task<IActionResult> RateMessage([FromBody] RateMessageRequest request)
    {
        var command = new RateMessageCommand(request.MessageId, request.Rate);
        await mediator.Send(command);

        return Ok();
    }
}