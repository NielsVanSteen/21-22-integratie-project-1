using Domain.DocReview;

namespace UI.MVC.Extensions;

/// <author> Michiel Verschueren </author>
/// <summary>
/// Class that holds extension methods for type <see cref="Emoji"/>
/// </summary>
public static class EmojiExtensions
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Prefix that should come before a code to make it a valid HTML code
    /// </summary>
    public const string CodePrefix = "&#";
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Suffix that should come after a code to make it a valid HTML code
    /// </summary>
    public const string CodeSuffix = ";";
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Get the full HTML code from a <see cref="Emoji"/> instance
    /// </summary>
    /// <param name="emoji">the emoji for wich the code is to be generated</param>
    /// <returns>A valid HTML code that represents the emoji</returns>
    public static string GetCode(this Emoji emoji)
    {
        return CodePrefix + emoji.Code + CodeSuffix;
    }
}