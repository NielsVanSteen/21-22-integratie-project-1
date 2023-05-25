using Domain.Comment;
using Domain.Project;
using Domain.Util;

namespace BL.Comment;

/// <summary>
/// The interface of the ProjectManager, used for <see cref="ReactionGroup"/>.
/// </summary>
public interface ICommentManager
{
    /// <summary>
    /// Adds a <see cref="ReactionGroup"/> to the database.
    /// </summary>
    /// <param name="reactionGroup"></param>
    public ReactionGroup AddComment(Domain.Comment.ReactionGroup reactionGroup);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds a <see cref="EmojiReaction"/> to the database.
    /// </summary>
    /// <param name="reaction"></param>
    public EmojiReaction AddReaction(EmojiReaction reaction);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns a <see cref="Reaction"/> given the Id.
    /// </summary>
    /// <param name="commentId">The Id of the comment</param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="CommentHistory"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeReactionUser">Whether or not to include the navigation property <see cref="Domain.Comment.EmojiReaction.User"/> in the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public Domain.Comment.ReactionGroup GetCommentById(int commentId, bool includeUser = false, bool includeDocReview = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false, bool includeReactionUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads an <see cref="EmojiReaction"/> given the Id.
    /// </summary>
    /// <param name="reactionId">The reaction Id.</param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="EmojiReaction.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="EmojiReaction.DocReview"/></param>
    /// <param name="includeComment">Whether or not to include the navigation property <see cref="Reaction.PlacedOnReactionGroup"/></param>
    /// <param name="includeEmoji">Whether or not to include the navigation property <see cref="EmojiReaction.Emoji"/></param>
    /// <returns><see cref="EmojiReaction"/></returns>
    public EmojiReaction GetReactionById(int reactionId, bool includeUser = false, bool includeDocReview = false, bool includeComment = false, bool includeEmoji = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="docReviewId">The Id of the <see cref="DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="CommentHistory"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeReactionUser">Whether or not to include the navigation property <see cref="Domain.Comment.EmojiReaction.User"/> in the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByDocReview(int docReviewId, bool includeUser = false, bool includeTags = false, bool includeHistory = false,bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocreview = false);
    
      /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the comments by a doc-review that include <see cref="necessaryStatuses"/> and don't include <see cref="forbiddenStatuses"/>
    /// </summary>
    /// /// <param name="filter"></param>
    /// <param name="necessaryStatuses"></param>
    /// <param name="forbiddenStatuses"></param>
    /// <param name="docReviewId">The Id of the <see cref="DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="ReactionGroup.CommentHistories"/></param>
    /// <param name="includePlacedOnComment">Whether or not to include the navigation property <see cref="Reaction.PlacedOnReactionGroup"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    /// <returns></returns>
    public IEnumerable<ReactionGroup> GetCommentsByDocReview(int docReviewId, UserCommentsFilter filter, 
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false);
     
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the sub-comments given the comment that include <see cref="necessaryStatuses"/> and don't include <see cref="forbiddenStatuses"/>
    /// </summary>
    /// <param name="necessaryStatuses"></param>
    /// <param name="forbiddenStatuses"></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="ReactionGroup.CommentHistories"/></param>
    /// <param name="includePlacedOnComment">Whether or not to include the navigation property <see cref="Reaction.PlacedOnReactionGroup"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    /// <returns></returns>
    public IEnumerable<ReactionGroup> GetSubCommentsByComment(ReactionGroup comment,
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false);
    
    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="userId">The Id of the <see cref="Domain.User.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="CommentHistory"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeReactionUser">Whether or not to include the navigation property <see cref="Domain.Comment.EmojiReaction.User"/> in the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByUser(string userId, bool includeDocReview = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false, bool includeReactionUser = false);
    
    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="userId">The Id of the <see cref="Domain.User.User"/></param>
    /// <param name="docReviewId">The Id of the <see cref="DocReview"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="CommentHistory"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeReactionUser">Whether or not to include the navigation property <see cref="Domain.Comment.EmojiReaction.User"/> in the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> GetCommentsByUserAndDocReview(string userId, int docReviewId, bool includeDocReview = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false, bool includeReactionUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all SubComments and <see cref="EmojiReaction"/> given the Comment.
    /// </summary>
    /// <param name="commentId"><see cref="Reaction.CommentId"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="EmojiReaction.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="EmojiReaction.DocReview"/></param>
    /// <returns><see cref="Reaction"/></returns>
    public IEnumerable<Reaction> GetReactionsOfCommentByComment(int commentId, bool includeUser = false, bool includeDocReview = false);
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Returns all <see cref="EmojiReaction"/> on the given Comment.
    /// </summary>
    /// <param name="id"><see cref="Reaction.CommentId"/></param>
    /// <returns><see cref="EmojiReaction"/></returns>
    public IEnumerable<EmojiReaction> GetEmojiReactionsByComment(int id);

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// Returns all the Comments of a project as a Reaction.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filterModel"><see cref="CommentsFilterModel"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.CommentHistories"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.PlacedOnReactionGroup"/></param>
    /// <param name="includeReactionUser">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.DocReview"/>></param>
    /// <returns></returns>
    public IEnumerable<ReactionGroup> GetCommentsOfProject(int id, CommentsFilterModel filterModel,
        bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false, bool includeReactionUser = false);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Get the total amount of comments for a project -> used to determine the amount of pages.
    /// </summary>
    /// <param name="project"></param>
    /// <param name="filterModel"></param>
    /// <returns></returns>
    public int GetCommentTotalByProject(Domain.Project.Project project, CommentsFilterModel filterModel = null);

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Returns all the Emoji's of a comment as a EmojiTotal
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentLoggedInUser">The current logged in user</param>
    /// <returns></returns>
    public IEnumerable<CommentEmojiTotals> GetEmojisOfComment(int id, Domain.User.User currentLoggedInUser);

   /// <summary>
   /// Returns the emoji reaction by comment Id and emoji Id
   /// </summary>
   /// <param name="id"></param>
   /// <param name="emojiId"></param>
   /// <returns></returns>
    public EmojiReaction GetEmojiReactionByCommentAndEmoji(int id, int emojiId, string userId);
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Returns all <see cref="ReactionGroup"/> on the given Comment.
    /// </summary>
    /// <param name="id"><see cref="Reaction.CommentId"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.CommentHistories"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property for <see cref="Domain.Comment.ReactionGroup.PlacedOnReactionGroup"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<ReactionGroup> GetSubCommentsByComment(int id, bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false);

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Change a <see cref="ReactionGroup"/>.
    /// </summary>
    /// <param name="reactionGroup"> The <see cref="ReactionGroup"/> you want to edit </param>
    public void ChangeComment(ReactionGroup reactionGroup);

    /// <summary>
    /// Gets an emojiReaction by comment id and user id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public EmojiReaction GetEmojiReactionsByCommentAndUser(int id, string userId);

    /// <summary>
    /// Delete a reaction.
    /// </summary>
    /// <param name="reaction"></param>
    public void RemoveReaction(EmojiReaction reaction);
}