using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// Class containing properties of comment
/// Class extends CommentComposite as a result of the composite pattern that is used
/// </summary>
public class ReactionGroup : Reaction
{
    // Properties

    /// <summary>
    /// Text the user commented.
    /// </summary>
    [Required]
    public string CommentText { get; set; }

    /// <summary>
    /// Beginning character of the text the user selected in the DocReview.
    /// Can be null -> SubComments are placed underneath other Comments.
    /// </summary>
    public int? BeginChar { get; set; }

    /// <summary>
    /// End character of the text the user selected in the DocReview.
    /// Can be null -> SubComments are placed underneath other Comments.
    /// </summary>
    public int? EndChar { get; set; }

    /// <summary>
    /// Tags assigned to the comment.
    /// </summary>
    public ICollection<CommentTag> CommentTags { get; set; }

    /// <summary>
    /// History of the comment overtime. <see cref="CommentHistory"/>
    /// </summary>
    public ICollection<CommentHistory> CommentHistories { get; set; }

    /// <summary>
    /// Other comments and/or emoji's on the comment.
    /// </summary>
    public ICollection<Reaction> Reactions { get; set; }

    // Constructors.
    public ReactionGroup()
    {
        Reactions = new List<Reaction>();
    }

    public ReactionGroup(string commentText) : this()
    {
        CommentText = commentText;
    }

    public ReactionGroup(User.User user, string commentText, DocReview.DocReview docReview) : this()
    {
        User = user;
        CommentText = commentText;
        DocReview = docReview;
    }

    public ReactionGroup(User.User user, string commentText, DocReview.DocReview docReview, int? beginChar, int? endChar) : this(user, commentText, docReview)
    {
        BeginChar = beginChar;
        EndChar = endChar;
    }
}