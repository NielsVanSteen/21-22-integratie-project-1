using DAL.Repositories.DocReview;
using Domain.DocReview;

namespace BL.DocReview;

public class EmojiManager : IEmojiManager
{
    private IEmojiRepository _repository;

    public EmojiManager(IEmojiRepository repository)
    {
        _repository = repository;
    }

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IEmojiManager.GetAvailableEmojis"/>
    /// </summary>
    public IEnumerable<Emoji> GetAvailableEmojis()
    {
        return _repository.ReadAvailableEmojis();
    } // GetAvailableEmojis.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IEmojiManager.GetEmoji"/>
    /// </summary>
    public Emoji GetEmoji(int id)
    {
        return _repository.ReadEmoji(id);
    } // GetEmoji.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IEmojiManager.GetEmojisOfDocReview"/>
    /// </summary>
    public IEnumerable<Emoji> GetEmojisOfDocReview(int id)
    {
        return _repository.ReadEmojisOfDocReview(id);
    } // GetEmojisOfDocReview.
}