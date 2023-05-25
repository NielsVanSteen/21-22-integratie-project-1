using Domain.Comment;
using Domain.DocReview;
using Domain.ProjectStatistics;
using UI.MVC.Extensions;

namespace UI.MVC.Models.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.CommentStatusTotal"/>
/// </summary>
public class DocReviewStatusTotalDto
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The id.
    /// </summary>
    public int DocReviewStatusTotalId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment status.
    /// </summary>
    public DocReviewStatus DocReviewStatus { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// string version of <see cref="CommentStatus"/>.
    /// </summary>
    public string DocReviewStatusString {get; set;}

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The total amount of times the <see cref="CommentStatus"/> was occurs on a comment.
    /// </summary>
    public int Total { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The total amount of times the <see cref="CommentStatus"/> was occurs on a comment.
    /// </summary>
    public string TotalFormatted { get; set; }

    // Constructor.
    public DocReviewStatusTotalDto()
    {
    }

    public DocReviewStatusTotalDto(DocReviewStatusTotal docReviewStatusTotal)
    {
        DocReviewStatusTotalId = docReviewStatusTotal.DocReviewStatusTotalId;
        DocReviewStatus = docReviewStatusTotal.DocReviewStatus;
        Total = docReviewStatusTotal.Total;
        DocReviewStatusString = docReviewStatusTotal.DocReviewStatus.ToString();
        TotalFormatted = docReviewStatusTotal.Total.FormatNumber();
    } // CommentStatusTotalModel.
}