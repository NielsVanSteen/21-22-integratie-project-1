using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Util;

namespace BL.Project;

/// <summary>
/// <see cref="IProjectManager"/>
/// </summary>
public class ProjectManager : IProjectManager
{
    // Fields.
    private IProjectRepository _repository;

    // Constructor.
    public ProjectManager(IProjectRepository repository)
    {
        _repository = repository;
    } // UserService.

    ///<author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectManager.GetProjectByExternalName"/>
    /// </summary>
    public Domain.Project.Project GetProjectByExternalName(string externalProjectName,
        bool includeProjectHistory = false, bool includeFooterLogos = false, bool includeStyling = false)
    {
        return _repository.ReadProjectByExternalName(externalProjectName, includeProjectHistory, includeFooterLogos, includeStyling);
    } // GetProjectByNameCaseInsensitive.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectManager.GetProjects"/>
    /// </summary>
    public IEnumerable<Domain.Project.Project> GetProjects(bool includeLogos = false, bool includeStyling = false,
        bool includeTags = false, bool includeUsers = false, bool includeTimeLine = false)
    {
        return _repository.ReadProjects(includeLogos, includeStyling, includeTags, includeUsers, includeTimeLine);
    } // GetProjects.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectManager.GetProjectByFilters"/>
    /// </summary>
    public IEnumerable<Domain.Project.Project> GetProjectByFilters(string name, SortOrder sortOrder,
        Domain.User.User user = null, bool includeHistory = false)
    {
        return _repository.ReadProjectByFilters(name, sortOrder, user, includeHistory);
    } // GetProjectByFilters.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectManager.AddProject"/>
    /// </summary>
    public Domain.Project.Project AddProject(Domain.Project.Project project)
    {
        Validator.ValidateObject(project, new ValidationContext(project), validateAllProperties: true);
        return _repository.CreateProject(project);
    } // AddProject.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IProjectManager.GetProjectById"/>
    /// </summary>
    public Domain.Project.Project GetProjectById(int id)
    {
        return _repository.ReadProjectById(id);
    } // GetProjectById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectManager.ChangeProject"/>
    /// </summary>
    public Domain.Project.Project ChangeProject(Domain.Project.Project project)
    {
        Validator.ValidateObject(project, new ValidationContext(project), validateAllProperties: true);
        return _repository.UpdateProject(project);
    } // ChangeProject.


    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IProjectManager.RemoveProject"/>
    /// </summary>
    /// <param name="project">The project to be deleted</param>
    public void RemoveProject(Domain.Project.Project project)
    {
        _repository.DeleteProject(project);
    } // RemoveProject.
}