using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public sealed class LoremChatbotService(ILoremService loremService) : IChatbotService
{
    public IAsyncEnumerable<string> GetResponseAsync(string prompt)
    {
        return loremService.GenerateSentencesRandomlyAsync(30, 250);
    }
}