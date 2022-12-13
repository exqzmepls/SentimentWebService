namespace SentimentWebService.Models;

public class CommentStatViewModel
{
    public CommentStatViewModel(int count, int total)
    {
        var percenatage = Math.Round((double)count / total * 100, 1);
        Value = $"{count} ({percenatage} %)";
    }

    public string Value { get; }
}
