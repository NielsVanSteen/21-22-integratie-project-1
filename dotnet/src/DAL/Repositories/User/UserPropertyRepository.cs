using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IUserPropertyRepository"/>
/// </summary>
public class UserPropertyRepository : Repository, IUserPropertyRepository
{
    // Constructor.
    public UserPropertyRepository(DocReviewDbContext context) : base(context) {}

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.CreateUserPropertyName"/>
    /// </summary>
    public UserPropertyName CreateUserPropertyName(UserPropertyName userPropertyName)
    {
        Context.UserPropertiesNames.Add(userPropertyName);
        Context.Entry(userPropertyName.RegisteredForProject).State = EntityState.Unchanged;
        Context.SaveChanges();
        return userPropertyName;
    } // CreateUserPropertyName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.CreateUserPropertyName"/>
    /// </summary>
    public void UpdateUserPropertyName(UserPropertyName userPropertyName)
    {
        var tag = Context.UserPropertiesNames.SingleOrDefault(d => d.UserPropertyNameId == userPropertyName.UserPropertyNameId);
        if (tag == null)
            return;
        
        Context.Entry(tag).CurrentValues.SetValues(userPropertyName);
        Context.SaveChanges();
    } // UpdateUserPropertyName

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.CreateUserPropertyName"/>
    /// </summary>
    public UserPropertyName ReadUserPropertyNameByProjectAndLabel(Domain.Project.Project project, string userPropertyLabel)
    {
        return Context.UserPropertiesNames.SingleOrDefault(u =>
            u.RegisteredForProjectId == project.ProjectId &&
            u.UserPropertyLabel.ToLower() == userPropertyLabel.ToLower());
    } // ReadUserPropertyNameByProjectAndLabel.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.ReadUserPropertyNamesByProject"/>
    /// </summary>
    public IEnumerable<UserPropertyName> ReadUserPropertyNamesByProject(Domain.Project.Project project)
    {
        return Context.UserPropertiesNames.Where(u => u.RegisteredForProject == project).AsEnumerable();
    } // ReadUserPropertyNamesByProject.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.ReadUserPropertyValueByUserAndProperty"/>
    /// </summary>
    public UserPropertyValue ReadUserPropertyValueByUserAndProperty(Domain.User.User user, UserPropertyName userPropertyName)
    {
        var value = Context.UserPropertiesValues.SingleOrDefault(v =>
            v.User == user && v.UserPropertyName == userPropertyName);
        return value;
    } // ReadUserPropertyValueByUserAndProperty.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.DeleteUserPropertyName"/>
    /// </summary>
    public bool DeleteUserPropertyName(int id)
    {
        var userPropertyName = Context.UserPropertiesNames.SingleOrDefault(t => t.UserPropertyNameId == id);
        if (userPropertyName == null)
            return false;
        Context.UserPropertiesNames.Remove(userPropertyName);
        Context.SaveChanges();
        return true;
    } // DeleteUserPropertyName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IUserPropertyRepository.ReadUserPropertyName"/>
    /// </summary>
    public UserPropertyName ReadUserPropertyName(int id)
    {
        return Context.UserPropertiesNames.Find(id);
    } // ReadUserPropertyName.
}