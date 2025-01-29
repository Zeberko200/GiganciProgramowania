namespace Application.Interfaces;

public interface IStreamTextService
{
    IAsyncEnumerable<string> GetTextStreamAsync(string text, int delayMs);
}