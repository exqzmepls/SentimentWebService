using SentimentWebService.Services.YoutubeComments.Dtos;

namespace SentimentWebService.Services.YoutubeComments;

public interface ISentimentService
{
    public IQueryable<Analysis> GetAnalyses();

    public IQueryable<CommentSentiment> GetAnalysisComments(int analysisId);

    public Task<int?> CreateAnalysis(string videoId);
}
