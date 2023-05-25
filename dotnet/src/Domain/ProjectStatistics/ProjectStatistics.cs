using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.Project;
using Domain.User;

namespace Domain.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// The project statistic will be resource intensive for big projects, hence this class is created. it contains all the numbers for the statistics.
/// This class will be stored in the database, and will only be updated once every x amount of time.
/// So when a user requests the statistic the object will just be returned from database.
/// </summary>
public class ProjectStatistics
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The id.
    /// </summary>
    [Key]
    public int ProjectStatisticsId { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The statistics are refreshed once every x amount of time. This <see cref="LastUpdated"/> is used to check whether
    /// the <see cref="ProjectStatistics"/> needs refreshing.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The project the statistics belong to.
    /// </summary>
    public Project.Project Project { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Project"/>.
    /// </summary>
    public int ProjectId { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// the total amount of see <see cref="ReactionGroup"/> on a specific <see cref="Project"/>
    /// </summary>
    public int ReactionGroupAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of times an emoji has been reacted on comments on the project.
    /// </summary>
    public int EmojiAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Each <see cref="Project"/> can have different amount of allowed <see cref="DocReview.Emoji"/>'s on their <see cref="DocReview"/>
    /// This means we can't (and definitely) shouldn't create a property for each allowed <see cref="DocReview.Emoji"/>.
    /// and thus we'll use this dictionary. where the key represents the <see cref="DocReview.Emoji"/> and the value represents the amount of times it has been
    /// reacted.
    /// </summary>
    public IEnumerable<EmojiTypeTotal> EmojiTypeAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of <see cref="UserRole.RegularUser"/> registered on the project.
    /// </summary>
    public int UsersAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of <see cref="UserRole.ProjectManager"/> registered on the project.
    /// </summary>
    public int ManagersAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of doc-reviews.
    /// </summary>
    public int DocReviewsAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// There are multiple <see cref="CommentStatus"/> on comments. this list holds the total amount each status occurs.
    /// </summary>
    public IEnumerable<CommentStatusTotal> CommentStatusTypeAmount { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// There are multiple <see cref="DocReview.DocReviewStatus"/> on comments. this list holds the total amount each status occurs.
    /// </summary>
    public IEnumerable<DocReviewStatusTotal> DocReviewStatusTypeAmount { get; set; }


    // Constructor.
    public ProjectStatistics()
    {
    }
}