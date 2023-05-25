using System.ComponentModel.DataAnnotations;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// Root class for the composite pattern with Comment and EmojiReaction
/// </summary>
public abstract class Reaction
{
    // Properties.
    /// <summary>
    /// Comment Id
    /// </summary>
    [Key]
    public int CommentId { get; set; }

    /// <summary>
    /// The comment the reaction belongs to.
    /// </summary>
    public ReactionGroup PlacedOnReactionGroup { get; set; }

    /// <summary>
    /// <see cref="PlacedOnReactionGroup"/>
    /// </summary>
    public int? PlacedOnReactionGroupId { get; set; }

    /// <summary>
    /// The <see cref="User"/> whom wrote the <see cref="ReactionGroup"/> / placed the <see cref="EmojiReaction"/>.
    /// </summary>
    public User.User User { get; set; }
    
    /// <summary>
    /// <see cref="UserId"/>
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The <see cref="DocReview"/> the <see cref="Reaction"/> was placed on.
    /// </summary>
    public DocReview.DocReview DocReview { get; set; }

    /// <summary>
    /// <see cref="DocReviewId"/>
    /// </summary>
    public int DocReviewId { get; set; }

    // Constructor.
    protected Reaction() { }
}