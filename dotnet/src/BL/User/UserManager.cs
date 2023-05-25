using System.ComponentModel.DataAnnotations;
using DAL.Repositories.User;
using Domain.User;

namespace BL.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IUserManager"/>
/// </summary>
public class UserManager : IUserManager
{
    // Fields.
    private IUserRepository _repository;

    // Constructor.
    public UserManager(IUserRepository repository)
    {
        _repository = repository;
    } // UserService.
    
    
    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.CreateUserToRegister"/>
    /// </summary>
    public Domain.User.User CreateUserToRegister(string firstname, string lastname, string email, Domain.Project.Project project, string externalProjectName)
    {
        var projects = new List<Domain.Project.Project>();
        
        if (project != null) 
            projects.Add(project);
        
        return new Domain.User.User
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            RegisteredForProjects = projects,
            UserName = email.ToLower() + "_" +externalProjectName.ToLower() //Username is a concatenation of Email and Project -> allow registration per project. make email + project unique.
        };
    } // CreateUserToRegister.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.GetUserByEmailAndProjectNameCaseInsensitive"/>
    /// </summary>
    public Domain.User.User GetUserByEmailAndProjectNameCaseInsensitive(string email, string externalProjectName)
    {
        return _repository.ReadUserByEmailAndProjectExternalName(email, externalProjectName);
    } // GetUserByEmailAndProjectNameCaseInsensitive.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.GetUsersWithHigherRoleThanNormalUser"/>
    /// </summary>
    public IEnumerable<Domain.User.User> GetUsersWithHigherRoleThanNormalUser(string userIdentiFier, bool includeProjects)
    {
        return _repository.ReadUsersWithHigherRoleThanNormalUser(userIdentiFier, includeProjects);
    } // GetUsersWithHigherRoleThanNormalUser

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.GetProjectManagersByProject"/>
    /// </summary>
    public IEnumerable<Domain.User.User> GetProjectManagersByProject(Domain.Project.Project project, string userIdentifier)
    {
        return _repository.ReadProjectManagersByProject(project, userIdentifier);
    } // ReadProjectManagersByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.ChangeUser"/>
    /// </summary>
    public void ChangeUser(Domain.User.User user)
    {
         Validator.ValidateObject(user, new ValidationContext(user), validateAllProperties: true);
        _repository.UpdateUser(user);
    } // ChangeUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.ChangeUserAssignedProjects"/>
    /// </summary>
    public void ChangeUserAssignedProjects(Domain.User.User user)
    {
         Validator.ValidateObject(user, new ValidationContext(user), validateAllProperties: true);
        _repository.UpdateUserAssignedProjects(user);
    } // UpdateUserAssignedProjects.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.GetUser"/>
    /// </summary>
    public Domain.User.User GetUser(string id, bool includeProjects = false, bool includePropertyValues = false)
    {
        return _repository.ReadUser(id, includeProjects, includePropertyValues);
    } // GetUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.GetUserByUserName"/>
    /// </summary>
    public Domain.User.User GetUserByUserName(string userName, bool includeProjects = false, bool includePropertyValues = false)
    {
        return _repository.ReadUserByUserName(userName, includeProjects, includePropertyValues);
    } // GetUserByUserName.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.AddUser"/>
    /// </summary>
    public void AddUser(Domain.User.User user)
    {
        Validator.ValidateObject(user, new ValidationContext(user), validateAllProperties: true);
        _repository.CreateUser(user);
    } // AddUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserManager.RemoveUser"/>
    /// </summary>
    public bool RemoveUser(string id)
    {
        return _repository.DeleteUser(id);
    } // RemoveUser.

}