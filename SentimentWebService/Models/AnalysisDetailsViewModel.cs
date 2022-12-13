using System.ComponentModel;

namespace SentimentWebService.Models;

public class AnalysisDetailsViewModel
{
    public AnalysisDetailsViewModel(string videoId, int negativeCommentsCount, int neutralCommentsCount, int positiveCommentsCount, CommentsViewModel comments)
    {
        VideoId = videoId;
        CommentsTotalCount = negativeCommentsCount + neutralCommentsCount + positiveCommentsCount;
        NegativeCommentsStat = new CommentStatViewModel(negativeCommentsCount, CommentsTotalCount);
        PositiveCommentsStat = new CommentStatViewModel(positiveCommentsCount, CommentsTotalCount);
        NeutralCommentsStat = new CommentStatViewModel(neutralCommentsCount, CommentsTotalCount);
        Comments = comments;
    }

    public string VideoId { get; }

    [DisplayName("Comments total count")]
    public int CommentsTotalCount { get; }

    [DisplayName("Negative comments")]
    public CommentStatViewModel NegativeCommentsStat { get; }
    
    [DisplayName("Neutral comments")]
    public CommentStatViewModel NeutralCommentsStat { get; }
    
    [DisplayName("Positive comments")]
    public CommentStatViewModel PositiveCommentsStat { get; }
    
    public CommentsViewModel Comments { get; }
}
