namespace Domain.Interfaces;

public interface IChatbotService
{
    Task<string> SendMessageAsync(string prompt);
}