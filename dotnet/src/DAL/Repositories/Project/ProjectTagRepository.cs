using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IProjectTagRepository"/>
/// </summary>
public class ProjectTagRepository : Repository, IProjectTagRepository
{
    // Constructor.
    public ProjectTagRepository(DocReviewDbContext context) : base(context) {}
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.CreateProjectTag"/>
    /// </summary>
    public void CreateProjectTag(ProjectTag projectTag)
    {
        // Create ProjectTag.
        Context.ProjectTags.Add(projectTag);
        Context.Entry(projectTag.Project).State = EntityState.Unchanged;
        Context.SaveChanges();
    } // CreateProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.ReadProjectTagsByProject"/>
    /// </summary>
    public IEnumerable<ProjectTag> ReadProjectTagsByProject(Domain.Project.Project project, bool includeProject = false)
    {
        IQueryable<ProjectTag> projectTags = Context.ProjectTags;

        if (includeProject)
            projectTags = projectTags.Include(t => t.Project);

        return projectTags.Where(t => t.Project == project).AsEnumerable();
    } // ReadProjectTagsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.ReadProjectTag"/>
    /// </summary>
    public ProjectTag ReadProjectTag(int id, bool includeProject = false)
    {
        IQueryable<ProjectTag> projectTags = Context.ProjectTags;

        if (includeProject)
            projectTags = projectTags.Include(t => t.Project);

        return projectTags.SingleOrDefault(t => t.ProjectTagId == id);
    } // ReadProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.UpdateProjectTag"/>
    /// </summary>
    public void UpdateProjectTag(ProjectTag projectTag)
    {
        var tag = Context.ProjectTags.SingleOrDefault(d => d.ProjectTagId == projectTag.ProjectTagId);
        if (tag == null)
            return;
        
        Context.Entry(tag).CurrentValues.SetValues(projectTag);
        Context.SaveChanges();
    } // UpdateProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.DeleteProjectTag"/>
    /// </summary>
    public bool DeleteProjectTag(int id)
    {
        ProjectTag tag = Context.ProjectTags.SingleOrDefault(t => t.ProjectTagId == id);
        if (tag == null)
            return false;
        Context.ProjectTags.Remove(tag);
        Context.SaveChanges();
        return true;
    } // DeleteProjectTag.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectTagRepository.ReadProjectTagByProjectAndName"/>
    /// </summary>
    public ProjectTag ReadProjectTagByProjectAndName(Domain.Project.Project project, string name)
    {
        return Context.ProjectTags.SingleOrDefault(t => t.Project == project && t.Name.ToLower() == name.ToLower());
    } // ReadProjectTagByProjectAndName.
}