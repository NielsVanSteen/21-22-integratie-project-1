using DAL.FluentApi.User;
using Domain.User;

namespace BL.User;

/// <summary>
/// The manager interface for all the classes concerning the user Properties.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.User.UserPropertyValue"/>
/// <see cref="Domain.User.UserPropertyName"/>
/// <see cref="Domain.User.UserPropertyStringValue"/>
/// <see cref="Domain.User.UserPropertyDateValue"/>
/// <see cref="Domain.User.UserPropertyNumericValue"/>
/// <see cref="Domain.User.UserPropertyDecimalValue"/>
/// </example>
public interface IUserPropertyManager
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a new <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="userPropertyName">The object to create.</param>
    /// <returns></returns>
    public UserPropertyName AddUserPropertyName(UserPropertyName userPropertyName);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates an existing <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="userPropertyName"></param>
    public void ChangeUserPropertyName(UserPropertyName userPropertyName);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Reads a userPropertyName by the project and the name, (those two have a unique index <see cref="UserPropertyNameEntityConfiguration"/>
    /// </summary>
    /// <param name="project"></param>
    /// <param name="userPropertyLabel"></param>
    /// <returns></returns>
    public UserPropertyName GetUserPropertyNameByProjectAndLabel(Domain.Project.Project project, string userPropertyLabel);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all the <see cref="UserPropertyName"/> that belong to a single project.
    /// </summary>
    /// <param name="project">The project all the <see cref="UserPropertyName"/> objects belong to. </param>
    /// <returns>List of <see cref="UserPropertyName"/> </returns>
    public IEnumerable<UserPropertyName> GetUserPropertyNamesByProject(Domain.Project.Project project);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns a property value based on the <see cref="user"/> and <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userPropertyName"></param>
    /// <returns></returns>
    public UserPropertyValue GetUserPropertyValueByUserAndProperty(Domain.User.User user, UserPropertyName userPropertyName);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Removes a <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="id">The id of the <see cref="UserPropertyName"/></param>
    /// <returns></returns>
    public bool RemoveUserPropertyName(int id);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get a <see cref="UserPropertyName"/> given the id.
    /// </summary>
    /// <param name="id">The id of the <see cref="UserPropertyName"/></param>
    /// <returns></returns>
    public UserPropertyName GetUserPropertyName(int id);


}