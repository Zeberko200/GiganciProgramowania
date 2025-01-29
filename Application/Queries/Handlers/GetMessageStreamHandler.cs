using System.Runtime.CompilerServices;
using Application.Interfaces;
using Application.Queries.Requests;
using Domain.Aggregates.MessageAggregate;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Queries.Handlers;

public sealed class GetMessageStreamHandler(IMessageRepository<AppDbContext> messageRepository, IStreamTextService streamTextService) : IStreamRequestHandler<GetMessageStream, string>
{
    public async IAsyncEnumerable<string> Handle(GetMessageStream request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // Find message.
        var message = await messageRepository.FindWhereAsync(p => p.Id == request.MessageId && p.UserIpAddress == request.UserIpAddress);
        if(message is null)
        {
            throw new ArgumentException("Message not found");
        }

        // Stream message.
        await foreach (var chunk in streamTextService.GetTextStreamAsync(message.Content, 300).WithCancellation(cancellationToken))
        {
            if(cancellationToken.IsCancellationRequested)
            {
                break;
            }

            yield return chunk;
        }
    }
}