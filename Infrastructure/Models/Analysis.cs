namespace Infrastructure.Models;

public class Analysis
{
    public int Id { get; set; }

    public DateTime CreationDateUtc { get; set; }

    public string VideoId { get; set; } = null!;

    public int NegativeCommentsCount { get; set; }

    public int NeutralCommentsCount { get; set; }

    public int PositiveCommentsCount { get; set; }

    public IEnumerable<Comment> Comments { get; set; } = null!;
}
