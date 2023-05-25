using Domain.Project;
using Microsoft.AspNetCore.Identity;
using MarkedEmail = Domain.User.MarkedEmail;
using UserPropertyName = Domain.User.UserPropertyName;
using UserPropertyValue = Domain.User.UserPropertyValue;

namespace DAL.Repositories.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// The repository interface for the <see cref="Domain.User.User"/>.
/// </summary>
/// <example>
/// </example>
public interface IUserRepository
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get a user by his Id.
    /// </summary>
    /// <param name="id">The Id of the user.</param>
    /// <param name="includeProjects"> Whether or not to load the <see cref="Project"/> navigation property inside the user class. </param>
    /// <param name="includePropertyValues"> Whether or not to load the <see cref="UserPropertyValue"/> navigation property inside the user class. </param>
    /// <returns></returns>
    public Domain.User.User ReadUser(string id, bool includeProjects = false, bool includePropertyValues = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get a <see cref="Domain.User.User"/> by their username.
    /// </summary>
    /// <param name="userName">The username of the <see cref="Domain.User.User"/></param>
    /// <param name="includeProjects"> Whether or not to load the <see cref="Project"/> navigation property inside the user class. </param>
    /// <param name="includePropertyValues"> Whether or not to load the <see cref="UserPropertyValue"/> navigation property inside the user class. </param>
    /// <returns></returns>
    public Domain.User.User ReadUserByUserName(string userName, bool includeProjects = false, bool includePropertyValues = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns a user given the project and email.
    /// Email or Project is not unique. Only together they are.
    /// </summary>
    /// <param name="email">Email of the user.</param>
    /// <param name="externalProjectName">Project the user should be linked to.</param>
    /// <returns> <see cref="User"/> </returns>
    public Domain.User.User ReadUserByEmailAndProjectExternalName(string email, string externalProjectName);

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
    public IEnumerable<Domain.User.User> ReadUsersWithHigherRoleThanNormalUser(string userIdentifier, bool includeProjects);

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
    public IEnumerable<Domain.User.User> ReadProjectManagersByProject(Domain.Project.Project project, string userIdentifier);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds a user to the database.
    /// </summary>
    /// <param name="user">The user object to add.</param>
    public void CreateUser(Domain.User.User user);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates all properties of a user.
    /// </summary>
    /// <param name="user">The updated user objected that should be persisted to the database.</param>
    public void UpdateUser(Domain.User.User user);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates the navigation property <see cref="Domain.User.User.RegisteredForProjects"/> in the database.
    /// </summary>
    /// <param name="user">The user object who's navigation property has changed.</param>
    public void UpdateUserAssignedProjects(Domain.User.User user);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    /// <returns></returns>
    public bool DeleteUser(string id);
}