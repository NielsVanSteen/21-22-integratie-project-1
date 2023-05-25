namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// Enum containing all the roles available in the application.
/// </summary>
public enum UserRole : byte
{
    /// <summary>
    /// Any normal-user, is always a normal-user project.
    /// </summary>
    RegularUser = 0,
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// Project-manager, is always a manager per project. Never for multiple projects. -> would need to register again for another project and be granted the project-manager role.
    /// </summary>
    ProjectManager = 1,
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// Admin is a user that is not tied to a single project, but has visibility on all projects.
    /// </summary>
    Admin = 2
}