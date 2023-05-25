using System.ComponentModel.DataAnnotations;
using Domain.Project;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// 'Intermediate' class between <see cref="ReactionGroup"/> and <see cref="ProjectTag"/>
/// Contains the data of a <see cref="ProjectTag"/> that has been placed on a <see cref="ReactionGroup"/>
/// </summary>
public class CommentTag
{
    // Properties.

    /// <summary>
    /// The id of the comment tag.
    /// </summary>
    [Key]
    public int CommentTagId { get; set; }

    /// <summary>
    /// The <see cref="ProjectTag"/> assigned to the <see cref="ReactionGroup"/>
    /// </summary>
    public ProjectTag ProjectTag { get; set; }

    /// <summary>
    /// <see cref="ProjectTag"/>
    /// </summary>
    public int ProjectTagId { get; set; }

    /// <summary>
    /// On which <see cref="ReactionGroup"/> the <see cref="ProjectTag"/> has been placed.
    /// </summary>
    public ReactionGroup ReactionGroup { get; set; }

    /// <summary>
    /// <see cref="ReactionGroupId"/>
    /// </summary>
    public int ReactionGroupId { get; set; }

    /// <summary>
    /// The <see cref="User"/> who placed the <see cref="ProjectTag"/>.
    /// </summary>
    public User.User PlacedByUser { get; set; }
    
    /// <summary>
    /// <see cref="PlacedByUserId"/>
    /// </summary>
    public string PlacedByUserId { get; set; }

    //Constructors
    public CommentTag() { }
}