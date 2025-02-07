namespace Domain.Interfaces;

public interface IChatbotService
{
    IAsyncEnumerable<string> GetResponseAsync(string prompt);
}