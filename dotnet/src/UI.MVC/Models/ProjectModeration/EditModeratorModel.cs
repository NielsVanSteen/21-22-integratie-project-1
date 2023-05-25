using BL.Project;
using Domain.Project;
using Domain.User;
using UI.MVC.Models.Dto;

namespace UI.MVC.Models.ProjectModeration;

/// <author>Niels Van Steen</author>
/// <summary>
/// The mode to change a <see cref="UserRole.ProjectManager"/> or <see cref="MarkedEmail"/>.
/// </summary>
public class EditModeratorModel
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// List containing the added projects.
    /// </summary>
    public ICollection<int> AssignedProjectIds { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// List containing the projects the manager currently belongs to.
    /// </summary>
    public IEnumerable<Domain.Project.Project> Projects { get; set; }

    // Constructor.
}