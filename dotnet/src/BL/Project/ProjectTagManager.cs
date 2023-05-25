using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Project;

namespace BL.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IProjectTagManager"/>
/// </summary>
public class ProjectTagManager : IProjectTagManager
{
    // Fields.
    private IProjectTagRepository _repository;

    // Constructor.
    public ProjectTagManager(IProjectTagRepository repository)
    {
        _repository = repository;
    } // UserService.

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.AddProjectTag"/>
    /// </summary>
    public void AddProjectTag(ProjectTag projectTag)
    {
        Validator.ValidateObject(projectTag, new ValidationContext(projectTag), validateAllProperties: true);
        _repository.CreateProjectTag(projectTag);
    } // AddProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.GetProjectTagsByProject"/>
    /// </summary>
    public IEnumerable<ProjectTag> GetProjectTagsByProject(Domain.Project.Project project, bool includeProject = false)
    {
        return _repository.ReadProjectTagsByProject(project, includeProject);
    } // GetProjectTagsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.GetProjectTag"/>
    /// </summary>
    public ProjectTag GetProjectTag(int id, bool includeProject = false)
    {
        return _repository.ReadProjectTag(id, includeProject);
    } // GetProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.ChangeProjectTag"/>
    /// </summary>
    public void ChangeProjectTag(ProjectTag projectTag)
    {
         Validator.ValidateObject(projectTag, new ValidationContext(projectTag), validateAllProperties: true);
        _repository.UpdateProjectTag(projectTag);
    } // ChangeProjectTag

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.RemoveProjectTag"/>
    /// </summary>
    public bool RemoveProjectTag(int id)
    {
        return _repository.DeleteProjectTag(id);
    } // RemoveProjectTag

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagManager.RemoveProjectTag"/>
    /// </summary>
    public ProjectTag GetProjectTagByProjectAndName(Domain.Project.Project project, string name)
    {
        return _repository.ReadProjectTagByProjectAndName(project, name);
    } // GetProjectTagByProjectAndName.
}