using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.DocReview;

namespace Domain.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.DocReviewStatusTotal"/>
/// </summary>
public class DocReviewStatusTotal
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The id.
    /// </summary>
    [Key]
    public int DocReviewStatusTotalId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// the statistics the total belongs to.
    /// </summary>
    public ProjectStatistics ProjectStatistics { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="ProjectStatistics"/>
    /// </summary>
    public int ProjectStatisticsId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The emoji.
    /// </summary>
    public DocReviewStatus DocReviewStatus { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The total amount of times the <see cref="DocReviewStatus"/> was occurs on a comment.
    /// </summary>
    public int Total { get; set; }

    // Constructor.
    public DocReviewStatusTotal()
    {
    }
}