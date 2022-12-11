using Core.Clients.Youtube.Dtos;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using static Google.Apis.YouTube.v3.CommentThreadsResource.ListRequest;

namespace Core.Clients.Youtube;

public class YoutubeClient : IYoutubeClient
{
    private readonly YouTubeService _youtubeService;

    public YoutubeClient(string apiKey)
    {
        _youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "sentiment_web_service"
        });
    }

    public async Task<CommentsPageDto> GetCommentsPageAsync(string videoId)
    {
        var listRequest = _youtubeService.CommentThreads.List("snippet");
        listRequest.VideoId = videoId;
        listRequest.TextFormat = TextFormatEnum.Html;
        var response = await listRequest.ExecuteAsync();

        var result = MapResult(response);
        return result;
    }

    public async Task<CommentsPageDto> GetCommentsPageAsync(string videoId, string pageToken)
    {
        var listRequest = _youtubeService.CommentThreads.List("snippet");
        listRequest.VideoId = videoId;
        listRequest.PageToken = pageToken;
        listRequest.TextFormat = TextFormatEnum.Html;
        var response = await listRequest.ExecuteAsync();

        var result = MapResult(response);
        return result;
    }

    private static CommentsPageDto MapResult(CommentThreadListResponse commentThreadListResponse)
    {
        var topLevelComments = commentThreadListResponse.Items.Select(i =>
        {
            var topLevelComment = i.Snippet.TopLevelComment.Snippet;
            var comment = new CommentDto(topLevelComment.AuthorDisplayName, topLevelComment.TextDisplay, topLevelComment.TextOriginal);
            return comment;
        });
        var result = new CommentsPageDto(topLevelComments.ToList(), commentThreadListResponse.NextPageToken);
        return result;
    }
}
