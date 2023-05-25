using Domain.DocReview;

namespace DAL.Repositories.DocReview;

/// <summary>
/// <see cref="IEmojiRepository"/>
/// </summary
public class EmojiRepository : Repository, IEmojiRepository
{
    // Constructor.
    public EmojiRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.
    
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IEmojiRepository.ReadAvailableEmojis"/>
    /// </summary>
    public IEnumerable<Emoji> ReadAvailableEmojis()
    {
        return Context.Emojis.Where(e => e.DocReviewId == null).AsEnumerable();
    } // ReadAvailableEmojis.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IEmojiRepository.ReadEmoji"/>
    /// </summary>
    public Emoji ReadEmoji(int id)
    {
        return Context.Emojis.Find(id);
    } // ReadEmoji.

    /// <summary>
    /// <see cref="IEmojiRepository.ReadEmojisOfDocReview"/>
    /// </summary>
    public IEnumerable<Emoji> ReadEmojisOfDocReview(int id)
    {
        return Context.Emojis.Where(emoji => emoji.DocReviewId == id);
    } // ReadEmojisOfDocReview.
}