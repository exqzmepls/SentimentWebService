namespace Core.Repositories.Comment;

public interface ICommentRepository
{
    public IQueryable<CommentDto> GetAll(int analysisId);

    public int Create(int analisysId, string author, string text, SentimentType sentimentType);
}
