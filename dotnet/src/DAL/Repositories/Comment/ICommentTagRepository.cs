using Domain.Comment;

namespace DAL.Repositories.Comment;

/// <summary>
/// Repository interface for the comment tag's.
/// </summary>
public interface ICommentTagRepository
{
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Creates a new CommentTag
    /// </summary>
    /// <param name="commentTag"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public CommentTag CreateCommentTag(CommentTag commentTag);

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Deletes an existing CommentTag
    /// </summary>
    /// <param name="commentTag"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public CommentTag DeleteCommentTag(CommentTag commentTag);
}