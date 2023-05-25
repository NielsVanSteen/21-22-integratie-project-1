using Domain.Comment;
using Domain.Project;
using Domain.Util;
using Emoji = Domain.DocReview.Emoji;


namespace DAL.Repositories.Comment;

/// <summary>
/// The repository interface for all the comments. <see cref="Reaction"/>.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Reaction"/>
/// <see cref="ReactionGroup"/>
/// <see cref="Domain.Comment.EmojiReaction"/>
/// <see cref="Domain.Comment.CommentTag"/>
/// <see cref="Domain.Comment.CommentHistory"/>
/// </example>
public interface ICommentRepository
{
    /// <summary>
    /// Adds a <see cref="ReactionGroup"/> to the database.
    /// </summary>
    /// <param name="reactionGroup"></param>
    public ReactionGroup CreateComment(Domain.Comment.ReactionGroup reactionGroup);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Create a <see cref="ReactionGroup"/>
    /// </summary>
    /// <param name="reaction"></param>
    public EmojiReaction CreateReaction(EmojiReaction reaction);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns a <see cref="Reaction"/> given the Id.
    /// </summary>
    /// <param name="commentId">The Id of the comment</param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="CommentHistory"/></param>
    /// <param name="placedOnComment">Whether or not to include the navigation property <sReaction.PlacedOnReactionGroupOnComment"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public Domain.Comment.ReactionGroup ReadCommentById(int commentId, bool includeUser = false,
        bool includeDocReview = false, bool includeTags = false, bool includeHistory = false,
        bool placedOnComment = false);

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
    public EmojiReaction ReadReactionById(int reactionId, bool includeUser = false, bool includeDocReview = false,
        bool includeComment = false, bool includeEmoji = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="docReviewId">The Id of the <see cref="DocReview"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="ReactionGroup.User"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="ReactionGroup.CommentHistories"/></param>
    /// <param name="includePlacedOnComment">Whether or not to include the navigation property <see cref="Reaction.PlacedOnReactionGroup"/></param>
    /// <param name="includeReactions">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> ReadCommentsByDocReview(int docReviewId, bool includeUser = false,
        bool includeTags = false,
        bool includeHistory = false, bool includePlacedOnComment = false, bool includeReactions = false,
        bool includeDocReview = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the comments by a doc-review that include <see cref="necessaryStatuses"/> and don't include <see cref="forbiddenStatuses"/>
    /// </summary>
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
    public IEnumerable<ReactionGroup> ReadCommentsByDocReview(int docReviewId, UserCommentsFilter filter,
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
    public IEnumerable<ReactionGroup> ReadSubCommentsByComment(ReactionGroup comment,
        IEnumerable<CommentStatus> necessaryStatuses, IEnumerable<CommentStatus> forbiddenStatuses,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includePlacedOnComment = false, bool includeReactions = false, bool includeDocReview = false);

    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="userId">The Id of the <see cref="User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="ReactionGroup.CommentHistories"/></param>
    /// <param name="placedOnComment">Whether or not to include the navigation property <see cref="Reaction.PlacedOnReactionGroup"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> ReadCommentsByUser(string userId, bool includeDocReview = false,
        bool includeTags = false, bool includeHistory = false, bool placedOnComment = false);

    /// <summary>
    /// Returns all <see cref="Reaction"/> given the DocReview.
    /// This are only the direct comments. not reactions/subComments. (they can be included, but the IEnumerable is of the type <see cref="ReactionGroup"/> and thus only included comments).
    /// </summary>
    /// <param name="userId">The Id of the <see cref="User"/></param>
    /// <param name="docReviewId">The Id of the <see cref="DocReview"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="ReactionGroup.DocReview"/></param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="ReactionGroup.CommentTags"/></param>
    /// <param name="includeHistory">Whether or not to include the navigation property <see cref="ReactionGroup.CommentHistories"/></param>
    /// <param name="PlacedOnComment">Whether or not to include the navigation property <see cref="ReactionGroup.Reactions"/></param>
    /// <returns><see cref="ReactionGroup"/></returns>
    public IEnumerable<Domain.Comment.ReactionGroup> ReadCommentsByUserAndDocReview(string userId, int docReviewId,
        bool includeDocReview = false, bool includeTags = false, bool includeHistory = false,
        bool PlacedOnComment = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all SubComments given the Comment.
    /// </summary>
    /// <param name="commentId"><see cref="Reaction.CommentId"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property <see cref="EmojiReaction.User"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property <see cref="EmojiReaction.DocReview"/></param>
    /// <returns><see cref="Reaction"/></returns>
    public IEnumerable<Reaction> ReadReactionsOfCommentByComment(int commentId, bool includeUser = false,
        bool includeDocReview = false);

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Returns all <see cref="EmojiReaction"/> on the given Comment.
    /// </summary>
    /// <param name="id"><see cref="Reaction.CommentId"/></param>
    /// <returns><see cref="EmojiReaction"/></returns>
    public IEnumerable<EmojiReaction> ReadEmojiReactionsByComment(int id);

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
    public IEnumerable<ReactionGroup> ReadSubCommentsByComment(int id, bool includeDocReview = false,
        bool includeUser = false, bool includeTags = false, bool includeHistory = false, bool includeReactions = false);

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Change a <see cref="ReactionGroup"/>.
    /// </summary>
    /// <param name="reactionGroup"> The <see cref="ReactionGroup"/> you want to edit </param>
    public void UpdateComment(ReactionGroup reactionGroup);

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Returns all the Comments of a project as a ReactionGroup.
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
    public IEnumerable<ReactionGroup> ReadCommentsOfProject(int id, CommentsFilterModel filterModel = null,
        bool includeDocReview = false, bool includeUser = false, bool includeTags = false, bool includeHistory = false,
        bool includeReactions = false, bool includeReactionUser = false);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Get the total amount of comments for a project -> used to determine the amount of pages.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public int ReadCommentTotalByProject(Domain.Project.Project project, CommentsFilterModel filterModel = null);

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// Returns the Emoji id + code and the amount of times that emoji has been posted on a specific comment (or sub comment). <see cref="id"/>.
    /// </summary>
    /// <param name="id">The id of the comment</param>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns></returns>
    public IEnumerable<CommentEmojiTotals> ReadEmojisOfComment(int id, Domain.User.User currentLoggedInUser);

    ///<author>Sander Verheyen</author>
    /// <summary>
    /// Returns the Emoji id + code and the amount of times that emoji has been posted on a specific comment (or sub comment). <see cref="id"/>.
    /// </summary>
    /// <param name="id">The id of the comment</param>
    /// <returns></returns>
    public EmojiReaction ReadEmojiReactionByCommentAndEmoji(int id, int emojiId, string userId);

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Returns the emoji reaction placed by user on a comment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public EmojiReaction ReadEmojiReactionsByCommentAndUser(int id, string userId);

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Deletes an emoji reaction
    /// </summary>
    /// <param name="reaction"></param>
    public void DeleteReaction(EmojiReaction reaction);
}