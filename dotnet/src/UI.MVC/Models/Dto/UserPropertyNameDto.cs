using System.ComponentModel.DataAnnotations;
using Domain.Project;
using Domain.User;

namespace UI.MVC.Models.Dto;

///<author>Niels Van Steen</author>
/// <summary>
/// <see cref="UserPropertyName"/>
/// </summary>
public class UserPropertyNameDto
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="UserPropertyName.UserPropertyNameId"/>.
    /// </summary>
    public int? UserPropertyNameId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="UserPropertyName.UserPropertyLabel"/>.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserPropertyLabel { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="UserPropertyName.Description"/>.
    /// </summary>
    public string Description { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="UserPropertyName.UserPropertyType"/>.
    /// </summary>
    [Required]
    public UserPropertyType UserPropertyType { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  <see cref="UserPropertyName.IsRequired"/>
    /// </summary>
    [Required]
    public bool IsRequired { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Project.ExternalName"/>.
    /// </summary>
    public string ProjectExternalName { get; set; }
    
    // Constructor.
    public UserPropertyNameDto() {}
    
    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Converts the DTO object in to the domain model one.
    /// </summary>
    /// <param name="project">The project object, with the external name of <see cref="ProjectExternalName"/></param>
    /// <returns></returns>
    public UserPropertyName ConvertToUserPropertyName(Domain.Project.Project project)
    {
        var userPropertyName = new UserPropertyName
        {
            UserPropertyNameId = this.UserPropertyNameId ?? 0,
            UserPropertyLabel = this.UserPropertyLabel,
            UserPropertyType = this.UserPropertyType,
            Description = this.Description,
            IsRequired = this.IsRequired,
            RegisteredForProject = project,
            RegisteredForProjectId = project.ProjectId
        };

        return userPropertyName;
    } // ConvertToUserPropertyName.
}