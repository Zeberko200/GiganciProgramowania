using Domain.Seed;

namespace Domain.Aggregates.MessageAggregate;

public class Message : Entity<Guid>
{
    public string Content { get; private set; }
    public string UserIpAddress { get; init; }
    public DateTime SentAt { get; init; }
    public MessageSender Sender { get; init; }
    public int Rate { get; private set; }

    public Message(Guid id, MessageSender sender, DateTime sentAt, string content, string userIpAddress) : base(id)
    {
        if(string.IsNullOrWhiteSpace(userIpAddress)) throw new ArgumentException("User IP address must be provided");
        UserIpAddress = userIpAddress;

        Sender = sender;
        SentAt = sentAt;
        Content = content;
    }

    public void SetRate(int rate)
    {
        if (rate < 0)
        {
            throw new ArgumentException("Rate must be greater than or equal to 0");
        }

        Rate = rate;
    }

    public void SetContent(string content)
    {
        Content = content;
    }
}

public enum MessageSender
{
    User = 0,
    Chatbot = 1
}