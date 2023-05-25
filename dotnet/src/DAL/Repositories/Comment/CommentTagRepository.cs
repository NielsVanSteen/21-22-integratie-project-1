using Domain.Comment;

namespace DAL.Repositories.Comment;

/// <summary>
/// <see cref="ICommentTagRepository"/>
/// </summary>
public class CommentTagRepository : Repository, ICommentTagRepository
{
    // Constructor.
    public CommentTagRepository(DocReviewDbContext context) : base(context)
    {
    }


    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentTagRepository.CreateCommentTag"/>
    /// </summary>
    public CommentTag CreateCommentTag(CommentTag commentTag)
    {
        Context.CommentTags.Add(commentTag);
        Context.SaveChanges();
        return commentTag;
    } // CreateCommentTag.

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ICommentTagRepository.CreateCommentTag"/>
    /// </summary>
    public CommentTag DeleteCommentTag(CommentTag commentTag)
    {
        var toRemove = Context.CommentTags.SingleOrDefault(tag =>
            tag.ProjectTagId == commentTag.ProjectTagId && tag.ReactionGroupId == commentTag.ReactionGroupId);
        if (toRemove != null)
        {
            Context.CommentTags.Remove(toRemove);
            Context.SaveChanges();
        }

        return toRemove;
    } // DeleteCommentTag.
}