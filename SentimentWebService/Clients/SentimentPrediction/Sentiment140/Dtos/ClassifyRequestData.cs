﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SentimentWebService.Clients.SentimentPrediction.Sentiment140.Dtos;

public class ClassifyRequestData
{
    [JsonProperty(Required = Required.Always, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public string Text { get; set; } = null!;
}