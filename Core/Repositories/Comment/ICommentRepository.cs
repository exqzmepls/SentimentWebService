namespace Core.Repositories.Comment;

public interface ICommentRepository
{
    public IEnumerable<CommentDto> GetAll(int analysisId);

    public bool CreateRange(int analisysId, IEnumerable<NewCommentDto> comments);
}
