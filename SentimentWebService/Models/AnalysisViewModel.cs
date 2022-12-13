namespace SentimentWebService.Models;

public class AnalysisViewModel
{
    public AnalysisViewModel(int id, string videoId, DateTime creationDateUtc)
    {
        Id = id;
        VideoId = videoId;
        CreationDateUtc = creationDateUtc;
    }

    public int Id { get; }
    public string VideoId { get; }
    public DateTime CreationDateUtc { get; }
}
