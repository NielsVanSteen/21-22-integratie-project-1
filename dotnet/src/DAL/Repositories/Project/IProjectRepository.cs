using Domain.Project;
using Domain.Util;

namespace DAL.Repositories.Project;

/// <summary>
/// The repository interface for all the classes concerning a Project.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.Project.Project"/>
/// <see cref="Domain.Project.ProjectStyling"/>
/// </example>
public interface IProjectRepository
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read a project by the internal (unique) project name.
    /// The internal project name makes up the URL-prefix. This is how the application knows which project to display.
    /// </summary>
    /// <param name="externalProjectName">The unique internal project name.</param>
    /// <param name="includeProjectHistory">Whether or not to include the navigation property <see cref="Domain.Project.Project.ProjectHistories"/></param>
    /// <param name="includeFooterLogos">Whether or not to include the navigation property <see cref="Domain.Project.Project.FooterLogos"/></param>
    /// <param name="includeStyling">Whether or not to include the navigation property for <see cref="Domain.Project.Project.ProjectStyling"/> NOTE: this also includes <see cref="ProjectStyling.ThemeStyle"/></param>
    /// <returns>The project information.</returns>
    public Domain.Project.Project ReadProjectByExternalName(string externalProjectName, bool includeProjectHistory = false, bool includeFooterLogos = false, bool includeStyling = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all the projects.
    /// </summary>
    /// <param name="includeLogos">Whether or not to include the navigation property <see cref="Domain.Project.Project.FooterLogos"/>.</param>
    /// <param name="includeStyling">Whether or not to include the navigation property <see cref="Domain.Project.Project.ProjectStyling"/>.</param>
    /// <param name="includeTags">Whether or not to include the navigation property <see cref="Domain.Project.Project.ProjectTags"/>.</param>
    /// <param name="includeUsers">Whether or not to include the navigation property <see cref="Domain.Project.Project.Users"/>.</param>
    /// <param name="includeTimeLine">Whether or not to include the navigation property <see cref="Domain.Project.Project.TimeLine"/>.</param>
    /// <returns></returns>
    public IEnumerable<Domain.Project.Project> ReadProjects(bool includeLogos = false, bool includeStyling = false,
        bool includeTags = false, bool includeUsers = false, bool includeTimeLine = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all projects given filter criteria, and a sort order.
    /// </summary>
    /// <param name="name">The name to filter on. This is both the <see cref="Domain.Project.Project.InternalName"/> as well as the <see cref="Domain.Project.Project.ExternalName"/></param>
    /// <param name="sortOrder">The order to sort on. See <see cref="SortOrder"/> for more detail.</param>
    /// <param name="user">When not null only the projects the user is assigned to will be filtered.</param>
    /// <returns></returns>
    public IEnumerable<Domain.Project.Project> ReadProjectByFilters(string name, SortOrder sortOrder, Domain.User.User user = null, bool includeHistory = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="project">The project to create.</param>
    /// <returns></returns>
    public Domain.Project.Project CreateProject(Domain.Project.Project project);

    /// <summary>
    /// Get all projects of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public IEnumerable<Domain.Project.Project> ReadProjectsByUser(Domain.User.User user);

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Returns a project given an id
    /// </summary>
    /// <param name="id">the id of the requested project</param>
    /// <returns>The found project or null</returns>
    public Domain.Project.Project ReadProjectById(int id);
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Deletes a project from the database
    /// </summary>
    /// <param name="project">the project to be deleted</param>
    public void DeleteProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates a project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public Domain.Project.Project UpdateProject(Domain.Project.Project project);
}