namespace SentimentWebService.Models;

public class CommentsViewModel
{
    public CommentsViewModel(int analysisId, IEnumerable<CommentSentimentViewModel> comments)
    {
        AnalysisId = analysisId;
        Comments = comments;
    }

    public int AnalysisId { get; }
    public IEnumerable<CommentSentimentViewModel> Comments { get; }
}
