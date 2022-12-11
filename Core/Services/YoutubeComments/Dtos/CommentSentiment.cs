namespace Core.Services.YoutubeComments.Dtos;

public class CommentSentiment
{
    public CommentSentiment(string authorName, string text, SentimentType sentimentType)
    {
        AuthorName = authorName;
        Text = text;
        SentimentType = sentimentType;
    }

    public string AuthorName { get; }
    public string Text { get; }
    public SentimentType SentimentType { get; }
}
