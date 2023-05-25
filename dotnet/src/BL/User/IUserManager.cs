using Domain.Project;
using Domain.User;

namespace BL.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// The manager class for <see cref="Domain.User.User"/>
/// </summary>
public interface IUserManager
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Gets a user based on the id.
    /// </summary>
    /// <param name="id">The Id of the user. </param>
    /// <param name="includeProjects">Whether or not to load the navigation property for <see cref="Project"/> of the user.</param>
    /// <param name="includePropertyValues">Whether or not to load the navigation property for <see cref="UserPropertyValue"/> of the user.</param>
    /// <returns>A user object.</returns>
    public Domain.User.User GetUser(string id, bool includeProjects = false, bool includePropertyValues = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Search for a user given the email and project.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="externalProjectName">The project the user should be registered for.</param>
    /// <returns>possibly a user object.</returns>
    public Domain.User.User GetUserByEmailAndProjectNameCaseInsensitive(string email, string externalProjectName);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get a <see cref="Domain.User.User"/> by their username.
    /// </summary>
    /// <param name="userName">The username of the <see cref="Domain.User.User"/></param>
    /// <param name="includeProjects"> Whether or not to load the <see cref="Project"/> navigation property inside the user class. </param>
    /// <param name="includePropertyValues"> Whether or not to load the <see cref="UserPropertyValue"/> navigation property inside the user class. </param>
    /// <returns></returns>
    public Domain.User.User GetUserByUserName(string userName, bool includeProjects = false, bool includePropertyValues = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns all the users who have one of the roles.
    /// </summary>
    /// <param name="userIdentifier">
    /// <see cref="Domain.User.UserRole.Admin"/> and <see cref="Domain.User.UserRole.ProjectManager"/> have an identifier in their userName.
    /// For normal users this identifier is the project name.
    /// </param>
    /// <param name="includeProjects">Whether or not to include the navigation property for <see cref="Domain.User.User.RegisteredForProjects"/></param>
    /// <returns></returns>
    public IEnumerable<Domain.User.User> GetUsersWithHigherRoleThanNormalUser(string userIdentifier, bool includeProjects);
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the project managers for a single project.
    /// </summary>
    /// <param name="project">The project to get all the project managers for.</param>
    /// <param name="userIdentifier">
    /// <see cref="Domain.User.UserRole.Admin"/> and <see cref="Domain.User.UserRole.ProjectManager"/> have an identifier in their userName.
    /// For normal users this identifier is the project name.
    /// </param>
    /// <returns></returns>
    public IEnumerable<Domain.User.User> GetProjectManagersByProject(Domain.Project.Project project, string userIdentifier);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This method will create a user object containing all the basic information about a user when they register.
    /// 
    /// NOTE: For a user to be able to register with the same email (and possibly password) on multiple projects.
    /// The username of the user will be set to the email + '_' + internalProjectName. Making the combination of these 2 unique. and allowing registration per project.
    /// </summary>
    /// <param name="firstname">The fist name of the user.</param>
    /// <param name="lastname">The lastname of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="project">The project the user registers for.</param>
    /// <param name="externalProjectName">The external project name. Used to concatenate to the username. -> uniqueness per project. </param>
    /// <returns>User object, WITH the concatenated username.</returns>
    public Domain.User.User CreateUserToRegister(string firstname, string lastname, string email, Domain.Project.Project project, string externalProjectName);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds a user to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    public void AddUser(Domain.User.User user);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">The updated user object.</param>
    public void ChangeUser(Domain.User.User user);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates the navigation property <see cref="Domain.User.User.RegisteredForProjects"/> in the database.
    /// </summary>
    /// <param name="user">The user object who's navigation property has changed.</param>
    public void ChangeUserAssignedProjects(Domain.User.User user);


    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    /// <returns></returns>
    public bool RemoveUser(string id);

}

