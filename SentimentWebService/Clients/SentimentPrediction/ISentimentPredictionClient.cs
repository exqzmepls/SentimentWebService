using SentimentWebService.Clients.SentimentPrediction.Dtos;

namespace SentimentWebService.Clients.SentimentPrediction;

public interface ISentimentPredictionClient
{
    public Task<IEnumerable<SentimentType>?> GetSentimentPredictionAsync(IEnumerable<string> texts);
}
