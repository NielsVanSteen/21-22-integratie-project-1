using Domain.User;

namespace UI.MVC.Models.Shared;

/// <author> Niels Van Steen </author>
/// <summary>
/// This class holds the <see cref="UserPropertyName"/>s and <see cref="UserPropertyValue"/> for a specific user & project.
/// This class is used to show the extra user information on the screen. as well as fill the fields if the data is present. (values might not be present when registering, but are present when changing the user's information).
/// </summary>
public class ExtraUserInfoModel
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// All the <see cref="UserPropertyName"/> for a specific project.
    /// </summary>
    public IEnumerable<UserPropertyName> UserPropertyNames { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// All the <see cref="UserPropertyValue"/> of a specific user.
    /// </summary>
    public IEnumerable<UserPropertyValue> UserPropertyValues { get; set; }
    
    // Constructors.
    public ExtraUserInfoModel() {}

    public ExtraUserInfoModel(IEnumerable<UserPropertyName> userPropertyNames, IEnumerable<UserPropertyValue> userPropertyValues)
    {
        UserPropertyNames = userPropertyNames;
        UserPropertyValues = userPropertyValues;
    } // ExtraUserInfoModel

    // Methods.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns a <see cref="UserPropertyValue"/> given a <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="userPropertyName"></param>
    /// <returns></returns>
    public UserPropertyValue GetUserPropertyValue(UserPropertyName userPropertyName)
    {
        return UserPropertyValues?.SingleOrDefault(p => p.UserPropertyName == userPropertyName);
    } // GetUserPropertyValue.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns the value of a <see cref="UserPropertyValue"/> given the abstract superclass.
    /// </summary>
    /// <param name="userPropertyValue"></param>
    /// <returns></returns>
    public string GetValueAsString(UserPropertyValue userPropertyValue)
    {
        if (userPropertyValue == null)
            return "";
        
        switch (userPropertyValue)
        {
            case UserPropertyStringValue stringValue:
                return stringValue.Value;;
            case UserPropertyDateValue dateValue:
                return dateValue.Value?.ToString("yyyy-MM-dd");
            case UserPropertyNumericValue numericValue:
                return numericValue.Value.ToString();
            case UserPropertyDecimalValue decimalValue:
                return decimalValue.Value.ToString();
        }

        return "";
    } // GetValueAsString.
}