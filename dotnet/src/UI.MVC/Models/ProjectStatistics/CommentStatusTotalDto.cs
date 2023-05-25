using Domain.Comment;
using Domain.ProjectStatistics;
using UI.MVC.Extensions;

namespace UI.MVC.Models.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.CommentStatusTotal"/>
/// </summary>
public class CommentStatusTotalDto
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The id.
    /// </summary>
    public int CommentStatusTotalId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment status.
    /// </summary>
    public CommentStatus CommentStatus { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// string version of <see cref="CommentStatus"/>.
    /// </summary>
    public string CommentStatusString {get; set;}

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
    public CommentStatusTotalDto()
    {
    }

    public CommentStatusTotalDto(CommentStatusTotal commentStatusTotal)
    {
        CommentStatusTotalId = commentStatusTotal.CommentStatusTotalId;
        CommentStatus = commentStatusTotal.CommentStatus;
        Total = commentStatusTotal.Total;
        CommentStatusString = commentStatusTotal.CommentStatus.ToString();
        TotalFormatted = commentStatusTotal.Total.FormatNumber();
    } // CommentStatusTotalModel.
}