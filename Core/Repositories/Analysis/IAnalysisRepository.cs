namespace Core.Repositories.Analysis;

public interface IAnalysisRepository
{
    public IEnumerable<AnalysisDto> GetAll();

    public AnalysisDetailsDto? GetOrDeafault(int id);

    public int? Create(string videoId, int negativeCommentsCount, int neutralCommentsCount, int positiveCommentsCount);
}
