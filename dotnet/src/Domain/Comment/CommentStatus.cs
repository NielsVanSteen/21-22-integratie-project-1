using Domain.User;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// Enum class with the different statuses a comment can have
/// </summary>
public enum CommentStatus : byte
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment has been created but is not yet visible to other <see cref="UserRole.RegularUser"/>
    /// </summary>
    Created = 1,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment has been published and is for everyone visible.
    /// </summary>
    Published = 2,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment has been edited by the owner or an <see cref="UserRole.Admin"/> or a <see cref="UserRole.ProjectManager"/>.
    /// </summary>
    Edited = 3,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment has been marked inappropriate by another <see cref="UserRole.RegularUser"/>.
    /// </summary>
    Marked = 4,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// an <see cref="UserRole.Admin"/> or a <see cref="UserRole.ProjectManager"/> reviewed a <see cref="Marked"/> a marked comment and has labeled the comment as actually <see cref="Inappropriate"/>.
    /// </summary>
    Inappropriate = 5,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The comment has been removed by the owner or by an <see cref="UserRole.Admin"/> or a <see cref="UserRole.ProjectManager"/>.
    /// The comment is no longer visible to other <see cref="UserRole.RegularUser"/>
    /// </summary>
    Removed = 6
}