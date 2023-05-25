using System.Linq;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IUserRepository"/>
/// </summary>
public class UserRepository : Repository, IUserRepository
{
    // Constructor.
    public UserRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.ReadUserByEmailAndProjectExternalName"/>
    /// </summary>
    public Domain.User.User ReadUserByEmailAndProjectExternalName(string email, string externalProjectName)
    {
        return Context.Users
            .SingleOrDefault(u => u.Email == email && u.RegisteredForProjects
                .Any(p => p.ExternalName.ToLower() == externalProjectName.ToLower()));
    } // ReadUserByEmailAndProjectNameCaseInsensitive.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.ReadUsersWithHigherRoleThanNormalUser"/>
    /// </summary>
    public IEnumerable<Domain.User.User> ReadUsersWithHigherRoleThanNormalUser(string userIdentifier,
        bool includeProjects = false)
    {
        IQueryable<Domain.User.User> users = Context.Users;

        if (includeProjects)
            users = users.Include(u => u.RegisteredForProjects);

        return users.Where(u => u.NormalizedUserName.Contains(userIdentifier.ToUpper())).AsEnumerable();
    } // ReadUsersWithHigherRoleThanNormalUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.ReadProjectManagersByProject"/>
    /// </summary>
    public IEnumerable<Domain.User.User> ReadProjectManagersByProject(Domain.Project.Project project,
        string userIdentifier)
    {
        return Context.Users.Where(h =>
                h.RegisteredForProjects.Contains(project) && h.NormalizedUserName.Contains(userIdentifier.ToUpper()))
            .AsEnumerable();
    } // ReadProjectManagersByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.UpdateUser"/>
    /// </summary>
    public void UpdateUser(Domain.User.User user)
    {
        // Set the User to Modified and the Collection of UserPropertyValues to not modified.
        var tmpUser = Context.Entry(user);
        tmpUser.State = EntityState.Modified;
        tmpUser.Collection(e => e.UserPropertyValues).IsModified = false;

        // Loop over all the UserPropertyValues of a user and set the state to modified.
        if (user.UserPropertyValues != null)
        {
            foreach (var navigationProperty in user.UserPropertyValues)
            {
                var entityEntry = Context.Entry(navigationProperty);
                entityEntry.State = EntityState.Modified;
                entityEntry.Reference(navProp => navProp.User).IsModified = false;
            }
        }

        // Update the user.
        Context.Users.Update(user);
        Context.SaveChanges();
    } // UpdateUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.UpdateUserAssignedProjects"/>
    /// </summary>
    public void UpdateUserAssignedProjects(Domain.User.User user)
    {
        // This is the list of projects the marked email should have after updating.
        var newProjects = new List<Domain.Project.Project>(user.RegisteredForProjects);

        // Get the user object from the database.
        var userFromDb = Context.Users.Include(m => m.RegisteredForProjects)
            .SingleOrDefault(u => u.Id == user.Id);

        if (userFromDb == null || !userFromDb.RegisteredForProjects.Any())
            return;

        // Remove all the projects that the marked-email had, but shouldn't have anymore. 
        foreach (var oldProject in userFromDb?.RegisteredForProjects?.ToList() ??
                                   Enumerable.Empty<Domain.Project.Project>())
        {
            if (!newProjects.Contains(oldProject))
                userFromDb.RegisteredForProjects.Remove(oldProject);
        }

        // Add the roles the marked-emails currently does not have yet.
        foreach (var newProject in newProjects)
        {
            if (userFromDb.RegisteredForProjects.All(r => r.ProjectId != newProject.ProjectId))
            {
                var newRole = new Domain.Project.Project {ProjectId = newProject.ProjectId};
                Context.Projects.Attach(newRole);
                userFromDb.RegisteredForProjects.Add(newRole);
            }
        }

        Context.SaveChanges();
    } // UpdateUserAssignedProjects.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.ReadUser"/>
    /// </summary>
    public Domain.User.User ReadUser(string id, bool includeProjects = false, bool includePropertyValues = false)
    {
        IQueryable<Domain.User.User> users = Context.Users;

        if (includeProjects)
            users = users.Include(u => u.RegisteredForProjects);

        if (includePropertyValues)
            users = users.Include(u => u.UserPropertyValues);

        return users.SingleOrDefault(u => u.Id == id);
    } // ReadUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.ReadUserByUserName"/>
    /// </summary>
    public Domain.User.User ReadUserByUserName(string userName, bool includeProjects = false,
        bool includePropertyValues = false)
    {
        IQueryable<Domain.User.User> users = Context.Users;

        if (includeProjects)
            users = users.Include(u => u.RegisteredForProjects);

        if (includePropertyValues)
            users = users.Include(u => u.UserPropertyValues);

        return users.SingleOrDefault(u => u.UserName.ToLower() == userName.ToLower());
    } // ReadUserByUserName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.CreateUser"/>
    /// </summary>
    public void CreateUser(Domain.User.User user)
    {
        Context.Users.Add(user);
        Context.SaveChanges();
    } // AddUser.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserRepository.DeleteUser"/>
    /// </summary>
    public bool DeleteUser(string id)
    {
        var user = Context.Users.SingleOrDefault(u => u.Id == id);
        if (user == null)
            return false;

        Context.Users.Remove(user);
        Context.SaveChanges();
        return true;
    } // DeleteUser.
}