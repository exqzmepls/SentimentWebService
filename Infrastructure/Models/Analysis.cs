namespace Infrastructure.Models;

public class Analysis
{
    public int Id { get; set; }

    public DateTime CreationDateUtc { get; set; }

    public string VideoId { get; set; } = null!;

    public IEnumerable<Comment> Comments { get; set; } = null!;
}
