using DocReviewHistory = Domain.DocReview.DocReviewHistory;
using DocReviewStatus = Domain.DocReview.DocReviewStatus;

namespace DAL.Repositories.DocReview;

/// /// <summary>
/// The repository interface for all the classes concerning a <see cref="Domain.DocReview.DocReview"/>.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.DocReview.DocReview"/>
/// <see cref="Domain.DocReview.DocReviewSetting"/>
/// <see cref="Domain.DocReview.Emoji"/>
/// </example>
public interface IDocReviewRepository
{
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// Returns a <see cref="DocReview"/> object given the Id.
    /// </summary>
    /// <param name="id">The Id of a <see cref="DocReview"/></param>
    /// <param name="includeWrittenBy"> Whether or not to include the navigation property <see cref="Domain.DocReview.DocReview.WrittenBy"/> </param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="Domain.DocReview.DocReview.DocReviewHistories"/></param>
    /// <param name="includeSurveys">Whether or not to include the naviagtion property for <see cref="Domain.DocReview.DocReview.Surveys"/></param>
    /// <param name="includeProject">Whether or not to include the naviagtion property for <see cref="Project"/></param>
    /// <returns>A <see cref="DocReview"/> </returns>
    public Domain.DocReview.DocReview ReadDocReview(int id, bool includeWrittenBy = false, bool includeHistory = false, bool includeSurveys = false, bool includeProject = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all doc-reviews for a project that are not linked to a timeline phase.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="includeWrittenBy"> Whether or not to include the navigation property <see cref="Domain.DocReview.DocReview.WrittenBy"/> </param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="Domain.DocReview.DocReview.DocReviewHistories"/></param>
    /// <returns></returns>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewsWithoutAPhaseByProject(Domain.Project.Project project, bool includeWrittenBy = false, bool includeHistory = false);

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// Add a <see cref="Domain.DocReview.DocReview"/> to the database.
    /// </summary>
    /// <param name="docReview">The <see cref="Domain.DocReview.DocReview"/> to add. </param>
    public Domain.DocReview.DocReview CreateDocReview(Domain.DocReview.DocReview docReview);
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Returns all the DocReviews that belong to an project.
    /// </summary>
    /// <param name="projectId">The id of the project we want all the <see cref="Domain.DocReview.DocReview"/> for.</param>
    /// <param name="includeUser">To choose if you also want the users data</param>
    /// <param name="includeEmojis">To choose if you also want the emojis data</param>
    /// <returns>A list of <see cref="DocReview"/></returns>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewsByProject(int projectId, bool includeUser = false, bool includeEmojis = false,  bool includeHistory = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get all the doc-reviews for a specific project and a specific status.
    ///
    /// The status is determined by ordering all the histories on date and taking the latest.
    /// </summary>
    /// <param name="project">The project to get all the doc-reviews for.</param>
    /// <param name="docReviewStatus">The status to get all doc-reviews with.</param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.Project"/></param>
    /// <param name="includePhase">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.TimeLinePhase"/></param>
    /// <param name="includeAvailableEmoji">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.AvailableEmoji"/></param>
    /// <param name="includeWrittenBy">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.WrittenBy"/></param>
    /// <param name="includeSurveys">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.Surveys"/></param>
    /// <param name="includeHistories">Whether or not to include the navigation property for <see cref="Domain.DocReview.DocReview.DocReviewHistories"/>></param>
    /// <returns></returns>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewByProjectAndStatus(Domain.Project.Project project, DocReviewStatus docReviewStatus, bool includeProject = false, bool includePhase = false,
        bool includeAvailableEmoji = false, bool includeWrittenBy = false, bool includeSurveys = false, bool includeHistories = false);
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Update a DocReview..
    /// </summary>
    /// <param name="docReview">The <see cref="Domain.DocReview.DocReview"/> you want to update.</param>
    public void UpdateDocReview(Domain.DocReview.DocReview docReview);

    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Create a <see cref="DocReviewHistory"/> to change the status of a DocReview.
    /// </summary>
    /// <param name="history"><see cref="DocReviewHistory"/></param>
    public void CreateDocReviewHistory(DocReviewHistory history);

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Delete a <see cref="DocReview"/> from the database
    /// </summary>
    /// <param name="docReview">The <see cref="DocReview"/> to be deleted</param>
    public void DeleteDocReview(Domain.DocReview.DocReview docReview);
}