using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Comment;
using Domain.Comment;

namespace BL.Comment;

/// <summary>
/// <see cref="ICommentTagManager"/>
/// </summary>  
public class CommentTagManager : ICommentTagManager
{
    //Fields.
    private ICommentTagRepository _repository;

    // Constructor.
    public CommentTagManager(ICommentTagRepository repo)
    {
        _repository = repo;
    }

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// <see cref="ICommentTagManager.AddCommentTag"/>
    /// </summary>    
    public CommentTag AddCommentTag(CommentTag commentTag)
    {
        Validator.ValidateObject(commentTag, new ValidationContext(commentTag), validateAllProperties: true);
        return _repository.CreateCommentTag(commentTag);
    } // AddCommentTag.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// <see cref="ICommentTagManager.RemoveCommentTag"/>
    /// </summary>
    public CommentTag RemoveCommentTag(CommentTag commentTag)
    {
        return _repository.DeleteCommentTag(commentTag);
    } // RemoveCommentTag.
}