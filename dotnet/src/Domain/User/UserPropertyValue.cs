using System.ComponentModel.DataAnnotations;

namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// A user has to give different registration information about themselves each time they register.
/// What that extra information is, is dependant on the project.
/// This class holds the value of 1 property for 1 user. -> It's an intermediate class between <see cref="User"/> and <see cref="UserPropertyName"/>
/// </summary>
public abstract class UserPropertyValue
{
    // Properties.
    [Key]
    public int UserPropertyValueId { get; set; }
    
    /// <summary>
    /// The property name of of the value.
    /// </summary>
    public UserPropertyName UserPropertyName { get; set; }

    /// <summary>
    /// <see cref="UserPropertyName"/>
    /// </summary>
    public int UserPropertyNameId { get; set; }
    
    /// <summary>
    /// The user that this property value belongs to.
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// <see cref="UserPropertyName"/>
    /// </summary>
    [Required]
    public string UserId { get; set; }

    // Constructor.
    protected UserPropertyValue() {}

    protected UserPropertyValue(UserPropertyName userPropertyName, User user)
    {
        UserPropertyName = userPropertyName;
        User = user;
    } // UserPropertyValue.
}