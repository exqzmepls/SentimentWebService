namespace Core.Services.YoutubeComments.Dtos;

public class AnalysisDetails
{
    public AnalysisDetails(int id, string videoId, int negativeCommentsCount, int neutralCommentsCount, int positiveCommentsCount, IEnumerable<CommentSentiment> comments)
    {
        Id = id;
        VideoId = videoId;
        NegativeCommentsCount = negativeCommentsCount;
        NeutralCommentsCount = neutralCommentsCount;
        PositiveCommentsCount = positiveCommentsCount;
        Comments = comments;
    }

    public int Id { get; }
    public string VideoId { get; }
    public int NegativeCommentsCount { get; }
    public int NeutralCommentsCount { get; }
    public int PositiveCommentsCount { get; }
    public IEnumerable<CommentSentiment> Comments { get; }
}
