using Core.Clients.SentimentPrediction.Dtos;

namespace Core.Clients.SentimentPrediction;

public interface ISentimentPredictionClient
{
    public Task<IEnumerable<SentimentType>?> GetSentimentPredictionAsync(IEnumerable<string> texts);
}
