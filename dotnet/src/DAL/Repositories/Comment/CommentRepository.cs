using Domain.Comment;
using Domain.Project;
using Domain.Util;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Comment;

/// <summary>
/// <see cref="ICommentRepository"/>
/// </summary>
public class CommentRepository : Repository, ICommentRepository
{
    // Constructor.
    public CommentRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Apply all the comment includes.
    /// </summary>
    private IQueryable<ReactionGroup> ApplyCommentIncludes(IQueryable<ReactionGroup> comments,
        bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnReactionGroup = false, bool includeReactions = false)
    {
        if (includeDocReview)
            comments = comments.Include(c => c.DocReview);

        if (includeUser)
            comments = comments.Include(r => r.User);

        if (includeTags)
            comments = comments.Include(c => c.CommentTags).ThenInclude(cTag => cTag.ProjectTag);

        if (includeHistory)
            comments = comments.Include(c => c.CommentHistories);

        if (includePlacedOnReactionGroup)
            comments = comments.Include(c => c.PlacedOnReactionGroup).ThenInclude(c => c.User);

        if (includeReactions)
            comments = comments.Include(c => c.Reactions);

        return comments;
    } // ApplyCommentIncludes.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Makes a new comment.
    /// </summary>
    /// <param name="reactionGroup"></param>
    /// <returns></returns>
    public ReactionGroup CreateComment(Domain.Comment.ReactionGroup reactionGroup)
    {
        Context.Comments.Add(reactionGroup);
        Context.Entry(reactionGroup.User).State = EntityState.Unchanged;
        Context.Entry(reactionGroup.DocReview).State = EntityState.Unchanged;

        if (reactionGroup.DocReview != null && reactionGroup.DocReview.WrittenBy != null)
        {
            Context.Entry(reactionGroup.DocReview.WrittenBy).State = EntityState.Unchanged;
        }

        if (reactionGroup.PlacedOnReactionGroup != null && reactionGroup.PlacedOnReactionGroup.User != null)
        {
            Context.Entry(reactionGroup.PlacedOnReactionGroup.User).State = EntityState.Unchanged;
        }

        Context.SaveChanges();
        return reactionGroup;
    } // CreateComment.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.CreateReaction"/>
    /// </summary>
    public EmojiReaction CreateReaction(EmojiReaction reaction)
    {
        Context.EmojiReactions.Add(reaction);
        Context.SaveChanges();
        return reaction;
    } // CreateReaction.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentById"/>
    /// </summary>
    public Domain.Comment.ReactionGroup ReadCommentById(int commentId, bool includeUser = false,
        bool includeDocReview = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false)
    {
        var comments = Context.Comments.AsQueryable();

        comments = ApplyCommentIncludes(comments, includeDocReview, includeUser, includeTags, includeHistory,
            includePlacedOnComment, false);

        return comments.SingleOrDefault(c => c.CommentId == commentId);
    } // ReadCommentById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadReactionById"/>
    /// </summary>
    public EmojiReaction ReadReactionById(int reactionId, bool includeUser = false, bool includeDocReview = false,
        bool includeComment = false, bool includeEmoji = false)
    {
        var reactions = Context.EmojiReactions.AsQueryable();

        if (includeUser)
            reactions = reactions.Include(r => r.User);

        if (includeDocReview)
            reactions = reactions.Include(r => r.DocReview);

        if (includeComment)
            reactions = reactions.Include(r => r.PlacedOnReactionGroup);

        if (includeEmoji)
            reactions = reactions.Include(r => r.Emoji);

        return reactions.SingleOrDefault(r => r.CommentId == reactionId);
    } // ReadReactionById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentsByDocReview"/>
    /// </summary>
    public IEnumerable<ReactionGroup> ReadCommentsByDocReview(int docReviewId, bool includeUser = false,
        bool includeTags = false, bool includeHistory = false, bool includePlacedOnComment = false,
        bool includeReactions = false, bool includeDocReview = false)
    {
        var comments = Context.Comments.AsQueryable();
        
        if (includeUser)
            comments = comments.Include(r => r.User);

        if (includeTags)
            comments = comments.Include(r => r.CommentTags);
        
        if (includeHistory)
            comments = comments.Include(r => r.CommentHistories);
        
        if (includePlacedOnComment)
            comments = comments.Include(r => r.PlacedOnReactionGroup);
        
        if (includeReactions)
            comments = comments.Include(r => r.Reactions);
        
        if (includeDocReview)
            comments = comments.Include(r => r.DocReview);

        return comments.Where(c => c.DocReview.DocReviewId == docReviewId && c.PlacedOnReactionGroup == null);
    } // ReadCommentsByDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentsByDocReview"/>
    /// </summary>
    public IEnumerable<ReactionGroup> ReadCommentsByDocReview(int docReviewId, UserCommentsFilter filter,
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false)
    {
        var comments = ApplyCommentsByDocReviewFilter(filter ,necessaryStatuses, forbiddenStatuses, includeUser,
            includeTags, includeHistory, includePlacedOnComment, includeReactions, includeDocReview);
        
        return filter.OwnComments ? comments.Where(c => c.DocReview.DocReviewId == docReviewId) : comments.Where(c => c.DocReview.DocReviewId == docReviewId && c.PlacedOnReactionGroup == null);
    } // ReadCommentsByDocReview.
    
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadSubCommentsByComment"/>
    /// </summary>
    public IEnumerable<ReactionGroup> ReadSubCommentsByComment(ReactionGroup comment,
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false)
    {
        var comments = ApplyCommentsByDocReviewFilter(null ,necessaryStatuses, forbiddenStatuses, includeUser,
            includeTags, includeHistory, includePlacedOnComment, includeReactions, includeDocReview);
        
          return comments.Where(c => c.PlacedOnReactionGroup == comment);
    } // ReadSubCommentsByDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the sub-comments of a comment. that must have some statuses <see cref="necessaryStatuses"/> and can't have other statues <see cref="forbiddenStatuses"/>
    /// </summary>
    /// <returns></returns>
    private IEnumerable<ReactionGroup> ApplyCommentsByDocReviewFilter(UserCommentsFilter filter,IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false)
    {
        // Get Comments.
        var comments = Context.Comments.AsQueryable();

        // Apply Includes.
        comments = ApplyCommentIncludes(comments, includeDocReview, includeUser, includeTags, includeHistory,
            includePlacedOnComment, includeReactions);

        // Filter statuses.
        //  SELECT * FROM CommentComposites c
        //  WHERE c.CommentId IN (
        //      SELECT h.ReactionGroupId
        //      FROM CommentHistories h
        //      WHERE h.CommentStatus IN(2) AND h.ReactionGroupId = c.CommentId
        //  ) AND c.CommentId NOT IN (
        //      SELECT h.ReactionGroupId
        //      FROM CommentHistories h
        //      WHERE h.CommentStatus IN(6) AND h.ReactionGroupId = c.CommentId
        //  ) AND c.DocReviewId = 1;
        
        

        // Check for the necessary statuses.
        if (necessaryStatuses != null)
            comments = comments.Where(c => c.CommentHistories.Any(h => necessaryStatuses.Contains(h.CommentStatus)));

        // Check for the forbidden statuses.
        if (forbiddenStatuses != null)
            comments = comments.Where(c => !c.CommentHistories.Any(h => forbiddenStatuses.Contains(h.CommentStatus)));

        // Filters.
        if (filter != null)
        {
            comments = ApplyUserCommentsFilterModel(comments, filter);
        }
        
        return comments;
    } // ApplyCommentsByDocReviewFilter.
    
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentsByDocReview"/>
    /// </summary>
    public IEnumerable<Domain.Comment.ReactionGroup> ReadCommentsByUser(string userId, bool includeDocReview = false,
        bool includeTags = false, bool includeHistory = false, bool includePlacedOnComment = false)
    {
        var comments = Context.Comments.AsQueryable();

        comments = ApplyCommentIncludes(comments, includeDocReview, false, includeTags, includeHistory,
            includePlacedOnComment, false);

        return comments.Where(c => c.User.Id == userId);
    } // ReadCommentsByUser.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Returns all the comments by user and doc-review
    /// </summary>
    public IEnumerable<ReactionGroup> ReadCommentsByUserAndDocReview(string userId, int docReviewId,
        bool includeDocReview = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false)
    {
        var comments = Context.Comments.AsQueryable();

        comments = ApplyCommentIncludes(comments, includeDocReview, false, includeTags, includeHistory,
            includePlacedOnComment, false);

        return comments.Where(c => c.User.Id == userId && c.DocReview.DocReviewId == docReviewId);
    } // ReadCommentsByUserAndDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadReactionsOfCommentByComment"/>
    /// </summary>
    public IEnumerable<Reaction> ReadReactionsOfCommentByComment(int commentId, bool includeUser = false,
        bool includeDocReview = false)
    {
        // Get comment with navigation Property 'Reactions'.
        var comment = Context.Comments.Include(c => c.Reactions).SingleOrDefault(c => c.CommentId == commentId);

        // If reactions == null -> return null.
        if (comment?.Reactions == null)
            return null;

        // Check to include other navigation properties.
        IQueryable<Reaction> reactions = comment.Reactions.AsQueryable();

        if (includeUser)
            reactions = reactions.Include(r => r.User);

        if (includeDocReview)
            reactions = reactions.Include(r => r.DocReview);

        return reactions;
    } // ReadReactionsOfCommentByComment.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadEmojiReactionsByComment"/>
    /// </summary>
    public IEnumerable<EmojiReaction> ReadEmojiReactionsByComment(int id)
    {
        IQueryable<EmojiReaction> emojiReactions = Context.EmojiReactions;
        return emojiReactions.Include(e => e.Emoji).Where(e => e.PlacedOnReactionGroupId == id);
    } // ReadEmojiReactionsByComment.

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentsOfProject"/>
    /// </summary>
    public IEnumerable<ReactionGroup> ReadCommentsOfProject(int id, CommentsFilterModel filterModel,
        bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includeReactions = false, bool includeReactionUser = false)
    {
        var comments = Context.Comments.Where(c => c.DocReview.ProjectId == id);

        // Includes.
        comments = ApplyCommentIncludes(comments, includeDocReview, includeUser, includeTags, includeHistory,
            includeReactionUser, includeReactions);

        // Filters.
        comments = ApplyCommentsFilterModel(comments, filterModel);

        // Sorting.
        comments = ApplyCommentsSorting(comments, filterModel.SortOn, filterModel.SortOrder);

        // Paging.
        comments = ApplyCommentsPagination(comments, filterModel.PageSize, filterModel.PageNumber);

        return comments;
    } // ReadCommentsOfProject.

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadCommentTotalByProject"/>
    /// </summary>>
    public int ReadCommentTotalByProject(Domain.Project.Project project, CommentsFilterModel filterModel = null)
    {
        var comments = Context.Comments.Where(c => c.DocReview.Project == project);

        if (filterModel != null)
            comments = ApplyCommentsFilterModel(comments, filterModel);

        return comments.Count();
    } // ReadCommentTotalByProject.

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadEmojisOfComment"/>
    /// </summary>>
    public IEnumerable<CommentEmojiTotals> ReadEmojisOfComment(int id, Domain.User.User currentLoggedInUser)
    {
        // SQL.
        
        //  SELECT c.EmojiId, COUNT(*) AS 'Amount', c.PlacedOnReactionGroupId AS 'CommentId', (  	
        //   	
        //  SELECT c2.userId
        //  FROM CommentComposites c2
        //  WHERE c2.CommentId IN (
        //         
        //      SELECT c3.CommentId
        //         FROM CommentComposites c3
        //         WHERE c3.EmojiId IS NOT NULL
        //         AND c3.CommentId = c2.CommentId
        //      ) 
        //   	AND c2.PlacedOnReactionGroupId = c.PlacedOnReactionGroupId
        //   	AND c2.UserId = '5938ddaa-f5e7-4259-8ebe-2b35c14326d8'
        //   
        //  ) AS 'Has User Like That Emoji'
        //  FROM CommentComposites c
        //  INNER JOIN Emojis e
        //  ON c.EmojiId = e.EmojiId
        //  WHERE c.PlacedOnReactionGroupId = 1
        //  GROUP BY c.EmojiId;
        
        // LINQ.
        var result = Context.EmojiReactions
            .Include(e => e.Emoji)
            .Where(e => e.PlacedOnReactionGroupId == id)
            .GroupBy(e => e.EmojiId)
            .Select(e => new CommentEmojiTotals()
            {
                EmojiId = e.Key,
                EmojiCode = e.First().Emoji.Code,
                Count = e.Count(),
                HasUserLikeThatEmoji = e.Any(r => r.UserId == currentLoggedInUser.Id),
                CommentId = e.First().PlacedOnReactionGroupId ?? 0
            });
        
        return result;
    } // ReadEmojisOfComment.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadEmojiReactionByCommentAndEmoji"/>
    /// </summary>
    public EmojiReaction ReadEmojiReactionByCommentAndEmoji(int id, int emojiId, string userId)
    {
        var result = Context.EmojiReactions.Include(reaction => reaction.Emoji).FirstOrDefault(reaction =>
            reaction.PlacedOnReactionGroupId == id && reaction.EmojiId == emojiId && reaction.UserId.Equals(userId));
        return result;
    } // ReadEmojiReactionByCommentAndEmoji.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadSubCommentsByComment"/>
    /// </summary>
    public IEnumerable<ReactionGroup> ReadSubCommentsByComment(int id, bool includeDocReview = false,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false)
    {
        IQueryable<ReactionGroup> comments = Context.Comments;

        comments = ApplyCommentIncludes(comments, includeDocReview, includeUser, includeTags, includeHistory, false,
            includeReactions);

        return comments.Where(c => c.PlacedOnReactionGroupId == id);
    } // ReadSubCommentsByComment.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentRepository.UpdateComment"/>
    /// </summary>
    public void UpdateComment(ReactionGroup reactionGroup)
    {
        var comment = Context.Comments.Find(reactionGroup.CommentId);
        if (comment != null)
        {
            comment.CommentText = reactionGroup.CommentText;
            comment.BeginChar = reactionGroup.BeginChar;
            comment.EndChar = reactionGroup.EndChar;
        }
    } // UpdateComment.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Applies the correct filters to the comments.
    /// </summary>
    private IQueryable<ReactionGroup> ApplyCommentsFilterModel(IQueryable<ReactionGroup> comments,
        CommentsFilterModel filterModel)
    {
        // Filter on search text.
        if (filterModel.SearchText != null)
            comments = comments.Where(c =>
                c.User.Firstname.Contains(filterModel.SearchText) || c.User.Lastname.Contains(filterModel.SearchText) ||
                c.CommentText.Contains(filterModel.SearchText));

        // Filter on comment status -> is a little bit more complex, since we should only check the latest comment status.
        var list = filterModel.CommentStatus?.ToList() ?? new List<CommentStatus>();
        if (list.Any())
        {
            comments = comments.Where(c => c.CommentHistories
                .Any(h => h.EditedOn == c.CommentHistories
                    .Where(h2 => h2.ReactionGroupId == c.CommentId)
                    .Max(h2 => h2.EditedOn) && list.Contains(h.CommentStatus)));
        } // If.

        // Filter on doc-reviews.
        if (filterModel.DocReviews != null && filterModel.DocReviews.Any())
            comments = comments.Where(c => filterModel.DocReviews.Any(d => d == c.DocReviewId));

        // Filter on project tags.
        if (filterModel.ProjectTags != null && filterModel.ProjectTags.Any())
            comments = comments.Where(c => c.CommentTags.Any(t => filterModel.DocReviews.Contains(t.ProjectTagId)));

        return comments;
    } // ApplyCommentsFilterModel.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Applies the correct sorting to the comments.
    /// </summary>
    private IQueryable<ReactionGroup> ApplyCommentsSorting(IQueryable<ReactionGroup> comments,
        SortOn sortOn, SortOrder sortOrder)
    {
        // Sort the comments.
        switch (sortOn)
        {
            case SortOn.Date:
                comments = comments.OrderBy(c => c.CommentId);
                break;
            case SortOn.Popularity:
                comments = comments.OrderByDescending(c => c.Reactions.Count);
                break;
        }

        // Reverse when the sort order is descending.
        if (sortOrder == SortOrder.Descending)
            comments = comments.Reverse();

        return comments;
    } // ApplyCommentsSorting.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Applies pagination to the comments query.
    /// </summary>
    private IQueryable<ReactionGroup> ApplyCommentsPagination(IQueryable<ReactionGroup> comments,
        int? pageSize, int? pageNumber)
    {
        if (pageNumber != null && pageSize != null)
            comments = comments.Skip(((pageNumber ?? 0) - 1) * pageSize ?? 0)
                .Take(pageSize ?? 0);

        return comments;
    } // ApplyCommentsPagination.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Apply the filtering for the user comment section.
    /// </summary>
    private IQueryable<ReactionGroup> ApplyUserCommentsFilterModel(IQueryable<ReactionGroup> comments,
        UserCommentsFilter filter)
    {
        comments = comments.Where(group => group.DocReviewId == filter.DocReviewId);
        if (filter.OwnComments && filter.UserId != "")
        {
            comments = comments.Where(group => group.UserId == filter.UserId);
        }

        comments = ApplyCommentsSorting(comments, filter.SortOn, filter.SortOrder);
        comments = ApplyCommentsPagination(comments, filter.PageSize, filter.PageNumber);
        return comments;
    } // ApplyUserCommentsFilterModel.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// <see cref="ICommentRepository.ReadEmojiReactionsByCommentAndUser"/>
    /// </summary>
    public EmojiReaction ReadEmojiReactionsByCommentAndUser(int id, string userId)
    {
        var result = Context.EmojiReactions.Include(reaction => reaction.Emoji).FirstOrDefault(reaction =>
            reaction.PlacedOnReactionGroupId == id && reaction.UserId.Equals(userId));
        return result;
    } // ReadEmojiReactionsByCommentAndUser.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// <see cref="ICommentRepository.DeleteReaction"/>
    /// </summary>
    public void DeleteReaction(EmojiReaction reaction)
    {
        Context.EmojiReactions.Remove(reaction);
        Context.SaveChanges();
    } // DeleteReaction.
}