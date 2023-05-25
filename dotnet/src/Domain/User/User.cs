using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Domain.Project;

namespace Domain.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// Class containing all user properties.
/// This class is used for a: Normal User, ProjectManager and Admin. -> because Identity provides an easy to implement solution for the different roles.
/// </summary>
public class User : IdentityUser
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The application needs a single 'root' user. This user can't be deleted, and will always be in the database with seeding.
    /// This user has the <see cref="UserRole.Admin"/> role and is used to create the other <see cref="UserRole.Admin"/>.
    /// </summary>
    /// <remarks>
    /// This const is placed in the domain class since each layer will have access to it.
    /// </remarks>
    public const string RootUserId = "5938ddaa-f5e7-4259-8ebe-2b35c14326d8";
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The user's first name.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Firstname { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The user's last name.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Lastname { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// By default a user doesn't have a profile picture. -> this boolean is set to false. and a default image is used.
    /// When the user has a profile picture. the name of the image would be the <see cref="User.Id"/>. This way no database storage is wasted storing the image string.
    /// </summary>
    public bool HasProfilePicture { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A <see cref="User"/> Is not registered globally but per <see cref="Project"/>
    /// A <see cref="User"/> with the role <see cref="UserRole.ProjectManager"/> can be registered for multiple <see cref="Project"/>s.
    /// A <see cref="User"/> with the role <see cref="UserRole.Admin"/> is not linked to a <see cref="Project"/>. But is registered globally.
    /// </summary>
    // NOTE: must be a list, even IList doesn't have the method 'AddRange'
    public List<Project.Project> RegisteredForProjects { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Stores all the extra user information defined per project.
    /// </summary>
    /// <see cref="UserPropertyValues"> for more information/clarification.</see>
    public IEnumerable<UserPropertyValue> UserPropertyValues { get; set; }

    // Constructor.
    public User() {}

    public User(string firstname)
    {
        Firstname = firstname;
    }
}