using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Comment;
using Domain.Comment;
using Domain.Project;
using Domain.Util;

namespace BL.Comment;

/// <summary>
/// <see cref="ICommentManager"/>
/// </summary>
public class CommentManager : ICommentManager
{
    // Fields.
    private ICommentRepository _repository;

    // Constructor.
    public CommentManager(ICommentRepository repo)
    {
        _repository = repo;
    }

    // Methods

    /// <summary>
    /// <see cref="ICommentManager.AddComment"/>
    /// </summary>
    public ReactionGroup AddComment(Domain.Comment.ReactionGroup reactionGroup)
    {
        Validator.ValidateObject(reactionGroup, new ValidationContext(reactionGroup), validateAllProperties: true);
        return _repository.CreateComment(reactionGroup);
    } // AddComment.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.AddReaction"/>
    /// </summary>
    public EmojiReaction AddReaction(EmojiReaction reaction)
    {
        Validator.ValidateObject(reaction, new ValidationContext(reaction), validateAllProperties: true);
        return _repository.CreateReaction(reaction);
    } // AddReaction.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentById"/>
    /// </summary>
    public Domain.Comment.ReactionGroup GetCommentById(int commentId, bool includeUser = false,
        bool includeDocReview = false, bool includeTags = false, bool includeHistory = false,
        bool includeReactions = false, bool includeReactionUser = false)
    {
        return _repository.ReadCommentById(commentId, includeUser, includeDocReview, includeTags, includeHistory,
            includeReactions);
    } // GetCommentById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetReactionById"/>
    /// </summary>
    public EmojiReaction GetReactionById(int reactionId, bool includeUser = false, bool includeDocReview = false,
        bool includeComment = false, bool includeEmoji = false)
    {
        return _repository.ReadReactionById(reactionId, includeUser, includeDocReview, includeComment, includeEmoji);
    } // GetReactionById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsByDocReview"/>
    /// </summary>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByDocReview(int docReviewId, bool includeUser = false,
        bool includeTags = false,
        bool includeHistory = false, bool includePlacedOnComment = false, bool includeReactions = false,
        bool includeDocreview = false)
    {
        return _repository.ReadCommentsByDocReview(docReviewId, includeUser, includeTags, includeHistory,
            includePlacedOnComment, includeReactions, includeDocreview);
    } // GetCommentsByDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsByDocReview(int, IEnumerable{CommentStatus}, IEnumerable{CommentStatus}, bool, bool, bool, bool, bool, bool)"/>
    /// </summary>
    
    public IEnumerable<ReactionGroup> GetCommentsByDocReview(int docReviewId, UserCommentsFilter filter, IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false)
    {
        return _repository.ReadCommentsByDocReview(docReviewId, filter, necessaryStatuses, forbiddenStatuses, includeUser,
            includeTags, includeHistory, includePlacedOnComment, includeReactions, includeDocReview);
    } // GetCommentsByDocReview.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetSubCommentsByComment"/>
    /// </summary>
    public IEnumerable<ReactionGroup> GetSubCommentsByComment(ReactionGroup comment,
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false)
    {
        return _repository.ReadSubCommentsByComment(comment, necessaryStatuses, forbiddenStatuses, includeUser,
            includeTags, includeHistory, includePlacedOnComment, includeReactions, includeDocReview);
    } // GetSubCommentsByComment.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsByUser"/>
    /// </summary>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByUser(string userId, bool includeUser = false,
        bool includeTags = false,
        bool includeHistory = false, bool includeReactions = false, bool includeReactionUser = false)
    {
        return _repository.ReadCommentsByUser(userId, includeUser, includeTags, includeHistory,
            includeReactions);
    } // GetCommentsByUser.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsByUser"/>
    /// </summary>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByUserAndDocReview(string userId, int docReviewId,
        bool includeDocReview = false,
        bool includeTags = false, bool includeHistory = false, bool includeReactions = false,
        bool includeReactionUser = false)
    {
        return _repository.ReadCommentsByUserAndDocReview(userId, docReviewId, includeDocReview, includeTags,
            includeHistory,
            includeReactions);
    } // GetCommentsByUserAndDocReview.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsByDocReview"/>
    /// </summary>
    public IEnumerable<Reaction> GetReactionsOfCommentByComment(int commentId, bool includeUser = false,
        bool includeDocReview = false)
    {
        return _repository.ReadReactionsOfCommentByComment(commentId, includeUser, includeDocReview);
    } // GetReactionsOfCommentByComment.

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsOfProject"/>
    /// </summary>    
    public IEnumerable<ReactionGroup> GetCommentsOfProject(int id, CommentsFilterModel filterModel = null,
        bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includeReactions = false, bool includeReactionUser = false)
    {
        return _repository.ReadCommentsOfProject(id, filterModel, includeDocReview, includeUser, includeTags,
            includeHistory, includeReactions, includeReactionUser);
    } // GetCommentsOfProject

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetCommentsOfProject"/>
    /// </summary> 
    public int GetCommentTotalByProject(Domain.Project.Project project, CommentsFilterModel filterModel = null)
    {
        return _repository.ReadCommentTotalByProject(project, filterModel);
    } // GetCommentTotalByProject.

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetEmojisOfComment"/>
    /// </summary>    
    public IEnumerable<CommentEmojiTotals> GetEmojisOfComment(int id, Domain.User.User currentLoggedInUser)
    {
        return _repository.ReadEmojisOfComment(id, currentLoggedInUser);
    }

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentManager.GetAllEmojisOfComment"/>
    /// </summary>    
    public EmojiReaction GetEmojiReactionByCommentAndEmoji(int id, int emojiId, string userId)
    {
        return _repository.ReadEmojiReactionByCommentAndEmoji(id, emojiId, userId);
    }
    // GetEmojisOfProject

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentManager.GetEmojiReactionsByComment"/>
    /// </summary>
    public IEnumerable<EmojiReaction> GetEmojiReactionsByComment(int id)
    {
        return _repository.ReadEmojiReactionsByComment(id);
    }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentManager.GetEmojiReactionsByComment"/>
    /// </summary>
    public IEnumerable<ReactionGroup> GetSubCommentsByComment(int id, bool includeDocReview = false,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false)
    {
        return _repository.ReadSubCommentsByComment(id, includeDocReview, includeUser, includeTags, includeHistory,
            includeReactions);
    } // GetSubCommentsByComment.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ICommentManager.ChangeComment"/>
    /// </summary>
    public void ChangeComment(ReactionGroup reactionGroup)
    {
        Validator.ValidateObject(reactionGroup, new ValidationContext(reactionGroup), validateAllProperties: true);
        _repository.UpdateComment(reactionGroup);
    } // ChangeComment.
    
    /// <summary>
    /// <see cref="ICommentManager.GetEmojiReactionsByCommentAndUser"/>
    /// </summary>
    public EmojiReaction GetEmojiReactionsByCommentAndUser(int id, string userId)
    {
        return _repository.ReadEmojiReactionsByCommentAndUser(id, userId);
    } // GetEmojiReactionsByCommentAndUser.

    /// <summary>
    /// <see cref="ICommentManager.RemoveReaction"/>
    /// </summary>
    public void RemoveReaction(EmojiReaction reaction)
    {
        _repository.DeleteReaction(reaction);
    } // RemoveReaction.
}