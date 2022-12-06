namespace SentimentWebService.Clients.Youtube.Dtos;

public class CommentsPageDto
{
    public CommentsPageDto(IReadOnlyList<CommentDto> comments, string nextPageToken)
    {
        Comments = comments;
        NextPageToken = nextPageToken;
    }

    public IReadOnlyList<CommentDto> Comments { get; }
    public string NextPageToken { get; }
}
