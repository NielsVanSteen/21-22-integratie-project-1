using System.ComponentModel.DataAnnotations;
using Domain.Project;
using Domain.User;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;

namespace UI.MVC.Models.Dto;

public class ProjectDto
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ProjectId"/>
    /// </summary>
    [Required]
    public int ProjectId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.InternalName"/>
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string InternalName { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ExternalName"/>
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ExternalName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ExternalName"/>
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string ProjectTitle { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.FooterLogos"/>
    /// </summary>
    public ICollection<FooterLogo> FooterLogos { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.Introduction"/>
    /// </summary>
    [Required]
    public string Introduction { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ProjectStyling"/>
    /// </summary>
    [Required]
    public Domain.Project.ProjectStyling ProjectStyling { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.PrivacyStatement"/>
    /// </summary>
    [Required]
    public string PrivacyStatement { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.AccessibilityStatement"/>
    /// </summary>
    [Required]
    public string AccessibilityStatement { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The url to the banner image.
    /// </summary>
    [Required]
    public string BannerImageUrl { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The url to the main project logo.
    /// </summary>
    [Required]
    public string LogoUrl { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The url to the manage page of the project.
    /// </summary>
    [Required]
    public string ProjectBackEndUrl { get; set; }
    
    // Constructor.
    public ProjectDto() {}

    public ProjectDto(Domain.Project.Project project)
    {
        ProjectId = project.ProjectId;
        InternalName = project.InternalName;
        ExternalName = project.ExternalName;
        ProjectTitle = project.ProjectTitle;
        FooterLogos = project.FooterLogos;
        Introduction = project.Introduction;
        ProjectStyling = project.ProjectStyling;
        PrivacyStatement = project.PrivacyStatement;
        AccessibilityStatement = project.AccessibilityStatement;

        BannerImageUrl = project.GetProjectBannerImageFullLink(LandscapeImageSize.MD);
        LogoUrl = project.GetProjectLogoFullLink(SquareImageSize.MD);
        ProjectBackEndUrl = "/" + project.ExternalName.ToLower() + "/ProjectManage/Index";
        
    } // ProjectDto.
}