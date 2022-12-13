using Core.Services.YoutubeComments;
using Core.Services.YoutubeComments.Dtos;
using Microsoft.AspNetCore.Mvc;
using SentimentWebService.Models;
using SentimentWebService.Tools.Youtube;

namespace SentimentWebService.Controllers;

public class AnalysisController : Controller
{
    private readonly ISentimentService _sentimentService;
    private readonly IUrlParser _urlParser;

    public AnalysisController(ISentimentService sentimentService, IUrlParser urlParser)
    {
        _sentimentService = sentimentService;
        _urlParser = urlParser;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var analyses = _sentimentService.GetAnalyses();
        var model = analyses.Select(a => new AnalysisViewModel(a.Id, a.VideoId, a.CreationDateUtc));
        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var analysisDetails = _sentimentService.GetAnalysisDetailsOrDefault(id);

        if (analysisDetails == null)
        {
            return NotFound();
        }

        var commentsModel = GetComments(id);
        var model = new AnalysisDetailsViewModel(analysisDetails.VideoId, analysisDetails.NegativeCommentsCount, analysisDetails.NeutralCommentsCount, analysisDetails.PositiveCommentsCount, commentsModel);
        return View(model);
    }

    [HttpGet]
    public IActionResult Comments(int id)
    {
        var model = GetComments(id);
        return PartialView("_CommentsGrid", model);
    }


    [HttpGet]
    public IActionResult Create()
    {
        var model = new NewAnalysisViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] NewAnalysisViewModel newAnalysis)
    {
        var videoUrl = newAnalysis.VideoUrl;
        var videoId = _urlParser.GetVideoId(videoUrl);
        var createdAnalysisId = await _sentimentService.CreateAnalysisAsync(videoId);
        if (createdAnalysisId == default)
        {
            return BadRequest();
        }
        return RedirectToAction("Details", new { id = createdAnalysisId });
    }

    private CommentsViewModel GetComments(int analysisId)
    {
        var comments = _sentimentService.GetComments(analysisId);
        var model = new CommentsViewModel(analysisId, comments.Select(c =>
        {
            var authorNameTrim = string.Join(' ', c.AuthorName.Split().Select(w => w.Length > 32 ? "*#long word#*" : w));
            var comment = new CommentSentimentViewModel(authorNameTrim, c.Text, MapSentimentType(c.SentimentType));
            return comment;
        }));
        return model;
    }

    private static Sentiment MapSentimentType(SentimentType sentimentType)
    {
        return sentimentType switch
        {
            SentimentType.Negative => Sentiment.Negative,
            SentimentType.Neutral => Sentiment.Neutral,
            SentimentType.Positive => Sentiment.Positive,
            _ => throw new InvalidCastException(),
        };
    }
}
