namespace UI.MVC.Models.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// The type of statistic to show.
/// </summary>
public enum StatisticType
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// User = either users or managers.
    /// </summary>
    Users = 0,

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Statistics about the comments (all, or per status).
    /// </summary>
    Comments = 1,

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Statistics about the Emoji. (all or per type).
    /// </summary>
    Emoji = 2,

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Statistics about the doc-reviews (all, or per status).
    /// </summary>
    DocReviews = 3
}