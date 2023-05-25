using Domain.Comment;
using Domain.Project;
using Domain.ProjectStatistics;
using UI.MVC.Extensions;

namespace UI.MVC.Models.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.ProjectStatistics"/>
/// </summary>
public class ProjectStatisticsDto
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.ProjectStatistics.ProjectStatisticsId"/>
    /// But in string format using <see cref="FormatExtensions"/>
    /// </summary>
    public int ProjectStatisticsId { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.ProjectStatistics.LastUpdated"/>
    /// But in string format using <see cref="FormatExtensions"/>
    /// </summary>
    public string LastUpdated { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// the total amount of see <see cref="ReactionGroup"/> on a specific <see cref="Project"/>
    /// But in string format using <see cref="FormatExtensions"/>
    /// </summary>
    public string ReactionGroupAmountFormatted { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ReactionGroupAmountFormatted"/>
    /// </summary>
    public int ReactionGroupAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of times an emoji has been reacted on comments on the project.
    /// But in string format using <see cref="FormatExtensions"/>
    /// </summary>
    public string EmojiAmountFormatted { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="EmojiAmountFormatted"/>
    /// </summary>
    public int EmojiAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of <see cref="UserRole.NormalUser"/> registered on the project.
    /// </summary>
    public string UsersAmountFormatted { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="UsersAmountFormatted"/>
    /// </summary>
    public int UsersAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of <see cref="UserRole.ProjectManager"/> registered on the project.
    /// </summary>
    public string ManagersAmountFormatted { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="EmojiAmountFormatted"/>
    /// </summary>
    public int ManagersAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of doc-reviews in a formatted manner.
    /// </summary>
    public string DocReviewsAmountFormatted { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of doc-reviews.
    /// </summary>
    public int DocReviewsAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.ProjectStatistics.EmojiTypeAmount"/>.
    /// </summary>
    public IEnumerable<EmojiTypeTotalDto> EmojiTypeAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.ProjectStatistics.CommentStatusTypeAmount"/>..
    /// </summary>
    public IEnumerable<CommentStatusTotalDto> CommentStatusTypeAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.ProjectStatistics.DocReviewStatusTypeAmount"/>..
    /// </summary>
    public IEnumerable<DocReviewStatusTotalDto> DocReviewStatusTypeAmount { get; set; }

    // Constructor.
    public ProjectStatisticsDto()
    {
        EmojiTypeAmount = new List<EmojiTypeTotalDto>();
        CommentStatusTypeAmount = new List<CommentStatusTotalDto>();
    }

    // Copy Constructor.
    public ProjectStatisticsDto(Domain.ProjectStatistics.ProjectStatistics stats) : base()
    {
        // The scalar properties.
        ProjectStatisticsId = stats.ProjectStatisticsId;
        LastUpdated = stats.LastUpdated.GetDateWithAbbreviatedMonth();
        ReactionGroupAmountFormatted = stats.ReactionGroupAmount.FormatNumber();
        EmojiAmountFormatted = stats.EmojiAmount.FormatNumber();
        UsersAmountFormatted = stats.UsersAmount.FormatNumber();
        ManagersAmountFormatted = stats.ManagersAmount.FormatNumber();
        ReactionGroupAmount = stats.ReactionGroupAmount;
        EmojiAmount = stats.EmojiAmount;
        UsersAmount = stats.UsersAmount;
        ManagersAmount = stats.ManagersAmount;
        DocReviewsAmount = stats.DocReviewsAmount;
        DocReviewsAmountFormatted = stats.DocReviewsAmount.FormatNumber();

        // The navigation properties.
        EmojiTypeAmount = stats.EmojiTypeAmount.Select(t => new EmojiTypeTotalDto(t));
        CommentStatusTypeAmount = stats.CommentStatusTypeAmount.Select(t => new CommentStatusTotalDto(t));
        DocReviewStatusTypeAmount = stats.DocReviewStatusTypeAmount.Select(t => new DocReviewStatusTotalDto(t));
    } // ProjectStatisticsModel.
}