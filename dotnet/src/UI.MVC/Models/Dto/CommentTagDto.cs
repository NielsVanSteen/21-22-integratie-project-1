using Domain.User;

namespace UI.MVC.Models.Dto;

public class CommentTagDto
{
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ProjectTag.ProjectTagId"/>.
    /// </summary>
    public int ProjectTagId { get; set; }
    
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// <see cref="ReactionGroup.CommentId"/>.
    /// </summary>
    public int ReactionGroupId { get; set; }

}