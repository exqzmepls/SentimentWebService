using Core.Clients.Youtube.Dtos;

namespace Core.Clients.Youtube;

public interface IYoutubeClient
{
    public Task<CommentsPageDto> GetCommentsPageAsync(string videoId);

    public Task<CommentsPageDto> GetCommentsPageAsync(string videoId, string pageToken);
}