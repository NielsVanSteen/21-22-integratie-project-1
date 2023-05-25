using Domain.Project;
using Domain.Util;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <summary>
/// <see cref="IProjectRepository"/>
/// </summary>
public class ProjectRepository : Repository, IProjectRepository
{
    // Constructor.
    public ProjectRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectRepository.ReadProjectByExternalName"/>
    /// </summary>
    public Domain.Project.Project ReadProjectByExternalName(string externalProjectName,
        bool includeProjectHistory = false, bool includeFooterLogos = false, bool includeStyling = false)
    {
        IQueryable<Domain.Project.Project> projects = Context.Projects;
        if (includeProjectHistory)
            projects = projects.Include(p => p.ProjectHistories);
        
        if (includeFooterLogos)
            projects = projects.Include(p => p.FooterLogos);
        
        if (includeStyling)
            projects = projects.Include(p => p.ProjectStyling).ThenInclude(p => p.ThemeStyle);
        
        return projects.SingleOrDefault(p => p.ExternalName.ToLower() == externalProjectName.ToLower());
        
    } // ReadProjectByName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectRepository.ReadProjects"/>
    /// </summary>
    public IEnumerable<Domain.Project.Project> ReadProjects(bool includeLogos = false, bool includeStyling = false, bool includeTags = false, bool includeUsers = false, bool includeTimeLine = false)
    {
        IQueryable<Domain.Project.Project> projects = Context.Projects;

        if (includeLogos)
            projects = projects.Include(p => p.FooterLogos);

        if (includeStyling)
            projects = projects.Include(p => p.ProjectStyling);

        if (includeTags)
            projects = projects.Include(p => p.ProjectTags);

        if (includeUsers)
            projects = projects.Include(p => p.Users);

        if (includeTimeLine)
            projects = projects.Include(p => p.TimeLine);

        return projects;
    } // ReadProjects.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectRepository.ReadProjectByFilters"/>
    /// </summary>
    public IEnumerable<Domain.Project.Project> ReadProjectByFilters(string name, SortOrder sortOrder, Domain.User.User user = null, bool includeHistory = false)
    {
        var projects = user == null ? Context.Projects : Context.Projects.Where(p => p.Users.Contains(user));

        if (includeHistory)
        {
            projects = projects.Include(p => p.ProjectHistories);
        }

        
        // Of the name is null -> all projects will be shown. Otherwise filter on internal | external name.
        if (name != null)
            projects = projects.Where(
                p => p.ExternalName.ToLower().Contains(name) ||
                     p.InternalName.ToLower().Contains(name) ||
                     p.ProjectTitle.ToLower().Contains(name));

        if (sortOrder == SortOrder.Ascending)
            projects = projects.OrderBy(p => p.ExternalName);
        else
            projects = projects.OrderByDescending(p => p.ExternalName);
        
        return projects;
    } // ReadProjectByFilters.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="IProjectRepository.ReadProjectsByUser"/>
    /// </summary>
    public IEnumerable<Domain.Project.Project> ReadProjectsByUser(Domain.User.User user)
    {
        return Context.Projects.Where(p => p.Users.Contains(user)).AsEnumerable();
    } // ReadProjectsByUser.

    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectRepository.CreateProject"/>
    /// </summary>
    public Domain.Project.Project CreateProject(Domain.Project.Project project)
    {
        // The project Histories can't be added yet because they require a project id, which is only generated after the insert.
        // And a project should always have an initial history of 'created'.
        var projectHistories = project.ProjectHistories;
        project.ProjectHistories = null;

        // Add the project, so it gets an ID.
        Context.Projects.Add(project);
        Context.SaveChanges();

        // Give the histories a project ID and add them to the database.
        if (projectHistories != null)
        {
            foreach (var projectHistory in projectHistories)
            {
                projectHistory.ProjectId = project.ProjectId;
                Context.ProjectHistories.Add(projectHistory);
            }
        }

        Context.SaveChanges();
        
        Context.TimeLines.Add(new TimeLine
        {
            ProjectId = project.ProjectId
        });
        Context.SaveChanges();
        
        return project;
    } // CreateProject.

    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// <see cref="IProjectRepository.ReadProjectById"/>
    /// </summary>
    public Domain.Project.Project ReadProjectById(int id)
    {
        return Context.Projects.Find(id);
    } // ReadProjectById.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectRepository.UpdateProject"/>
    /// </summary>
    public Domain.Project.Project UpdateProject(Domain.Project.Project project)
    {
        var tmp = Context.Entry(project);
        tmp.State = EntityState.Modified;
        //tmp.Collection(e => e.FooterLogos).IsModified = false;

        Context.Projects.Update(project);
        Context.SaveChanges();
        
        return project;
    } // UpdateProject.
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="IProjectRepository.DeleteProject"/>
    /// </summary>
    /// <param name="project">The project to be deleted</param>
    public void DeleteProject(Domain.Project.Project project)
    {
        Context.Projects.Remove(project);
        Context.SaveChanges();
    } // DeleteProject.
}