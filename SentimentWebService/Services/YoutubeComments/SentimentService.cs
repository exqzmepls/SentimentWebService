using SentimentWebService.Clients.SentimentPrediction;
using SentimentWebService.Clients.Youtube;
using SentimentWebService.Clients.Youtube.Dtos;
using SentimentWebService.Services.YoutubeComments.Dtos;
using SentimentTypeDto = SentimentWebService.Clients.SentimentPrediction.Dtos.SentimentType;

namespace SentimentWebService.Services.YoutubeComments;

public class SentimentService : ISentimentService
{
    private readonly IYoutubeClient _youtubeClient;
    private readonly ISentimentPredictionClient _sentimentPredictionClient;

    public SentimentService(IYoutubeClient youtubeClient, ISentimentPredictionClient sentimentPredictionClient)
    {
        _youtubeClient = youtubeClient;
        _sentimentPredictionClient = sentimentPredictionClient;
    }

    public async Task<IEnumerable<CommentSentiment>> GetCommentsSentimentAsync(string videoId)
    {
        var commentsFirstPage = await _youtubeClient.GetCommentsPageAsync(videoId);
        var sentimentPredictions = await GetSentimentsAsync(commentsFirstPage.Comments);
        var result = new List<CommentSentiment>(sentimentPredictions);

        var nextPageToken = commentsFirstPage.NextPageToken;
        while (!string.IsNullOrEmpty(nextPageToken))
        {
            var commentsPage = await _youtubeClient.GetCommentsPageAsync(videoId, nextPageToken);
            var predictions = await GetSentimentsAsync(commentsPage.Comments);
            result.AddRange(predictions);
            nextPageToken = commentsPage.NextPageToken;
        }

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
}
