using Application.Interfaces;
using Bogus;

namespace Application.Services;

public sealed class LoremService(Faker faker) : ILoremService
{
    public async IAsyncEnumerable<string> GenerateSentencesAsync(int sentences, int delayBetweenMs = 0)
    {
        for (var i = 0; i < sentences; i++)
        {
            var wordCount = faker.Random.Int(3, 15);

            for (var j = 0; j < wordCount; j++)
            {
                var word = faker.Lorem.Word();

                if (j == 0)
                {
                    word = char.ToUpper(word[0]) + word[1..];
                }

                if (j == wordCount - 1)
                {
                    word += ".";
                }

                yield return word;

                if (delayBetweenMs > 0)
                {
                    await Task.Delay(delayBetweenMs);
                }
            }
        }
    }

    public IAsyncEnumerable<string> GenerateSentencesRandomlyAsync(int maxSentences, int delayBetweenMs = 0)
    {
        var random = new Random();
        var sentences = random.Next(1, maxSentences + 1);

        return GenerateSentencesAsync(sentences, delayBetweenMs);
    }
}