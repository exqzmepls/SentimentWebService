namespace Core.Repositories.Analysis;

public class AnalysisDto
{
    public AnalysisDto(int id, DateTime creationDateUtc, string videoId)
    {
        Id = id;
        CreationDateUtc = creationDateUtc;
        VideoId = videoId;
    }

    public int Id { get; }
    public DateTime CreationDateUtc { get; }
    public string VideoId { get; }
}
