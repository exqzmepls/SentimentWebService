namespace Core.Repositories.Comment;

public class CommentDto
{
    public CommentDto(int id, string author, string text, SentimentType sentimentType)
    {
        Id = id;
        Author = author;
        Text = text;
        SentimentType = sentimentType;
    }

    public int Id { get; }
    public string Author { get; }
    public string Text { get; }
    public SentimentType SentimentType { get; }
}
