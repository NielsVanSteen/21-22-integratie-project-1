using System.ComponentModel.DataAnnotations;

namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// A specific implementation for <see cref="UserPropertyValue"/>
/// Holds no other information other than to specify the generic type.
/// </summary>
public class UserPropertyStringValue : UserPropertyValue
{
    // Fields.
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// This property holds the value of a <see cref="UserPropertyName"/> for a single <see cref="User"/> E.g., '20' for property 'Age'.
    /// Where T instance of <see cref="UserPropertyType"/>
    /// </summary>
    public string Value { get; set; }
    
    // Constructor.
    public UserPropertyStringValue() {}

    public UserPropertyStringValue(UserPropertyName userPropertyName, User user, string value) : base(userPropertyName, user)
    {
        Value = value;
    } // UserPropertyStringValue.
}