using DAL.Repositories.Comment;
using Domain.Comment;

namespace BL.Comment;

/// <summary>
/// Interface for the comment tags.
/// </summary>
public interface ICommentTagManager
{
    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Adds a new CommentTag
    /// </summary>
    /// <param name="commentTag"></param>
    /// <returns></returns>
    public CommentTag AddCommentTag(CommentTag commentTag);

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Remove an existing CommentTag
    /// </summary>
    /// <param name="commentTag"></param>
    /// <returns></returns>
    public CommentTag RemoveCommentTag(CommentTag commentTag);
}