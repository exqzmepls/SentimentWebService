using Microsoft.AspNetCore.Mvc;
using SentimentWebService.Models;
using SentimentWebService.Services.YoutubeComments;
using SentimentWebService.Tools.Youtube;

namespace SentimentWebService.Controllers;
public class YoutubeCommentsController : Controller
{
    private readonly ISentimentService _sentimentService;
    private readonly IUrlParser _urlParser;

    public YoutubeCommentsController(ISentimentService sentimentService, IUrlParser urlParser)
    {
        _sentimentService = sentimentService;
        _urlParser = urlParser;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetCommentsSentiment(string videoUrl)
    {
        var videoId = _urlParser.GetVideoId(videoUrl);
        var result = await _sentimentService.GetCommentsSentimentAsync(videoId);

        var model = result.Select(s => new CommentSentimentViewModel());
        return View(model);
    }
}
