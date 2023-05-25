using System.ComponentModel.DataAnnotations;
using Domain.Project;

namespace UI.MVC.Models.Dto;

/// <author>Niels Van Steen</author>
/// <summary>
/// DTO counterpart of <see cref="ProjectTag"/>
/// </summary>
public class ProjectTagDto
{
 
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectTag.ProjectTagId"/>
    /// </summary>
    public int ProjectTagId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectTag.Name"/>
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The external name of the project. the tag belongs to.
    /// </summary>
    public string ProjectExternalName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectTag.Color"/>
    /// </summary>
    [Required]
    public string Color { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectTag.IsTextWhite"/>
    /// </summary>
    public bool IsTextWhite { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectTag.IsPublic"/>
    /// </summary>
    public bool IsPublic { get; set; }

    // Constructor.
    public ProjectTagDto() {}
    
    // Methods.

    /// <author>Niels Van Steen</author>    
    /// <summary>
    /// Converts the DTO in to the normal projectTag.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public ProjectTag ConvertToProjectTag(Domain.Project.Project project)
    {
        var projectTag = new ProjectTag()
        {
            ProjectTagId = this.ProjectTagId,
            Name = this.Name,
            Color = this.Color,
            IsPublic = this.IsPublic,
            IsTextWhite = this.IsTextWhite,
            Project = project,
            ProjectId = project.ProjectId
        };

        return projectTag;
    } // ConvertToProjectTag.

}