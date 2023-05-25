using System.ComponentModel.DataAnnotations;
using Domain.User;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// 
/// </summary>
public class ProjectHistory
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id of the project History.
    /// </summary>
    [Key]
    public int ProjectHistoryId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/> who edited the <see cref="Project"/> status.
    /// </summary>
    public User.User EditedBy { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="EditedBy"/>
    /// </summary>
    public string EditedById { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project that was changed.
    /// </summary>
    public Project Project { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project"/>
    /// </summary>
    public int ProjectId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The date and time the status was updated.
    /// </summary>
    public DateTime EditedOn { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStatus"/>
    /// </summary>
    public ProjectStatus ProjectStatus { get; set; }
    
    // Constructor.
}