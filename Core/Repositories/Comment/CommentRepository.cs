using Infrastructure;

namespace Core.Repositories.Comment;

public class CommentRepository : ICommentRepository
{
    private readonly Context _dbContext;

    public CommentRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<CommentDto> GetAll(int analysisId)
    {
        var all = _dbContext.Comments.Where(c => c.AnalysisId == analysisId).Select(c => new CommentDto(c.Id, c.Author, c.Text, MapSentimentType(c.SentimentType))).AsEnumerable();
        return all;
    }

    public bool CreateRange(int analisysId, IEnumerable<NewCommentDto> comments)
    {
        var newComments = comments.Select(c => new Infrastructure.Models.Comment
        {
            AnalysisId = analisysId,
            Author = c.Author,
            Text = c.Text,
            SentimentType = MapSentimentType(c.SentimentType)
        });
        _dbContext.Comments.AddRange(newComments);
        try
        {
            _dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
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
