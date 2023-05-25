using Domain.Project;

namespace BL.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// The interface of the <see cref="ProjectTagManager"/>, used for all the operations on the class <see cref="ProjectTag"/>
/// </summary>
public interface IProjectTagManager
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a project tag.
    /// </summary>
    /// <param name="projectTag">The project tag to create. </param>
    public void AddProjectTag(ProjectTag projectTag);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all tags of a project.
    /// </summary>
    /// <param name="project">The project to get all tags for.</param>
    /// <param name="includeProject">Whether or not to include the navigation property <see cref="ProjectTag.Project"/></param>
    /// <returns></returns>
    public IEnumerable<ProjectTag> GetProjectTagsByProject(Domain.Project.Project project, bool includeProject = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read a single project tag.
    /// </summary>
    /// <param name="id">The id of the project tag.</param>
    /// <param name="includeProject">Whether or not to include the navigation property <see cref="ProjectTag.Project"/></param>
    /// <returns></returns>
    public ProjectTag GetProjectTag(int id, bool includeProject = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates a project tag.
    /// </summary>
    /// <param name="projectTag">The project tag to update.</param>
    public void ChangeProjectTag(ProjectTag projectTag);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Deletes a project tag.
    /// </summary>
    /// <param name="id">The id of the tag to delete.</param>
    /// <returns></returns>
    public bool RemoveProjectTag(int id);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads a project tag by the project and name -> those 2 values form a unique index.
    /// </summary>
    /// <param name="project">the project the tag belongs to.</param>
    /// <param name="name">The name of the tag.</param>
    /// <returns></returns>
    public ProjectTag GetProjectTagByProjectAndName(Domain.Project.Project project, string name);
}