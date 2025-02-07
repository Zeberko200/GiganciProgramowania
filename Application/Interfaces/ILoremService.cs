namespace Application.Interfaces;

public interface ILoremService
{
    public IAsyncEnumerable<string> GenerateSentencesAsync(int sentences, int delayBetweenMs = 0);
    public IAsyncEnumerable<string> GenerateSentencesRandomlyAsync(int maxSentences, int delayBetweenMs = 0);
}