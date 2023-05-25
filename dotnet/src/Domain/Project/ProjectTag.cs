using System.ComponentModel.DataAnnotations;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// Tag belong to a project. And can be assigned to comments.
/// </summary>
public class ProjectTag
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The id of the tag.
    /// </summary>
    [Key]
    public int ProjectTagId;

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Name of the tag.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project the tag belongs to.
    /// </summary>
    public Project Project { get; set; }

    /// <author>Brian Nys</author>
    /// <summary>
    /// Foreign key for project of this tag.
    /// </summary>
    public int ProjectId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A quick description describing the text.
    /// </summary>
    [Required]
    public string Color { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Whether to user white or black text. White text works well for darker backgrounds and vice versa.
    /// </summary>
    public bool IsTextWhite { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Tags can be either public or private. Normal users can only see public tag.s
    /// </summary>
    public bool IsPublic { get; set; }

    // Constructor.
    public ProjectTag() { }
}