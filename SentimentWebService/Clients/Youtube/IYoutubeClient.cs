using SentimentWebService.Clients.Youtube.Dtos;

namespace SentimentWebService.Clients.Youtube;

public interface IYoutubeClient
{
    public Task<CommentsPageDto> GetCommentsPageAsync(string videoId);

    public Task<CommentsPageDto> GetCommentsPageAsync(string videoId, string pageToken);
}
