using System.ComponentModel.DataAnnotations;
using Domain.Project;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;

namespace UI.MVC.Models.Android;

public class ProjectAndroidDto
{
    // Properties.
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of the project -> name is not pk => project-timeline is a one-to-one relation and PK's must be of a compatible type for fluent API.
    /// </summary>
    [Key] 
    public int ProjectId { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// This is the name only visible for project-managers, and not for normal users.
    /// It's a unique name each project gets inside a company.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string InternalName { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// This is the unique name for a project gets, which all users will be able to see.
    /// This name will define the URL-prefix.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ExternalName { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The title for a project, This can be non-unique. And gives more information about the project.
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string ProjectTitle { get; set; }
    

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// A quick introduction giving more information about the project.
    /// </summary>
    [Required]
    public string Introduction { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The status of the <see cref="Project"/>.
    /// </summary>
    public string Status{ get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Banner of the <see cref="Project"/>.
    /// </summary>
    public string Banner{ get; set; }
    
    //Constructor
    public ProjectAndroidDto()
    {
        
    }
    
    public ProjectAndroidDto(Domain.Project.Project project)
    {
        ProjectId = project.ProjectId;
        InternalName = project.InternalName;
        ExternalName = project.ExternalName;
        ProjectTitle = project.ProjectTitle;
        Introduction = project.Introduction;
        Status = project.ProjectHistories.OrderBy(dh => dh.EditedOn).Last().ProjectStatus.ToString();
        Banner = project.GetProjectBannerImageFullLink(LandscapeImageSize.MD);
    } // ProjectAndroidDto.
}