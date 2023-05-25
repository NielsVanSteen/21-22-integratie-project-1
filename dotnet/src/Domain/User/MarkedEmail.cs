using System.ComponentModel.DataAnnotations;

namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// When u new user registers with an email that exists in this 'MarkedEmail' class that user
/// will be granted the <see cref="UserRole"/> defined in this class, for the given project.
/// Even though <see cref="UserRole"/> can hold any Role, this class is not user for giving normal-user roles. -> but for granting someone a higher role.
/// </summary>
public class MarkedEmail
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// MarkedEmail Id.
    /// </summary>
    [Key]
    public int MarkedEmailId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A user registering with this Email will be granted the role: <see cref="UserRole"/>
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// The role the user will be given when registering.
    /// </summary>
    [Required]
    public UserRole UserRole { get; set; }
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// For which projects the user will be granted the role.
    /// Can be null for <see cref="UserRole.Admin"/>
    /// </summary>
    public ICollection<Project.Project> Projects { get; set; }
    
}