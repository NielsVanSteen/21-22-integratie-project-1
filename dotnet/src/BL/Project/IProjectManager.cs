using Domain.Util;

namespace BL.Project;

/// <summary>
/// The interface of the ProjectManager, used for all the classes concerning a <see cref="Domain.Project.Project"/>
/// </summary>
public interface IProjectManager
{
    /// <author>Niels Van Steen</author>
    ///  <summary>
    ///  Search for a project given the internal name.
    ///  </summary>
    /// <param name="externalProjectName">The unique internal project name (which is the URL-prefix).</param>
    /// <param name="includeProjectHistory">Whether or not to include the navigation property <see cref="Domain.Project.Project.ProjectHistories"/>.</param>
    /// <param name="includeFooterLogos">Whether or not to include the navigation property <see cref="Domain.Project.Project.FooterLogos"/></param>
    /// <param name="includeStyling">Whether or not to include the navigation property for <see cref="Domain.Project.Project.ProjectStyling"/> NOTE: this also includes <see cref="ProjectStyling.ThemeStyle"/></param>
    /// <returns>A Project object.</returns>
    public Domain.Project.Project GetProjectByExternalName(string externalProjectName,
        bool includeProjectHistory = false, bool includeFooterLogos = false, bool includeStyling = false);

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
    public IEnumerable<Domain.Project.Project> GetProjects(bool includeLogos = false, bool includeStyling = false,
        bool includeTags = false, bool includeUsers = false, bool includeTimeLine = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all projects given filter criteria, and a sort order.
    /// </summary>
    /// <param name="name">The name to filter on. This is both the <see cref="Domain.Project.Project.InternalName"/> as well as the <see cref="Domain.Project.Project.ExternalName"/></param>
    /// <param name="sortOrder">The order to sort on. See <see cref="SortOrder"/> for more detail.</param>
    /// <param name="user">When not null only the projects the user is assigned to will be filtered.</param>
    /// <param name="includeHistory">To include the project history</param>
    /// <returns></returns>
    public IEnumerable<Domain.Project.Project> GetProjectByFilters(string name, SortOrder sortOrder,
        Domain.User.User user = null, bool includeHistory = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="project">The project to create.</param>
    /// <returns></returns>
    public Domain.Project.Project AddProject(Domain.Project.Project project);


    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// Returns a project given an id
    /// </summary>
    /// <param name="id">The id of the requested project</param>
    /// <returns>the found project or null</returns>
    public Domain.Project.Project GetProjectById(int id);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates a project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public Domain.Project.Project ChangeProject(Domain.Project.Project project);

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Removes a <see cref="Domain.Project.Project"/> from the database
    /// </summary>
    /// <param name="project">The project to be deleted</param>
    public void RemoveProject(Domain.Project.Project project);
}