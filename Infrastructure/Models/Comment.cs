namespace Infrastructure.Models;

public class Comment
{
    public int Id { get; set; }

    public string Author { get; set; } = null!;

    public string Text { get; set; } = null!;

    public SentimentType SentimentType { get; set; }

    public int AnalysisId { get; set; }

    public Analysis Analysis { get; set; } = null!;
}
