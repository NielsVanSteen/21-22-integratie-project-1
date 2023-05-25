using System.ComponentModel.DataAnnotations;
using DAL.Repositories.DocReview;
using Domain.DocReview;

namespace BL.DocReview;

/// <summary>
/// <see cref="IDocReviewHistoryManager"/>
/// </summary>
public class DocReviewHistoryManager : IDocReviewHistoryManager
{
    // Fields.
    private readonly IDocReviewHistoryRepository _repository;
    
    // Constructor.
    public DocReviewHistoryManager(IDocReviewHistoryRepository repository)
    {
        _repository = repository;
    } // DocReviewHistoryManager.

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryManager.GetDocReviewHistory"/>.
    /// </summary>
    public DocReviewHistory GetDocReviewHistory(int id, bool includeDocReview = false, bool includeUser = false)
    {
        return _repository.ReadDocReviewHistory(id, includeDocReview, includeUser);
    } // GetDocReviewHistory.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryManager.GetDocReviewHistoriesBydProject"/>.
    /// </summary>
    public IEnumerable<DocReviewHistory> GetDocReviewHistoriesBydProject(Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false)
    {
        return _repository.ReadCommentHistoriesBydProject(project, includeDocReview, includeUser);
    } // ReadCommentHistoriesBydProject.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryManager.GetDocReviewHistoriesByUserAndProject"/>.
    /// </summary>
    public IEnumerable<DocReviewHistory> GetDocReviewHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false)
    {
        return _repository.ReadDocReviewHistoriesByUserAndProject(user, project, includeDocReview, includeUser);
    } // GetDocReviewHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryManager.AddDocReviewHistory"/>.
    /// </summary>
    public DocReviewHistory AddDocReviewHistory(DocReviewHistory docReviewHistory)
    {
        Validator.ValidateObject(docReviewHistory, new ValidationContext(docReviewHistory), validateAllProperties: true);
        return _repository.CreateDocReviewHistory(docReviewHistory);
    } // AddDocReviewHistory.
}