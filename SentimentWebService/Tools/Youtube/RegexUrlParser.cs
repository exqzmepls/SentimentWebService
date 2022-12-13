using System.Text.RegularExpressions;

namespace SentimentWebService.Tools.Youtube;

public class RegexUrlParser : IUrlParser
{
    public string GetVideoId(string videoUrl)
    {
        var regex = new Regex(@"(?<=https://(www\.youtube\.com/watch\?v=)|(youtu\.be/)).{11}");
        var match = regex.Match(videoUrl);
        return match.Value;
    }
}
