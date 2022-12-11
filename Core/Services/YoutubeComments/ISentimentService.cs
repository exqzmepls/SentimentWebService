using Core.Services.YoutubeComments.Dtos;

namespace Core.Services.YoutubeComments;

public interface ISentimentService
{
    public IEnumerable<Analysis> GetAnalyses();

    public AnalysisDetails? GetAnalysisDetailsOrDefault(int analysisId);

    public IEnumerable<CommentSentiment> GetComments(int analysisId);

    public Task<int?> CreateAnalysisAsync(string videoId);
}
