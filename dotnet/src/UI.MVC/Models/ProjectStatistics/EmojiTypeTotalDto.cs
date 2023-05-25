using Domain.ProjectStatistics;
using UI.MVC.Extensions;

namespace UI.MVC.Models.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="Domain.ProjectStatistics.EmojiTypeTotal"/>
/// </summary>
public class EmojiTypeTotalDto
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="Domain.ProjectStatistics.EmojiTypeTotal.EmojiTypeTotalId"/>
    /// </summary>
    public int EmojiTypeTotalId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.EmojiTypeTotal.Emoji"/>
    /// </summary>
    public string EmojiCode { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.EmojiTypeTotal.EmojiId"/>
    /// </summary>
    public int EmojiId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Domain.ProjectStatistics.EmojiTypeTotal.Total"/>.
    /// But in string format using <see cref="FormatExtensions"/>
    /// </summary>
    public string TotalFormatted { get; set; }
    
     /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="TotalFormatted"/>
    /// </summary>
    public int Total { get; set; }
    
    // Constructor.
    public EmojiTypeTotalDto()
    {
    }
    
    // Copy constructor.
    public EmojiTypeTotalDto(EmojiTypeTotal emojiTypeTotal)
    {
        EmojiTypeTotalId = emojiTypeTotal.EmojiTypeTotalId;
        EmojiCode = emojiTypeTotal.Emoji.Code;
        EmojiId = emojiTypeTotal.EmojiId ?? 0;
        TotalFormatted = emojiTypeTotal.Total.FormatNumber();
        Total = emojiTypeTotal.Total;
    } // EmojiTypeTotalModel.
}