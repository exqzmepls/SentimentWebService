namespace Core.Repositories.Analysis;

public class AnalysisDetailsDto
{
    public AnalysisDetailsDto(int id, string videoId, int negativeCommentsCount, int neutralCommentsCount, int positiveCommentsCount)
    {
        Id = id;
        VideoId = videoId;
        NegativeCommentsCount = negativeCommentsCount;
        NeutralCommentsCount = neutralCommentsCount;
        PositiveCommentsCount = positiveCommentsCount;
    }

    public int Id { get; }
    public string VideoId { get; }
    public int NegativeCommentsCount { get; }
    public int NeutralCommentsCount { get; }
    public int PositiveCommentsCount { get; }
}
