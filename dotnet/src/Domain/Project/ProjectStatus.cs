using Domain.User;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// Enum containing all the possible <see cref="Project"/> states.
/// </summary>
public enum ProjectStatus
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Project was created but is not yet visible to normal <see cref="UserRole.RegularUser"/>.
    /// </summary>
    Created = 0,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project is visible for everyone.
    /// </summary>
    Published,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project is archived and is no longer visible to <see cref="UserRole.RegularUser"/>.
    /// </summary>
    Archived
    
    // The state can't be deleted -> since all the data will be deleted from the database.
}