using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SentimentWebService.Clients.SentimentPrediction.Sentiment140.Dtos;

public class ClassifyRequestObject
{
    [JsonProperty(Required = Required.Always, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public IEnumerable<ClassifyRequestData> Data { get; set; } = null!;
}
