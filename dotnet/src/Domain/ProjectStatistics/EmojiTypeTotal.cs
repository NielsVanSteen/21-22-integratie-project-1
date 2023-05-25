using System.ComponentModel.DataAnnotations;
using Domain.DocReview;

namespace Domain.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.ProjectStatistics.EmojiTypeAmount"/>
/// </summary>
public class EmojiTypeTotal
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The id.
    /// </summary>
    [Key]
    public int EmojiTypeTotalId { get; set; }

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
    public Emoji Emoji { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Emoji"/>
    /// </summary>
    public int? EmojiId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The total amount of times the <see cref="Emoji"/> was reacted.
    /// </summary>
    public int Total { get; set; }

    // Constructor.
    public EmojiTypeTotal()
    {
    }
}