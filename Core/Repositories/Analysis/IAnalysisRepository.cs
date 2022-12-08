namespace Core.Repositories.Analysis;

public interface IAnalysisRepository
{
    public IQueryable<AnalysisDto> GetAll();

    public int Create(string videoId);
}
