using Domain.Comment;

namespace DAL.Repositories.Comment;

/// <summary>
/// The repository interface for <see cref="Domain.Comment.CommentHistory"/>.
/// </summary>

public interface ICommentHistoryRepository
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads a single <see cref="CommentHistory"/>
    /// </summary>
    /// <param name="id">The id of the object.</param>
    /// <param name="includeReactionGroup">Whether or not to include the navigation property for <see cref="CommentHistory.ReactionGroup"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="CommentHistory.EditedBy"/></param>
    /// <returns></returns>
    public CommentHistory ReadCommentHistory(int id, bool includeReactionGroup = false, bool includeUser = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the comment histories for a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeReactionGroup">Whether or not to include the navigation property for <see cref="CommentHistory.ReactionGroup"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="CommentHistory.EditedBy"/></param>
    /// <returns></returns>
    public IEnumerable<CommentHistory> ReadCommentHistoriesBydProject(Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the comment histories given a <see cref="Domain.User.User"/> and a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="user">The comment histories of all this user will be returned.</param>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeReactionGroup">Whether or not to include the navigation property for <see cref="CommentHistory.ReactionGroup"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="CommentHistory.EditedBy"/></param>
    /// <returns></returns>
    public IEnumerable<CommentHistory> ReadCommentHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Create a new comment history.
    /// </summary>
    /// <param name="commentHistory"></param>
    /// <returns></returns>
    public CommentHistory CreateCommentHistory(CommentHistory commentHistory);

    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Select per doc-review (or if null for all doc-reviews) the amount of comments with a specific status.
    /// </summary>
    /// <param name="docReview"></param>
    /// <param name="commentStatus"></param>
    /// <returns></returns>
    public Dictionary<Domain.DocReview.DocReview, int> GetCommentStatisticsByDocReviewAndStatus(Domain.DocReview.DocReview docReview,
        CommentStatus commentStatus);

}