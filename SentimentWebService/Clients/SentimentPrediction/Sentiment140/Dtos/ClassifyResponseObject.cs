using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SentimentWebService.Clients.SentimentPrediction.Sentiment140.Dtos;

public class ClassifyResponseObject
{
    [JsonProperty(Required = Required.Always, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public IEnumerable<ClassifyResponseData> Data { get; set; } = null!;
}
