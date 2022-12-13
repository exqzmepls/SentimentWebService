namespace SentimentWebService.Models;

public class CommentSentimentViewModel
{
    public CommentSentimentViewModel(string author, string text, Sentiment sentiment)
    {
        Author = author;
        Text = text;
        Sentiment = sentiment;
    }

    public string Author { get; }
    public string Text { get; }
    public Sentiment Sentiment { get; }
}
