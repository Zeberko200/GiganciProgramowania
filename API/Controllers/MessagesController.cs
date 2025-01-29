using Application.Commands.Requests;
using Application.DTOs;
using Application.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public sealed class MessagesController(IMediator mediator) : BaseController
{
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest dto)
    {
        var message = new SendMessageCommand(dto.Message, GetIpAddress());

        var responseMessageId = await mediator.Send(message);

        return Ok(new SendMessageResponse(responseMessageId));
    }

    [HttpGet("get-message-stream")]
    [Produces("text/event-stream")]
    public async Task GetMessageStream([FromQuery] Guid messageId)
    {
        var query = new GetMessageStream(messageId, GetIpAddress());
        
        var stream = mediator.CreateStream(query);

        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await foreach (var word in stream)
        {
            await Response.WriteAsync(word + " ");
            await Response.Body.FlushAsync();
        }
    }

    [HttpGet("get-message")]
    public async Task<IActionResult> GetMessageQuery([FromQuery] Guid messageId)
    {
        var query = new GetMessageQuery(messageId, GetIpAddress());

        var result = await mediator.Send(query);

        return Ok(result);
    }
}