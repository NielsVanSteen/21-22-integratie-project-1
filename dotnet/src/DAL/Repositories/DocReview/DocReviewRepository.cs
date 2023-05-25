using Domain.DocReview;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.DocReview;

/// <summary>
/// <see cref="IDocReviewRepository"/>
/// </summary>
public class DocReviewRepository : Repository, IDocReviewRepository
{
    // Constructor.
    public DocReviewRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.ReadDocReview"/>
    /// </summary>
    public Domain.DocReview.DocReview ReadDocReview(int id, bool includeWrittenBy = false, bool includeHistory = false,
        bool includeSurveys = false, bool includeProject = false)
    {
        IQueryable<Domain.DocReview.DocReview> docReviews = Context.DocReviews;

        if (includeWrittenBy)
            docReviews = docReviews.Include(d => d.WrittenBy);
        if (includeHistory)
            docReviews = docReviews.Include(d => d.DocReviewHistories);
        if (includeSurveys)
            docReviews = docReviews.Include(d => d.Surveys).ThenInclude(s => s.SurveyOptions);
        if (includeProject)
            docReviews = docReviews.Include(d => d.Project);
        
        return docReviews.SingleOrDefault(dr => dr.DocReviewId == id);
    } // ReadDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.ReadDocReviewsWithoutAPhaseByProject"/>
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewsWithoutAPhaseByProject(Domain.Project.Project project,
        bool includeWrittenBy = false, bool includeHistory = false)
    {
        var docReviews = Context.DocReviews.AsQueryable();

        if (includeWrittenBy)
            docReviews = docReviews.Include(d => d.WrittenBy);
        if (includeHistory)
            docReviews = docReviews.Include(d => d.DocReviewHistories);
        
        return docReviews.Where(d => d.TimeLinePhases == null && d.Project == project);
    } // ReadDocReviewsWithoutAPhaseByProject.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.CreateDocReview"/>
    /// </summary>
    public Domain.DocReview.DocReview CreateDocReview(Domain.DocReview.DocReview docReview)
    {
        Context.DocReviews.Add(docReview);
        Context.Entry(docReview.WrittenBy).State =
            EntityState.Unchanged; // Set the user navigation property to unchanged -> user already exists and shouldn't be added twice.
        Context.SaveChanges();
        return docReview;
    } // CreateDocReview.


    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.ReadDocReviewsByProject"/>
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewsByProject(int projectId, bool includeUser = false,
        bool includeEmojis = false, bool includeHistory = false)
    {
        IQueryable<Domain.DocReview.DocReview> docReviews = Context.DocReviews;

        if (includeUser)
            docReviews = docReviews.Include(d => d.WrittenBy);

        if (includeEmojis)
            docReviews = docReviews.Include(d => d.AvailableEmoji);

        if (includeHistory)
            docReviews = docReviews.Include(d => d.DocReviewHistories);

        return docReviews.Where(p => p.Project.ProjectId == projectId).AsEnumerable();
    } // ReadDocReviewsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.ReadDocReviewByProjectAndStatus"/>
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> ReadDocReviewByProjectAndStatus(Domain.Project.Project project,
        DocReviewStatus docReviewStatus, bool includeProject = false, bool includePhase = false,
        bool includeAvailableEmoji = false, bool includeWrittenBy = false, bool includeSurveys = false,
        bool includeHistories = false)
    {
        IQueryable<Domain.DocReview.DocReview> docReviews = Context.DocReviews;

        if (includeProject)
            docReviews = docReviews.Include(d => d.Project);

        if (includePhase)
            docReviews = docReviews.Include(d => d.TimeLinePhases);

        if (includeAvailableEmoji)
            docReviews = docReviews.Include(d => d.AvailableEmoji);

        if (includeWrittenBy)
            docReviews = docReviews.Include(d => d.WrittenBy);

        if (includeSurveys)
            docReviews = docReviews.Include(d => d.Surveys);

        if (includeHistories)
            docReviews = docReviews.Include(d => d.DocReviewHistories);


        // Return all the doc-reviews given the project & where the status is a specific status.
        return docReviews
            .Where(d => d.DocReviewHistories
                    .OrderBy(h => h.EditedOn)
                    .Reverse()
                    .First().DocReviewStatus == docReviewStatus
            )
            .Where(d => d.Project == project)
            .AsEnumerable();
    } // ReadDocReviewByProjectAndStatus.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.UpdateDocReview"/>
    /// </summary>
    public void UpdateDocReview(Domain.DocReview.DocReview docreviewUpdate)
    {
        Domain.DocReview.DocReview docReview = Context.DocReviews
            // .Include(d => d.Project)
            // .Include(d => d.WrittenBy)
            .Find(docreviewUpdate.DocReviewId);
        if (docReview != null)
        {
            docReview.Name = docreviewUpdate.Name;
            docReview.Description = docreviewUpdate.Description;
            docReview.DocReviewText = docreviewUpdate.DocReviewText;
            docReview.TimeLinePhases = docReview.TimeLinePhases;
            docReview.DocReviewSettings = docreviewUpdate.DocReviewSettings;
            docReview.DocReviewHistories = docreviewUpdate.DocReviewHistories;
        }

        Context.SaveChanges();
    } // UpdateDocReview.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IDocReviewRepository.CreateDocReviewHistory"/>
    /// </summary>
    public void CreateDocReviewHistory(DocReviewHistory history)
    {
        Context.DocReviewHistories.Add(history);
        Context.SaveChanges();
    } // CreateDocReviewHistory.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IDocReviewRepository.DeleteDocReview"/>
    /// </summary>
    /// <param name="docReview"></param>
    public void DeleteDocReview(Domain.DocReview.DocReview docReview)
    {
        Context.DocReviews.Remove(docReview);
        Context.SaveChanges();
    } // DeleteDocReview.
}