using Infrastructure;

namespace Core.Repositories.Analysis;

public class AnalysisRepository : IAnalysisRepository
{
    private readonly Context _dbContext;

    public AnalysisRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public int Create(string videoId)
    {
        var newAnalisys = new Infrastructure.Models.Analysis
        {
            VideoId = videoId,
            CreationDateUtc = DateTime.UtcNow,
        };
        var createdAnalisys = _dbContext.Analyses.Add(newAnalisys);
        return createdAnalisys.Entity.Id;
    }

    public IQueryable<AnalysisDto> GetAll()
    {
        var all = _dbContext.Analyses.Select(a => new AnalysisDto(a.Id, a.CreationDateUtc, a.VideoId));
        return all;
    }
}
