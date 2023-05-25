using System.ComponentModel.DataAnnotations;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class contains the project styling for the current project.
/// </summary>
public class ProjectStyling
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStyling"/>
    /// </summary>
    public int ProjectStylingId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStyling"/>
    /// </summary>
    public ThemeStyles ThemeStyle { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The primary key.
    /// </summary>
    public int ThemeStylesId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The default serif font.
    /// </summary>
    public string FontSerif { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The default sans serif font.
    /// </summary>
    public string FontSansSerif { get; set; }

    // Constructor.
    public ProjectStyling()
    {
    }

    // Methods
}