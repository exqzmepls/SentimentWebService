using SentimentWebService.Services.YoutubeComments.Dtos;

namespace SentimentWebService.Services.YoutubeComments;

public interface ISentimentService
{
    public Task<IEnumerable<CommentSentiment>> GetCommentsSentimentAsync(string videoId);
}
