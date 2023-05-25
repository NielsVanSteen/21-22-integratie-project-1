using Domain.Project;

namespace BL.Project;

/// <summary>
/// The interface of the ProjectManager, used for all the classes concerning a <see cref="Domain.Project.ProjectHistory"/>
/// </summary>
public interface IProjectHistoryManager
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads a single <see cref="ProjectHistory"/>
    /// </summary>
    /// <param name="id">The id of the object.</param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="ProjectHistory.Project"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="ProjectHistory.EditedBy"/></param>
    /// <returns></returns>
    public ProjectHistory GetProjectHistory(int id, bool includeProject = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the project histories for a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="ProjectHistory.Project"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="ProjectHistory.EditedBy"/></param>
    /// <returns></returns>
    public IEnumerable<ProjectHistory> GetProjectHistoriesBydProject(Domain.Project.Project project, bool includeProject = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads all the project histories given a <see cref="Domain.User.User"/> and a <see cref="Domain.Project.Project"/>.
    /// </summary>
    /// <param name="user">The project histories of all this user will be returned.</param>
    /// <param name="project">Only the histories for a specific project.</param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="ProjectHistory.Project"/></param>
    /// <param name="includeUser">Whether or not to include the navigation property for <see cref="ProjectHistory.EditedBy"/></param>
    /// <returns></returns>
    public IEnumerable<ProjectHistory> GetProjectHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeProject = false, bool includeUser = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Create a new project history.
    /// </summary>
    /// <param name="projectHistory"></param>
    /// <returns></returns>
    public ProjectHistory AddProjectHistory(ProjectHistory projectHistory);
}