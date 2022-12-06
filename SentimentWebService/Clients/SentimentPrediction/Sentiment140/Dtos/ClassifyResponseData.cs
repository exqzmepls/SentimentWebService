using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SentimentWebService.Clients.SentimentPrediction.Sentiment140.Dtos;

public class ClassifyResponseData
{
    [JsonProperty(Required = Required.Always, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public int Polarity { get; set; }
}
