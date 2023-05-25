using System.ComponentModel.DataAnnotations;
using Domain.DocReview;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// Class containing properties of EmojiReaction
/// Class extends CommentCompiste as a result of the composite pattern that is used
/// </summary>
public class EmojiReaction : Reaction
{
    // Properties.

    /// <summary>
    /// The emoji of the reaction.
    /// </summary>
    public Emoji Emoji { get; set; }

    /// <summary>
    /// <see cref="EmojiId"/>
    /// </summary>
    public int EmojiId { get; set; }
    
    // Constructor.
    public EmojiReaction() {}
}