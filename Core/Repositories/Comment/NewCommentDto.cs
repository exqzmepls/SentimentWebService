namespace Core.Repositories.Comment;

public class NewCommentDto
{
    public NewCommentDto(string author, string text, SentimentType sentimentType)
    {
        Author = author;
        Text = text;
        SentimentType = sentimentType;
    }

    public string Author { get; }
    public string Text { get; }
    public SentimentType SentimentType { get; }
}
