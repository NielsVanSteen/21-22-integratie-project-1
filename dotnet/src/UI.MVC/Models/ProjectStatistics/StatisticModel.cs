namespace UI.MVC.Models.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// Model to return a specific statistic.
/// </summary>
public class StatisticModel
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The type of statistic, e.g., 'Comment', 'Emoji', 'DocReview', 'Userss'.
    /// </summary>
    public StatisticType Type { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The specific statistic to show (user, manager, deleted comments, archived doc-reviews, ...)
    /// </summary>
    public string Statistic { get; set; }

     /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="StatisticLengthEnum"/>
    /// </summary>
    public StatisticLengthEnum Length { get; set; }
}