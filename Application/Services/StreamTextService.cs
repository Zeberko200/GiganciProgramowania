using Application.Interfaces;

namespace Application.Services;

public sealed class StreamTextService : IStreamTextService
{
    public async IAsyncEnumerable<string> GetTextStreamAsync(string text, int delayMs)
    {
        var words = text.Split(' ');
        
        foreach (var word in words)
        {
            yield return word;
            await Task.Delay(delayMs);
        }
    }
}