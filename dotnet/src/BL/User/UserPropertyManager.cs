using System.ComponentModel.DataAnnotations;
using DAL.Repositories.User;
using Domain.User;

namespace BL.User;

/// <summary>
/// <see cref="IUserPropertyManager"/>
/// </summary>
public class UserPropertyManager : IUserPropertyManager
{
    
    // Fields.
    private IUserPropertyRepository _repository;

    // Constructor.
    public UserPropertyManager(IUserPropertyRepository repository)
    {
        _repository = repository;
    } // UserService.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.AddUserPropertyName"/>
    /// </summary>
    public UserPropertyName AddUserPropertyName(UserPropertyName userPropertyName)
    {
        Validator.ValidateObject(userPropertyName, new ValidationContext(userPropertyName), validateAllProperties: true);
        return _repository.CreateUserPropertyName(userPropertyName);
    } // AddUserPropertyName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.ChangeUserPropertyName"/>
    /// </summary>
    public void ChangeUserPropertyName(UserPropertyName userPropertyName)
    {
        Validator.ValidateObject(userPropertyName, new ValidationContext(userPropertyName), validateAllProperties: true);
        _repository.UpdateUserPropertyName(userPropertyName);
    } // ChangeUserPropertyName

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.GetUserPropertyNameByProjectAndLabel"/>
    /// </summary>
    public UserPropertyName GetUserPropertyNameByProjectAndLabel(Domain.Project.Project project, string userPropertyLabel)
    {
        return _repository.ReadUserPropertyNameByProjectAndLabel(project, userPropertyLabel);
    } // GetUserPropertyNameByProjectAndLabel.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.GetUserPropertyNamesByProject"/>
    /// </summary>
    public IEnumerable<UserPropertyName> GetUserPropertyNamesByProject(Domain.Project.Project project)
    {
        return _repository.ReadUserPropertyNamesByProject(project);
    }  // GetUserPropertyNamesByProject.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.GetUserPropertyValueByUserAndProperty"/>
    /// </summary>
    public UserPropertyValue GetUserPropertyValueByUserAndProperty(Domain.User.User user, UserPropertyName userPropertyName)
    {
        return _repository.ReadUserPropertyValueByUserAndProperty(user, userPropertyName);
    } // GetUserPropertyValueByUserAndProperty.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.RemoveUserPropertyName"/>
    /// </summary>
    public bool RemoveUserPropertyName(int id)
    {
        return _repository.DeleteUserPropertyName(id);
    } // RemoveUserPropertyName

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyManager.GetUserPropertyName"/>
    /// </summary>
    public UserPropertyName GetUserPropertyName(int id)
    {
        return _repository.ReadUserPropertyName(id);
    } // GetUserPropertyName.


    
}