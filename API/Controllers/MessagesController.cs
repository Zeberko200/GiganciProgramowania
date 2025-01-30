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

    [HttpGet("generate-response/{messageId:guid}")]
    [Produces("text/event-stream")]
    public async Task GenerateResponse(Guid messageId)
    {
        var stream = mediator.CreateStream(new GetResponseCommand(messageId));

        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        await foreach (var data in stream)
        {
            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                break;
            }

            await Response.WriteAsync(data);
            await Response.Body.FlushAsync();
        }
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