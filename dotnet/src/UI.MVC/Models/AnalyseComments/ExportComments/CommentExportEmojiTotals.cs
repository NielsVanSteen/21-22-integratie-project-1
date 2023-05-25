namespace UI.MVC.Models.AnalyseComments.ExportComments;

public class CommentExportEmojiTotals
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The emoji code.
    /// </summary>
    public string EmojiCode { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The emoji icon.
    /// </summary>
    public string Emoji { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of times the emoji was used on a comment.
    /// </summary>
    public int Amount { get; set; }
    
    // Constructors.
    public CommentExportEmojiTotals()
    {
    }
    
    public CommentExportEmojiTotals(string emojiCode, string emoji, int amount)
    {
        EmojiCode = emojiCode;
        Emoji = emoji;
        Amount = amount;
    }
    
}