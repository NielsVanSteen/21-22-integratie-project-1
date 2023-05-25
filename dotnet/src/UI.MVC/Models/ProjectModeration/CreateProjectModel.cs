using System.ComponentModel.DataAnnotations;
using Domain.Project;
using UI.MVC.Attributes;

namespace UI.MVC.Models.ProjectModeration;

/// <author>Niels Van Steen</author>
/// <summary>
/// The model used to create a new project.
/// This only contains the most vital information, later project-managers can edit the project information and add more detailed information.
/// </summary>
public class CreateProjectModel
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ProjectTitle"/>
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string ProjectTitle { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.InternalName"/>
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5)]
    [AllowedCharacters(AllowedCharactersAttribute.AllowedCharactersOptions.AlphaNumeric, '_', '-')]
    public string InternalName { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ExternalName"/>
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5)]
    [AllowedCharacters(AllowedCharactersAttribute.AllowedCharactersOptions.AlphaNumeric, '_', '-')]
    public string ExternalName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.GetProjectLogoName"/>
    /// </summary>
    [AllowedExtensions(".png", ".jpg", ".jpeg", ".svg", ".gif")]
    [MaxFileSize(5)]
    [Required]
    public IFormFile ProjectLogo { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.GetProjectBannerImageName"/>
    /// </summary>
    [AllowedExtensions(".png", ".jpg", ".jpeg", ".svg", ".gif")]
    [MaxFileSize(12)]
    [Required]
    public IFormFile ProjectBannerImage { get; set; }
    
    // Constructor.
    public CreateProjectModel() {}
    
    // Methods.
}