using System.ComponentModel.DataAnnotations;
using Domain.Project;

namespace UI.MVC.Models.Dto;

public class ProjectSettingsDto
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="Project.AccessibilityStatement"/>
    /// </summary>
    [Required]
    public string Accessibility { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// <see cref="Project.PrivacyStatement"/>
    /// </summary>
    [Required]
    public string Privacy { get; set; }

    public ProjectSettingsDto(Domain.Project.Project project)
    {
        Privacy = project.PrivacyStatement;
        Accessibility = project.AccessibilityStatement;
    }

    public ProjectSettingsDto()
    {
        
    }
}