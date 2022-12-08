using Core.Repositories.Analysis;
using Core.Repositories.Comment;
using Infrastructure;

namespace Core;

public class UnitOfWork
{
    private readonly Context _dbContext;

    public UnitOfWork(Context dbContext)
    {
        _dbContext = dbContext;
        AnalysisRepository = new AnalysisRepository(_dbContext);
        CommentRepository = new CommentRepository(_dbContext);
    }

    public IAnalysisRepository AnalysisRepository { get; }

    public ICommentRepository CommentRepository { get; }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}
