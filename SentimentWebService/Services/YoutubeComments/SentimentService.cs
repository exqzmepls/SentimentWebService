using Core;
using SentimentWebService.Clients.SentimentPrediction;
using SentimentWebService.Clients.Youtube;
using SentimentWebService.Clients.Youtube.Dtos;
using SentimentWebService.Services.YoutubeComments.Dtos;
using SentimentTypeDto = SentimentWebService.Clients.SentimentPrediction.Dtos.SentimentType;

namespace SentimentWebService.Services.YoutubeComments;

public class SentimentService : ISentimentService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IYoutubeClient _youtubeClient;
    private readonly ISentimentPredictionClient _sentimentPredictionClient;

    public SentimentService(UnitOfWork unitOfWork, IYoutubeClient youtubeClient, ISentimentPredictionClient sentimentPredictionClient)
    {
        _unitOfWork = unitOfWork;
        _youtubeClient = youtubeClient;
        _sentimentPredictionClient = sentimentPredictionClient;
    }

    public async Task<int?> CreateAnalysis(string videoId)
    {
        var commentsFirstPage = await _youtubeClient.GetCommentsPageAsync(videoId);
        var firstPageSentimentPredictions = await GetSentimentsAsync(commentsFirstPage.Comments);
        var sentimentPredictions = new List<CommentSentiment>(firstPageSentimentPredictions);

        var nextPageToken = commentsFirstPage.NextPageToken;
        while (!string.IsNullOrEmpty(nextPageToken))
        {
            var commentsPage = await _youtubeClient.GetCommentsPageAsync(videoId, nextPageToken);
            var predictions = await GetSentimentsAsync(commentsPage.Comments);
            sentimentPredictions.AddRange(predictions);
            nextPageToken = commentsPage.NextPageToken;
        }

        try
        {
            var analysisId = _unitOfWork.AnalysisRepository.Create(videoId);
            sentimentPredictions.ForEach(c => _unitOfWork.CommentRepository.Create(analysisId, c.AuthorName, c.Text, MapSentimentType(c.SentimentType)));
            _unitOfWork.SaveChanges();
            return analysisId;
        }
        catch
        {
            return default;
        }
    }

    public IQueryable<Analysis> GetAnalyses()
    {
        var analyses = _unitOfWork.AnalysisRepository.GetAll();
        var result = analyses.Select(a => new Analysis(a.Id, a.VideoId, a.CreationDateUtc));
        return result;
    }

    public IQueryable<CommentSentiment> GetAnalysisComments(int analysisId)
    {
        var comments = _unitOfWork.CommentRepository.GetAll(analysisId);
        var result = comments.Select(c => new CommentSentiment(c.Author, c.Text, MapSentimentType(c.SentimentType)));
        return result;
    }

    private async Task<IEnumerable<CommentSentiment>> GetSentimentsAsync(IReadOnlyList<CommentDto> comments)
    {
        var texts = comments.Select(c => c.PlainText);
        var predictions = await _sentimentPredictionClient.GetSentimentPredictionAsync(texts);

        if (predictions == null)
        {
            return Enumerable.Empty<CommentSentiment>();
        }

        var result = comments.Zip(predictions, (c, p) =>
        {
            var sentimentType = p switch
            {
                SentimentTypeDto.Negative => SentimentType.Negative,
                SentimentTypeDto.Neutral => SentimentType.Neutral,
                SentimentTypeDto.Positive => SentimentType.Positive,
                _ => throw new InvalidCastException(),
            };
            var commentSentiment = new CommentSentiment(c.Author, c.HtmlText, sentimentType);
            return commentSentiment;
        });
        return result;
    }

    private static SentimentType MapSentimentType(Core.Repositories.Comment.SentimentType sentimentType)
    {
        return sentimentType switch
        {
            Core.Repositories.Comment.SentimentType.Negative => SentimentType.Negative,
            Core.Repositories.Comment.SentimentType.Neutral => SentimentType.Neutral,
            Core.Repositories.Comment.SentimentType.Positive => SentimentType.Positive,
            _ => throw new InvalidCastException()
        };
    }

    private static Core.Repositories.Comment.SentimentType MapSentimentType(SentimentType sentimentType)
    {
        return sentimentType switch
        {
            SentimentType.Negative => Core.Repositories.Comment.SentimentType.Negative,
            SentimentType.Neutral => Core.Repositories.Comment.SentimentType.Neutral,
            SentimentType.Positive => Core.Repositories.Comment.SentimentType.Positive,
            _ => throw new InvalidCastException()
        };
    }
}
