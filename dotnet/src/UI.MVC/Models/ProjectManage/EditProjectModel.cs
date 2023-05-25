using System.ComponentModel.DataAnnotations;
using Domain.Project;
using UI.MVC.Attributes;

namespace UI.MVC.Models.ProjectManage;

/// <author> Niels Van Steen </author>
/// <summary>
/// Model to edit the main project information.
/// </summary>
public class EditProjectModel
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Project.ProjectTitle"/>
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 5)]
    [Display(Name = "Title")]
    public string ProjectTitle { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Project logo file.
    /// </summary>
    [MaxFileSize(MaxFileSizeAttribute.DefaultMaxFileSizeInBytes)]
    [AllowedExtensions(".jpg", ".png", ".jpeg", ".svg", ".gif")]
    [Display(Name = "Logo")]
    public IFormFile ProjectLogo { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Project logo file.
    /// </summary>
    [MaxFileSize(MaxFileSizeAttribute.DefaultMaxFileSizeInBytes)]
    [AllowedExtensions(".jpg", ".png", ".jpeg", ".svg", ".gif")]
    [Display(Name = "Banner Image")]
    public IFormFile ProjectBannerImage { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="Project.Introduction"/>
    /// </summary>
    [Required]
    public string Introduction { get; set; }

    // Constructor.
    public EditProjectModel()
    {
    }
}