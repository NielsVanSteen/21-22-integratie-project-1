using Domain.DocReview;

namespace BL.DocReview;

public interface IEmojiManager
{
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// Returns all available <see cref="Emoji"/>.
    /// </summary>
    /// <returns>An IEnumerable of <see cref="Emoji"/> </returns>
    IEnumerable<Emoji> GetAvailableEmojis();
    
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// method to request an <see cref="Emoji"/> from the database given an id
    /// </summary>
    /// <param name="id">An emoji id</param>
    /// <returns></returns>
    Emoji GetEmoji(int id);

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Method that return all the emoji's avaiable on a docreview
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IEnumerable<Emoji> GetEmojisOfDocReview(int id);

}