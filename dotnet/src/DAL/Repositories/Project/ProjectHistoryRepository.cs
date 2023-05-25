using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <summary>
/// <see cref="IProjectHistoryRepository"/>
/// </summary>
public class ProjectHistoryRepository : Repository, IProjectHistoryRepository
{
    // Constructor.
    public ProjectHistoryRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryRepository.ReadProjectHistory"/>
    /// </summary>
    public ProjectHistory ReadProjectHistory(int id, bool includeProject = false, bool includeUser = false)
    {
        IQueryable<ProjectHistory> histories = Context.ProjectHistories;

        if (includeProject)
            histories = histories.Include(h => h.Project);

        if (includeUser)
            histories = histories.Include(h => h.EditedBy);

        return histories.SingleOrDefault(h => h.ProjectHistoryId == id);
    } // ReadProjectHistory.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryRepository.ReadProjectHistoriesBydProject"/>
    /// </summary>
    public IEnumerable<ProjectHistory> ReadProjectHistoriesBydProject(Domain.Project.Project project,
        bool includeProject = false, bool includeUser = false)
    {
        IQueryable<ProjectHistory> histories = Context.ProjectHistories;

        if (includeProject)
            histories = histories.Include(h => h.Project);

        if (includeUser)
            histories = histories.Include(h => h.EditedBy);

        return histories.Where(h => h.Project == project);
    } // ReadProjectHistoriesBydProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryRepository.ReadProjectHistoriesByUserAndProject"/>
    /// </summary>
    public IEnumerable<ProjectHistory> ReadProjectHistoriesByUserAndProject(Domain.User.User user,
        Domain.Project.Project project, bool includeProject = false, bool includeUser = false)
    {
        IQueryable<ProjectHistory> histories = Context.ProjectHistories;

        if (includeProject)
            histories = histories.Include(h => h.Project);

        if (includeUser)
            histories = histories.Include(h => h.EditedBy);

        return histories.Where(h => h.EditedBy == user && h.Project == project);
    } // ReadProjectHistoriesByUserAndProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectHistoryRepository.CreateProjectHistory"/>
    /// </summary>
    public ProjectHistory CreateProjectHistory(ProjectHistory projectHistory)
    {
        Context.ProjectHistories.Add(projectHistory);
        Context.SaveChanges();
        return projectHistory;
    } // CreateProjectHistory.
}