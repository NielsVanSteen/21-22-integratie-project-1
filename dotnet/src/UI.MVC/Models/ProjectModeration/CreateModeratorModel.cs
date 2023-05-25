using System.ComponentModel.DataAnnotations;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UI.MVC.Models.ProjectModeration;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model used to create a <see cref="MarkedEmail"/> object.
/// </summary>
public class CreateModeratorModel : IValidatableObject
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The role the marked-email has.
    /// </summary>
    [Required]
    public UserRole UserRole { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Parsed version of <see cref="AssignedProjectIds"/>
    /// </summary>
    public ICollection<int> AssignedProjectIds { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A list with the already assigned projects. -> when validation fails and the view is returned the assigned project will also remain because of this list.
    /// </summary>
    public ICollection<Domain.Project.Project> Projects { get; set; }
    
    // Constructor.
    public CreateModeratorModel() {}
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// When the role is <see cref="Domain.User.UserRole.ProjectManager"/> the <see cref="AssignedProjectIds"/> must be greater or equal to 1.
    /// a <see cref="Domain.User.UserRole.ProjectManager"/> without any assigned projects is virtually a <see cref="Domain.User.UserRole.RegularUser"/>.
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var result = new List<ValidationResult>();
        
        if (UserRole == UserRole.ProjectManager && (AssignedProjectIds == null || AssignedProjectIds.Count < 1))
            result.Add(new ValidationResult($"A {UserRole.ProjectManager} must have at least 1 project.",
            new string[] {"UserRole", "AssignedProjectIds"}));

        return result;
    } // Validate.
}