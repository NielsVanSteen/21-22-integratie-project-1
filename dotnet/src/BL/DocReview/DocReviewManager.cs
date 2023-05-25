using System.ComponentModel.DataAnnotations;
using DAL.Repositories.DocReview;
using Domain.DocReview;

namespace BL.DocReview;

/// <summary>
/// <see cref="IDocReviewManager"/>
/// </summary>
public class DocReviewManager : IDocReviewManager
{
    // Fields.
    private readonly IDocReviewRepository _repository;

    // Constructor.
    public DocReviewManager(IDocReviewRepository repository)
    {
        _repository = repository;
    } // DocReviewManager.

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewManager.GetDocReview"/>
    /// </summary>
    public Domain.DocReview.DocReview GetDocReview(int id, bool includeWrittenBy = false, bool includeHistory = false,
        bool includeSurveys = false, bool includeProject = false)
    {
        return _repository.ReadDocReview(id, includeWrittenBy, includeHistory, includeSurveys, includeProject);
    } // GetDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewManager.GetDocReview"/>
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> GetDocReviewsWithoutAPhaseByProject(Domain.Project.Project project, bool includeWrittenBy = false, bool includeHistory = false)
    {
        return _repository.ReadDocReviewsWithoutAPhaseByProject(project, includeWrittenBy, includeHistory);
    } // GetDocReviewsWithoutAPhaseByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewManager.AddDocReview"/>
    /// </summary>
    public Domain.DocReview.DocReview AddDocReview(Domain.DocReview.DocReview docReview)
    {
        Validator.ValidateObject(docReview, new ValidationContext(docReview), validateAllProperties: true);
        return _repository.CreateDocReview(docReview);
    } // CreateDocReview.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewManager.GetDocReviewsByProject"/>
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> GetDocReviewsByProject(int projectId, bool includeUser = false,
        bool includeEmojis = false, bool includeHistory = false)
    {
        return _repository.ReadDocReviewsByProject(projectId, includeUser, includeEmojis, includeHistory);
    } // GetDocReviewsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get all the doc-reviews for a specific project and a specific status.
    ///
    /// The status is determined by ordering all the histories on date and taking the latest.
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> GetDocReviewByProjectAndStatus(Domain.Project.Project project,
        DocReviewStatus docReviewStatus, bool includeProject = false, bool includePhase = false,
        bool includeAvailableEmoji = false, bool includeWrittenBy = false, bool includeSurveys = false,
        bool includeHistories = false)
    {
        return _repository.ReadDocReviewByProjectAndStatus(project, docReviewStatus, includeProject, includePhase,
            includeAvailableEmoji, includeWrittenBy, includeSurveys, includeHistories);
    } // GetDocReviewByProjectAndStatus.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewManager.ChangeDocReview"/>
    /// </summary>
    public void ChangeDocReview(Domain.DocReview.DocReview docReview)
    {
        Validator.ValidateObject(docReview, new ValidationContext(docReview), validateAllProperties: true);
        _repository.UpdateDocReview(docReview);
    } // ChangeDocReview.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewManager.AddDocReviewHistory"/>
    /// </summary>
    public void AddDocReviewHistory(DocReviewHistory history)
    {
        Validator.ValidateObject(history, new ValidationContext(history), validateAllProperties: true);
        _repository.CreateDocReviewHistory(history);
    } // AddDocReviewHistory.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IDocReviewManager.RemoveDocReview"/>
    /// </summary>
    /// <param name="docReview"></param>
    public void RemoveDocReview(Domain.DocReview.DocReview docReview)
    {
        _repository.DeleteDocReview(docReview);
    } // RemoveDocReview.
}