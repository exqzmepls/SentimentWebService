using Infrastructure;

namespace Core.Repositories.Comment;

public class CommentRepository : ICommentRepository
{
    private readonly Context _dbContext;

    public CommentRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public int Create(int analisysId, string author, string text, SentimentType sentimentType)
    {
        var newComment = new Infrastructure.Models.Comment
        {
            AnalysisId = analisysId,
            Author = author,
            Text = text,
            SentimentType = MapSentimentType(sentimentType)
        };
        var createdComment = _dbContext.Comments.Add(newComment);
        var commentId = createdComment.Entity.Id;
        return commentId;
    }

    public IQueryable<CommentDto> GetAll(int analysisId)
    {
        var all = _dbContext.Comments.Where(c => c.AnalysisId == analysisId).Select(c => new CommentDto(c.Id, c.Author, c.Text, MapSentimentType(c.SentimentType)));
        return all;
    }

    private static Infrastructure.Models.SentimentType MapSentimentType(SentimentType sentimentType)
    {
        return sentimentType switch
        {
            SentimentType.Negative => Infrastructure.Models.SentimentType.Negative,
            SentimentType.Neutral => Infrastructure.Models.SentimentType.Neutral,
            SentimentType.Positive => Infrastructure.Models.SentimentType.Positive,
            _ => throw new InvalidCastException(),
        };
    }

    private static SentimentType MapSentimentType(Infrastructure.Models.SentimentType sentimentType)
    {
        return sentimentType switch
        {
            Infrastructure.Models.SentimentType.Negative => SentimentType.Negative,
            Infrastructure.Models.SentimentType.Neutral => SentimentType.Neutral,
            Infrastructure.Models.SentimentType.Positive => SentimentType.Positive,
            _ => throw new InvalidCastException()
        };
    }
}
