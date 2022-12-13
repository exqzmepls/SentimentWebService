using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SentimentWebService.Models;

public class NewAnalysisViewModel
{
    [Required]
    [RegularExpression("https://((www\\.youtube\\.com/watch\\?v=)|(youtu.be/)).{11}", ErrorMessage = "Invalid video URL")]
    [DisplayName("Video URL")]
    public string VideoUrl { get; set; } = null!;
}
