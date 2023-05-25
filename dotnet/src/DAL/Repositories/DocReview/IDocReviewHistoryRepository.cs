using Domain.Comment;
using Domain.DocReview;

namespace DAL.Repositories.DocReview;

/// <summary>
/// The repository interface for <see cref="Domain.DocReview.DocReviewHistory"/>.
/// </summary>
public interface IDocReviewHistoryRepository
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads a single <see cref="DocReviewHistory"/>
    /// </summary>
    /// <param name="id">The id of the object.</param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="DocReviewHistory.DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="DocReviewHistory.Editor"/></param>
    /// <returns></returns>
    public DocReviewHistory ReadDocReviewHistory(int id, bool includeDocReview = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the docReview histories for a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="DocReviewHistory.DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="DocReviewHistory.Editor"/></param>
    /// <returns></returns>
    public IEnumerable<DocReviewHistory> ReadCommentHistoriesBydProject(Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the DocReview histories given a <see cref="Domain.User.User"/> and a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="user">The DocReview histories of all this user will be returned.</param>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="DocReviewHistory.DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="DocReviewHistory.Editor"/></param>
    /// <returns></returns>
    public IEnumerable<DocReviewHistory> ReadDocReviewHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Create a new DocReview history.
    /// </summary>
    /// <param name="docReviewHistory"></param>
    /// <returns></returns>
    public DocReviewHistory CreateDocReviewHistory(DocReviewHistory docReviewHistory);
}