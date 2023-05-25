using System.ComponentModel.DataAnnotations;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// A Project can have many footer logo's. This class holds one.
/// </summary>
public class FooterLogo
{
    // Properties.
    
    /// <author>Brian Nys</author>
    /// <summary>
    /// Primary key for seedings.
    /// </summary>
    [Key]
    public int FooterLogoId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project the footer logo belongs to.
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// <see cref="ProjectId"/>
    /// </summary>
    public int ProjectId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Path to a footer logo.
    /// </summary>
    [Required]
    public string ImageName { get; set; }

    // Constructors.
    public FooterLogo() { }

    public FooterLogo(Project project, string imageName)
    {
        Project = project;
        ImageName = imageName;
    } // ProjectFooterLogo.
}