using Domain.Comment;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Comment;

/// <summary>
/// <see cref="ICommentHistoryRepository"/>
/// </summary>
public class CommentHistoryRepository : Repository, ICommentHistoryRepository
{
    // Constructor.
    public CommentHistoryRepository(DocReviewDbContext context) : base(context) { }
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryRepository.ReadCommentHistory"/>
    /// </summary>
    public CommentHistory ReadCommentHistory(int id, bool includeReactionGroup = false, bool includeUser = false)
    {
        IQueryable<CommentHistory> histories = Context.CommentHistories;

        if (includeReactionGroup)
            histories = histories.Include(h => h.ReactionGroup);
        
        if (includeUser)
            histories = histories.Include(h => h.EditedBy);

        return histories.SingleOrDefault(h => h.CommentHistoryId == id);
    } // ReadCommentHistory.
    
       /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryRepository.ReadCommentHistoriesBydProject"/>
    /// </summary>
    public IEnumerable<CommentHistory> ReadCommentHistoriesBydProject(Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false) {
         IQueryable<CommentHistory> histories = Context.CommentHistories;

        if (includeReactionGroup)
            histories = histories.Include(h => h.ReactionGroup);
        
        if (includeUser)
            histories = histories.Include(h => h.EditedBy);
        
        return histories.Where(h => h.ReactionGroup.DocReview.Project == project);
    } // ReadCommentHistoriesBydProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryRepository.ReadCommentHistoriesByUserAndProject"/>
    /// </summary>
    public IEnumerable<CommentHistory> ReadCommentHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false)
    {
        IQueryable<CommentHistory> histories = Context.CommentHistories;

        if (includeReactionGroup)
            histories = histories.Include(h => h.ReactionGroup);
        
        if (includeUser)
            histories = histories.Include(h => h.EditedBy);

        return histories.Where(h => h.EditedBy == user && h.ReactionGroup.DocReview.Project == project);
    } // ReadCommentHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryRepository.CreateCommentHistory"/>
    /// </summary>
    public CommentHistory CreateCommentHistory(CommentHistory commentHistory)
    {
        Context.CommentHistories.Add(commentHistory);
        Context.SaveChanges();
        return commentHistory;
    } // CreateCommentHistory.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentHistoryRepository.GetCommentStatisticsByDocReviewAndStatus"/>
    /// </summary>
    public Dictionary<Domain.DocReview.DocReview, int> GetCommentStatisticsByDocReviewAndStatus(Domain.DocReview.DocReview docReview, CommentStatus commentStatus)
    {
        // SQL.
    
        //  SELECT COUNT(*), c.DocReviewId, d.Name, p.projectId, p.ExternalName
        //  FROM CommentComposites c
        //  INNER JOIN DocReviews d
        //  ON d.DocReviewId = c.DocReviewId
        //  INNER JOIN Projects p
        //  ON p.ProjectId = d.ProjectId
        //  WHERE c.CommentId = (
        // 	 SELECT h.ReactionGroupId
        //   	FROM CommentHistories h
        //   	WHERE h.CommentStatus=3 AND h.ReactionGroupId = c.CommentId
        //  )
        //  AND d.DocReviewId = 1
        //  GROUP BY c.DocReviewId;
        // LINQ.
        var result = 
            Context.CommentComposites
        
                // Get all comments that have the given comment status.
                .Where(c => c.CommentId == Context.CommentHistories
                    .Where(h => h.CommentStatus == commentStatus && h.ReactionGroupId == c.CommentId)
                    .Select(h => h.ReactionGroupId)
                    .FirstOrDefault()
                )
        
                // Check if the doc-review is the same as the given doc-review, or if null -> get all doc-reviews.
                .Where(c => docReview == null ? c.DocReview != null : c.DocReview == docReview )
        
                // Group query by doc-review.
                .GroupBy(c => c.DocReview.DocReviewId)
        
                // Select only the necessary information as key value pair.
                .Select(group => new KeyValuePair<Domain.DocReview.DocReview,int>(
                    group.FirstOrDefault().DocReview,
                    group.Count()
                )).ToDictionary(x => x.Key, x => x.Value);

        return result;
    } // GetSomeStatistics.
}