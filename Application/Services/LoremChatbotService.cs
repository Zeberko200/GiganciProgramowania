using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public sealed class LoremChatbotService(ILoremService loremService) : IChatbotService
{
    public Task<string> SendMessageAsync(string prompt)
    {
        var header = $"\"{prompt}\"... Thinking... 🤖\n\n";

        var content = loremService.GenerateSentencesRandomly(30);

        var fullMessage = header + content;
        return Task.FromResult(fullMessage);
    }
}