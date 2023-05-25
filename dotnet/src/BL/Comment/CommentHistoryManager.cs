using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Comment;
using Domain.Comment;

namespace BL.Comment;

/// <summary>
/// <see cref="ICommentHistoryManager"/>
/// </summary>
public class CommentHistoryManager : ICommentHistoryManager
{
    // Fields.
    private ICommentHistoryRepository _repository;

    // Constructor.
    public CommentHistoryManager(ICommentHistoryRepository repo)
    {
        _repository = repo;
    } // CommentHistoryManager.

    // Methods
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryManager.GetCommentHistory"/>
    /// </summary>
    public CommentHistory GetCommentHistory(int id, bool includeReactionGroup = false, bool includeUser = false)
    {
        return _repository.ReadCommentHistory(id, includeReactionGroup, includeUser);
    } // GetCommentHistory.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryManager.GetCommentHistoriesBydProject"/>
    /// </summary>
    public IEnumerable<CommentHistory> GetCommentHistoriesBydProject(Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false)
    {
        return _repository.ReadCommentHistoriesBydProject(project, includeReactionGroup, includeUser);
    } // GetCommentHistoriesBydProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryManager.GetCommentHistoriesByUserAndProject"/>
    /// </summary>
    public IEnumerable<CommentHistory> GetCommentHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false)
    {
        return _repository.ReadCommentHistoriesByUserAndProject(user, project, includeReactionGroup, includeUser);
    } // GetCommentHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryManager.AddCommentHistory"/>
    /// </summary>
    public CommentHistory AddCommentHistory(CommentHistory commentHistory)
    {
        Validator.ValidateObject(commentHistory, new ValidationContext(commentHistory), validateAllProperties: true);
        return _repository.CreateCommentHistory(commentHistory);
    } // AddCommentHistory.

    /// <summary>
    /// <see cref="ICommentHistoryManager.GetCommentStatisticsByDocReviewAndStatus"/>
    /// </summary>
    /// <param name="docReview"></param>
    /// <param name="commentStatus"></param>
    /// <returns></returns>
    public Dictionary<Domain.DocReview.DocReview, int> GetCommentStatisticsByDocReviewAndStatus(Domain.DocReview.DocReview docReview,
        CommentStatus commentStatus)
    {
        return _repository.GetCommentStatisticsByDocReviewAndStatus(docReview, commentStatus);
    } // GetCommentStatisticsByDocReviewAndStatus.

}