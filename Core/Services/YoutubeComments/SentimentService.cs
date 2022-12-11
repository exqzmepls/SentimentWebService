using Core.Clients.SentimentPrediction;
using Core.Clients.Youtube;
using Core.Repositories.Analysis;
using Core.Repositories.Comment;
using Core.Services.YoutubeComments.Dtos;
using CommentDto = Core.Clients.Youtube.Dtos.CommentDto;
using SentimentType = Core.Services.YoutubeComments.Dtos.SentimentType;
using SentimentTypeDto = Core.Clients.SentimentPrediction.Dtos.SentimentType;

namespace Core.Services.YoutubeComments;

public class SentimentService : ISentimentService
{
    private readonly IAnalysisRepository _analysisRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IYoutubeClient _youtubeClient;
    private readonly ISentimentPredictionClient _sentimentPredictionClient;

    public SentimentService(IAnalysisRepository analysisRepository, ICommentRepository commentRepository, IYoutubeClient youtubeClient, ISentimentPredictionClient sentimentPredictionClient)
    {
        _analysisRepository = analysisRepository;
        _commentRepository = commentRepository;
        _youtubeClient = youtubeClient;
        _sentimentPredictionClient = sentimentPredictionClient;
    }

    public async Task<int?> CreateAnalysisAsync(string videoId)
    {
        var commentsSentiment = await GetCommentsSentimentAsync(videoId);
        var positiveCount = commentsSentiment.Count(c => c.SentimentType == SentimentType.Positive);
        var negativeCount = commentsSentiment.Count(c => c.SentimentType == SentimentType.Negative);
        var neutralCount = commentsSentiment.Count(c => c.SentimentType == SentimentType.Neutral);

        var analysisId = _analysisRepository.Create(videoId, negativeCount, neutralCount, positiveCount);
        if (analysisId == null)
        {
            return default;
        }

        var analysisComments = commentsSentiment.Select(p =>
        {
            var comment = new NewCommentDto(p.AuthorName, p.Text, MapSentimentType(p.SentimentType));
            return comment;
        });
        var creationResult = _commentRepository.CreateRange((int)analysisId, analysisComments);
        return analysisId;
    }

    public IEnumerable<Analysis> GetAnalyses()
    {
        var analyses = _analysisRepository.GetAll();
        var result = analyses.Select(a => new Analysis(a.Id, a.VideoId, a.CreationDateUtc));
        return result;
    }

    public AnalysisDetails? GetAnalysisDetailsOrDefault(int analysisId)
    {
        var analysis = _analysisRepository.GetOrDeafault(analysisId);
        if (analysis == null)
        {
            return default;
        }

        var comments = GetComments(analysisId);
        var result = new AnalysisDetails(analysis.Id, analysis.VideoId, analysis.NegativeCommentsCount, analysis.NeutralCommentsCount, analysis.PositiveCommentsCount, comments);
        return result;
    }

    public IEnumerable<CommentSentiment> GetComments(int analysisId)
    {
        var comments = _commentRepository.GetAll(analysisId);
        var commetsDto = comments.Select(c => new CommentSentiment(c.Author, c.Text, MapSentimentType(c.SentimentType)));
        return commetsDto;
    }

    private async Task<IList<CommentSentiment>> GetCommentsSentimentAsync(string videoId)
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
        return sentimentPredictions;
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

    private static SentimentType MapSentimentType(Repositories.Comment.SentimentType sentimentType)
    {
        return sentimentType switch
        {
            Repositories.Comment.SentimentType.Negative => SentimentType.Negative,
            Repositories.Comment.SentimentType.Neutral => SentimentType.Neutral,
            Repositories.Comment.SentimentType.Positive => SentimentType.Positive,
            _ => throw new InvalidCastException()
        };
    }

    private static Repositories.Comment.SentimentType MapSentimentType(SentimentType sentimentType)
    {
        return sentimentType switch
        {
            SentimentType.Negative => Repositories.Comment.SentimentType.Negative,
            SentimentType.Neutral => Repositories.Comment.SentimentType.Neutral,
            SentimentType.Positive => Repositories.Comment.SentimentType.Positive,
            _ => throw new InvalidCastException()
        };
    }
}
