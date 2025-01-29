using Application.Interfaces;

namespace Application.Services;

public sealed class LoremService : ILoremService
{
    public string GenerateSentences(int sentences)
    {
        return LoremNET.Lorem.Sentence(sentences);
    }

    public string GenerateSentencesRandomly(int maxSentences)
    {
        var random = new Random();
        var sentences = random.Next(1, maxSentences + 1);

        return GenerateSentences(sentences);
    }
}