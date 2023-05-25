using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Domain.DocReview;
using Domain.User;

namespace Domain.Project;

///<author>Niels Van Steen</author>
/// <summary>
/// 
/// </summary>
public class Project
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id of the project -> name is not pk => project-timeline is a one-to-one relation and PK's must be of a compatible type for fluent API.
    /// </summary>
    [Key]
    public int ProjectId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This is the name only visible for project-managers, and not for normal users.
    /// It's a unique name each project gets inside a company.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string InternalName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This is the unique name for a project gets, which all users will be able to see.
    /// This name will define the URL-prefix.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ExternalName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The title for a project, This can be non-unique. And gives more information about the project.
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string ProjectTitle { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Unlike the main-logo a project can also have many footer logo's.
    /// </summary>
    public ICollection<FooterLogo> FooterLogos { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A quick introduction giving more information about the project.
    /// </summary>
    public string Introduction { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Styling elements used to edit the website's theme.
    /// </summary>
    public ProjectStyling ProjectStyling { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStyling"/>
    /// </summary>
    public int? ProjectStylingId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Tags for the project.
    /// </summary>
    public ICollection<ProjectTag> ProjectTags { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Timeline belonging to the project.
    /// </summary>
    public TimeLine TimeLine { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// All the users registered for the project. -> is only necessary for Fluent API.
    /// </summary>
    [Required]
    public ICollection<User.User> Users { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Holds no other values than to define the Fluent API.
    /// </summary>
    public ICollection<MarkedEmail> MarkedEmails { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The status of the project <see cref="ProjectStatus"/>.
    /// </summary>
    public ICollection<ProjectHistory> ProjectHistories { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Privacy statement for the current project.
    /// </summary>
    public string PrivacyStatement { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Accessibility statement for the project.
    /// </summary>
    public string AccessibilityStatement { get; set; }

    // Constructor.
    public Project()
    {
        ProjectHistories = new List<ProjectHistory>();
        MarkedEmails = new List<MarkedEmail>();
        ProjectTags = new List<ProjectTag>();
        Users = new List<User.User>();
        FooterLogos = new List<FooterLogo>();
    } // Project.

    // Methods.
    
}