using Domain.DocReview;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.DocReview;

/// <summary>
/// <see cref="IDocReviewHistoryRepository"/>
/// </summary>
public class DocReviewHistoryRepository : Repository, IDocReviewHistoryRepository
{
    // Constructor.
    public DocReviewHistoryRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryRepository.ReadDocReviewHistory"/>
    /// </summary>
    public DocReviewHistory ReadDocReviewHistory(int id, bool includeDocReview = false, bool includeUser = false)
    {
        IQueryable<DocReviewHistory> histories = Context.DocReviewHistories;

        if (includeDocReview)
            histories = histories.Include(h => h.DocReview);
        
        if (includeUser)
            histories = histories.Include(h => h.Editor);

        return histories.SingleOrDefault(h => h.DocReviewHistoryId == id);
    } // ReadDocReviewHistory.
    
      /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryRepository.ReadCommentHistoriesBydProject"/>
    /// </summary>
    public IEnumerable<DocReviewHistory> ReadCommentHistoriesBydProject(Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false)
    {
         IQueryable<DocReviewHistory> histories = Context.DocReviewHistories;

        if (includeDocReview)
            histories = histories.Include(h => h.DocReview);
        
        if (includeUser)
            histories = histories.Include(h => h.Editor);
          
        return histories.Where(h => h.DocReview.Project == project);
    } // ReadCommentHistoriesBydProject.


    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryRepository.ReadDocReviewHistoriesByUserAndProject"/>
    /// </summary>
    public IEnumerable<DocReviewHistory> ReadDocReviewHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeDocReview = false, bool includeUser = false)
    {
        IQueryable<DocReviewHistory> histories = Context.DocReviewHistories;

        if (includeDocReview)
            histories = histories.Include(h => h.DocReview);
         
        if (includeUser)
            histories = histories.Include(h => h.Editor);

        return histories.Where(h => h.Editor == user && h.DocReview.Project == project);
    } // ReadDocReviewHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IDocReviewHistoryRepository.CreateDocReviewHistory"/>
    /// </summary>
    public DocReviewHistory CreateDocReviewHistory(DocReviewHistory docReviewHistory)
    {
        Context.DocReviewHistories.Add(docReviewHistory);
        Context.SaveChanges();
        return docReviewHistory;
    } // CreateDocReviewHistory.
}