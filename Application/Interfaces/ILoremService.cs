namespace Application.Interfaces;

public interface ILoremService
{
    public string GenerateSentences(int sentences);
    public string GenerateSentencesRandomly(int maxSentences);
}