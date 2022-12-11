using Infrastructure;

namespace Core.Repositories.Analysis;

public class AnalysisRepository : IAnalysisRepository
{
    private readonly Context _dbContext;

    public AnalysisRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public int? Create(string videoId, int negativeCommentsCount, int neutralCommentsCount, int positiveCommentsCount)
    {
        var newAnalysis = new Infrastructure.Models.Analysis
        {
            VideoId = videoId,
            CreationDateUtc = DateTime.UtcNow,
            NegativeCommentsCount = negativeCommentsCount,
            NeutralCommentsCount = neutralCommentsCount,
            PositiveCommentsCount = positiveCommentsCount
        };
        var createdAnalisys = _dbContext.Analyses.Add(newAnalysis);
        try
        {
            _dbContext.SaveChanges();
            var createdAnalysisId = createdAnalisys.Entity.Id;
            return createdAnalysisId;
        }
        catch
        {
            return default;
        }
    }

    public IEnumerable<AnalysisDto> GetAll()
    {
        var all = _dbContext.Analyses.Select(a => new AnalysisDto(a.Id, a.CreationDateUtc, a.VideoId)).AsEnumerable();
        return all;
    }

    public AnalysisDetailsDto? GetOrDeafault(int id)
    {
        var analysis = _dbContext.Analyses.Find(id);
        if (analysis == null)
        {
            return default;
        }

        return new AnalysisDetailsDto(analysis.Id, analysis.VideoId, analysis.NegativeCommentsCount, analysis.NeutralCommentsCount, analysis.PositiveCommentsCount);
    }
}
