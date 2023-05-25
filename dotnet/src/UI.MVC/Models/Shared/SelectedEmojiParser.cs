using BL.DocReview;
using Domain.DocReview;

namespace UI.MVC.Models.Shared;

/// <author> Michiel Verschueren </author>
/// <summary>
/// Class used for parsing a list of emojiId's that the user selects
/// to be used in a <see cref="DocReview"/> to HTML Codes for those emoji's
/// </summary>
public class SelectedEmojiParser
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IEmojiManager"/> used for retrieving the emoji's from the database
    /// </summary>
    private readonly IEmojiManager _emojiManager;
    
    public SelectedEmojiParser(IEmojiManager emojiManager)
    {
        _emojiManager = emojiManager;
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Parses the id's into HTML Codes
    /// </summary>
    /// <param name="selectedEmojiIds">The id's of the selected emoji's</param>
    /// <returns>List of HTML codes</returns>
    public ICollection<string> Parse(ICollection<string> selectedEmojiIds)
    {
        var codes = new List<string>();

        foreach (string selectedEmojiId in selectedEmojiIds ?? Enumerable.Empty<string>())
        {
            var id = Int32.Parse(selectedEmojiId);
            codes.Add(_emojiManager.GetEmoji(id).Code);
        }

        return codes;
    }
}