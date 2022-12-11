using System.Net.Http;
using Core.Clients.SentimentPrediction.Dtos;
using Core.Clients.SentimentPrediction.Sentiment140.Dtos;
using Newtonsoft.Json;

namespace Core.Clients.SentimentPrediction.Sentiment140;

public class Sentiment140Client : ISentimentPredictionClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public Sentiment140Client(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<SentimentType>?> GetSentimentPredictionAsync(IEnumerable<string> texts)
    {
        var client = _httpClientFactory.CreateClient("sentiment140");

        var uriBuilder = new UriBuilder
        {
            Scheme = "http",
            Host = "www.sentiment140.com",
            Path = string.Join('/', "api", "bulkClassifyJson"),
        };
        var uri = uriBuilder.Uri;

        var requestObject = new ClassifyRequestObject
        {
            Data = texts.Select(t =>
            {
                var data = new ClassifyRequestData
                {
                    Text = t,
                };
                return data;
            })
        };
        var requestBody = JsonConvert.SerializeObject(requestObject);
        var content = new StringContent(requestBody);

        var responseMessage = await client.PostAsync(uri, content);

        var responseString = await responseMessage.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ClassifyResponseObject>(responseString);
        var result = responseObject?.Data.Select(d => GetSentimentType(d.Polarity));
        return result;
    }

    private static SentimentType GetSentimentType(int polarity)
    {
        return polarity switch
        {
            0 => SentimentType.Negative,
            2 => SentimentType.Neutral,
            4 => SentimentType.Positive,
            _ => throw new InvalidCastException(),
        };
    }
}
