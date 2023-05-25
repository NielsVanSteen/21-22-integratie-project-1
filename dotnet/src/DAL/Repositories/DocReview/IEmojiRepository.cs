using Domain.Comment;
using Domain.DocReview;

namespace DAL.Repositories.DocReview;

/// <summary>
/// Interface repository for the emoji's.
/// </summary>
public interface IEmojiRepository
{
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// Returns all available <see cref="Emoji"/>.
    /// </summary>
    /// <returns>An IEnumerable of <see cref="Emoji"/> </returns>
    public IEnumerable<Emoji> ReadAvailableEmojis();

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// method to request an <see cref="Emoji"/> from the database given an id
    /// </summary>
    /// <param name="id">An emoji id</param>
    /// <returns></returns>
    Emoji ReadEmoji(int id);

    /// <summary>
    /// Read all the emoji's given a doc-review
    /// </summary>
    /// <param name="id">The id of the doc-review</param>
    /// <returns></returns>
    IEnumerable<Emoji> ReadEmojisOfDocReview(int id);
}