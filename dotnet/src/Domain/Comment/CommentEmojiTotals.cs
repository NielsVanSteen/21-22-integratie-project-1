namespace Domain.Comment;

/// <author> Niels Van Steen </author>
/// <summary>
/// Class containing the amount of times an emoji has been placed on a given comment. and if the current logged in user has placed that emoji on this comment.
/// </summary>
public class CommentEmojiTotals
{
    // Properties.

    /// </author> Niels Van Steen </author>
    /// <summary>
    /// The comment the emoji totals are for.
    /// </summary>
    public int CommentId { get; set; }
    
    /// <author> Niels Van Steen</author>.
    /// <summary>
    /// The emoji id.
    /// </summary>
    public int EmojiId { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The emoji code of the emoji.
    /// </summary>
    public string EmojiCode { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of times the emoji has been placed on the comment.
    /// </summary>
    public int Count { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Whether the current logged in user has placed this emoji on this comment.
    /// </summary>
    public bool HasUserLikeThatEmoji { get; set; }
    
    // Constructor.
    public CommentEmojiTotals()
    {
    }
}