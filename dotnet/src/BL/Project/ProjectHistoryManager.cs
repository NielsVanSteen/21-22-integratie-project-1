using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Project;

namespace BL.Project;

public class ProjectHistoryManager : IProjectHistoryManager
{
    // Fields.
    private IProjectHistoryRepository _repository;

    // Constructor.
    public ProjectHistoryManager(IProjectHistoryRepository repo)
    {
        _repository = repo;
    } // ProjectHistoryManager.

    // Methods
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryManager.GetProjectHistory"/>
    /// </summary>
    public ProjectHistory GetProjectHistory(int id, bool includeProject = false, bool includeUser = false)
    {
        return _repository.ReadProjectHistory(id, includeProject, includeUser);
    } // GetProjectHistory.
    
     /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryManager.GetProjectHistoriesBydProject"/>
    /// </summary>
    public IEnumerable<ProjectHistory> GetProjectHistoriesBydProject(Domain.Project.Project project, bool includeProject = false, bool includeUser = false)
    {
        return _repository.ReadProjectHistoriesBydProject(project, includeProject, includeUser);
    } // GetProjectHistoriesBydProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryManager.GetProjectHistoriesByUserAndProject"/>
    /// </summary>
    public IEnumerable<ProjectHistory> GetProjectHistoriesByUserAndProject(Domain.User.User user, Domain.Project.Project project, bool includeReactionGroup = false, bool includeUser = false)
    {
        return _repository.ReadProjectHistoriesByUserAndProject(user, project, includeReactionGroup, includeUser);
    } // GetCommentHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryManager.AddProjectHistory"/>
    /// </summary>
    public ProjectHistory AddProjectHistory(ProjectHistory projectHistory)
    {
        Validator.ValidateObject(projectHistory, new ValidationContext(projectHistory), validateAllProperties: true);
        return _repository.CreateProjectHistory(projectHistory);
    } // AddProjectHistory.
}