using System.ComponentModel.DataAnnotations;

namespace Domain.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// The name of a property of a user. Specified per project.
/// E.g., Age | Integer | Project 1
/// </summary>
public class UserPropertyName
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int UserPropertyNameId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A <see cref="User"/> can have extra information about themselves. This property stores the name of that information. E.g., 'Age'
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserPropertyLabel { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// An optional description about the <see cref="UserPropertyLabel"/>.
    /// </summary>
    public string Description { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The extra information of a <see cref="User"/> can have different <see cref="UserPropertyType"/>. E.g., 'Age' -> 'Integer'.
    /// </summary>
    public UserPropertyType UserPropertyType { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Whether the <see cref="User"/> is required to enter the <see cref="UserPropertyValue"/>
    /// </summary>
    [Required]
    public bool IsRequired { get; set; }
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// The extra information a <see cref="User"/> has to give about themselves is defined per project.
    /// </summary>
    public Project.Project RegisteredForProject { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="RegisteredForProject"/>
    /// </summary>
    public int RegisteredForProjectId { get; set; }

    // Constructors.
    public UserPropertyName() {}

    public UserPropertyName(string userPropertyLabel, string description, UserPropertyType userPropertyType, Project.Project registeredForProject)
    {
        UserPropertyLabel = userPropertyLabel;
        Description = description;
        UserPropertyType = userPropertyType;
        RegisteredForProject = registeredForProject;
    } // UserPropertyName.
}