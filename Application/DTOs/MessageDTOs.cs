using Domain.Aggregates.MessageAggregate;

namespace Application.DTOs;

public record SendMessageRequest(string Message);

public record GetMessageResponse(IEnumerable<MessageDto> Messages);

public record RateMessageRequest(Guid MessageId, int Rate);

public record MessageDto(Guid Id, string Content, string UserIpAddress, DateTime SentAt, MessageSender Sender, int Rate)
{
    public static MessageDto FromMessage(Message message) => new(message.Id, message.Content, message.UserIpAddress, message.SentAt, message.Sender, message.Rate);
}