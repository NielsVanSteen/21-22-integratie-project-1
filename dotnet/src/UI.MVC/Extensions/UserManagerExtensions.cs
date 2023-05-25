using System.Security.Claims;
using BL.User;
using Domain.Project;
using Domain.User;
using identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.Shared;

namespace UI.MVC.Extensions;

public static class UserManagerExtensions
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This method is called when a user registers, either with a local account or with an external provider.
    /// This method checks whether the <see cref="RegisterModel"/> has valid inputs. including the optional registration information. (<see cref="ExtraUserInfoParseModel.UserPropertyValues"/>)
    /// This method also takes into account that <see cref="UserRole.Admin"/> and <see cref="UserRole.ProjectManager"/> do not have to enter this optional information,
    /// Since their registration is not bound to a project.
    /// </summary>
    /// <param name="markedEmailManager"><see cref="IMarkedEmailManager"/><</param>
    /// <param name="userPropertyService"><see cref="UserPropertyManager"/></param>
    /// <param name="projectName">
    /// Name of the project the user is registering for. This must be included explicitly since <paramref name="project"/> can be null.
    /// -> then <paramref name="projectName"/> will hold the 'admin' -> <see cref="ApplicationConstants.BackEndUrlName"/> value.
    /// </param>
    /// <param name="project">The project the user is registering for. Can be 'null' in case of registration for <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/> </param>
    /// <param name="registerModel"></param>
    /// <param name="modelState">ModelState, is used to check for validity, and possibly to add errors to.</param>
    /// <param name="mngr"></param>
    /// <returns>boolean, whether or not the validation has succeeded. True = Succeeded.</returns>
    public static bool ValidateUserToRegister(this UserManager<User> mngr, IMarkedEmailManager markedEmailManager, IUserPropertyManager userPropertyService ,string projectName,
        Project project, RegisterModel registerModel, ModelStateDictionary modelState)
    {
        var userPropertyNames = userPropertyService.GetUserPropertyNamesByProject(project);

        // Maps all the user properties to their corresponding classes and adds modelErrors if necessary. (Only when not on the admin login register page).
        if (projectName != ApplicationConstants.BackEndUrlName.ToLower())
            registerModel.ParseUserPropertyValues(userPropertyNames.ToList(), modelState);

        // Check if the user exists in the MarkedEmail.
        MarkedEmail markedEmail = markedEmailManager.GetMarkedEmailByEmail(registerModel.Email);
        if (projectName == ApplicationConstants.BackEndUrlName.ToLower() && markedEmail == null)
        {
            modelState.AddModelError("Toegang geweigerd!", "U Heeft geen toegang hier te registreren.");
            return true;
        } // If.

        // Check if ModelState is valid.
        if (!modelState.IsValid)
            return true;

        return false;
    } // Test.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This method is the continuation of <see cref="ValidateUserToRegister"/>
    /// This method adds the parsed properties to the user objects. And tries to create the user in the database.
    /// </summary>
    /// <param name="projectName">
    /// Name of the project the user is registering for. This must be included explicitly since <paramref name="project"/> can be null.
    /// -> then <paramref name="projectName"/> will hold the 'admin' -> <see cref="ApplicationConstants.BackEndUrlName"/> value.
    /// </param>
    /// <param name="project">The project the user is registering for. Can be 'null' in case of registration for <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/> </param>
    /// <param name="registerModel"></param>
    /// <param name="user"></param>
    /// <returns>result of the user creation.</returns>
    public static async Task<IdentityResult> CreateUserForRegistration(this UserManager<User> userManager,
        string projectName, Project project, RegisterModel registerModel, User user)
    {
        // Create a user object.
        registerModel.AddUserToProperties(user);
        user.UserPropertyValues = registerModel.UserPropertyValues;

        // Try to create the user in the database.
        return await userManager.CreateAsync(user, registerModel.Password);
    } // Test2.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds the user to a role after registration.
    /// </summary>
    /// <param name="userManager">The extension method type.</param>
    /// <param name="projectName">
    /// Name of the project the user is registering for. This must be included explicitly since <paramref name="project"/> can be null.
    /// -> then <paramref name="projectName"/> will hold the 'admin' -> <see cref="ApplicationConstants.BackEndUrlName"/> value.
    /// </param>
    /// <param name="user">The user to add the role to.</param>
    /// <param name="markedEmail"><see cref="MarkedEmail"/> Possibly holds the role for the user. otherwise user will receive the default role.</param>
    public static async Task AddToCustomRole(this UserManager<User> userManager, string projectName, User user,
        MarkedEmail markedEmail)
    {
        // Add the user to role.
        if (projectName == ApplicationConstants.BackEndUrlName.ToLower() && markedEmail != null)
            await userManager.AddToRoleAsync(user, markedEmail.UserRole.ToString());
        else
            await userManager.AddToRoleAsync(user, UserRole.RegularUser.ToString());
    } // AddToCustomRole.
    
     /// <author> Niels Van Steen</author>
    /// <summary>
    /// Generates a username.
    /// 
    /// The email is not unique in the database, -> user's register themselves per <see cref="Project"/>
    /// Thus the combination of <see cref="User.Email"/> and <see cref="Project"/> is unique.
    /// To store this easily in the database the <see cref="User.UserName"/> is the concatenation of <see cref="User.Email"/> and <see cref="Project.ExternalName"/>.
    /// This methods generates the <see cref="User.UserName"/> like that.
    /// 
    /// Note: <see cref="UserRole.ProjectManager"/> and <see cref="UserRole.Admin"/> do not register on project-basis their username will be the concatenation of:
    /// both <see cref="User.Email"/> and <see cref="ApplicationConstants.BackEndUrlName"/>. -> this ensures the <see cref="User.Email"/> is unique for
    /// all <see cref="UserRole.ProjectManager"/> and <see cref="UserRole.Admin"/>.
    /// </summary>
    /// <param name="userManager">Extension method is for the class <see cref="Microsoft.AspNetCore.Identity.UserManager{T}"/></param>
    /// <param name="email">The <see cref="User.Email"/></param>
    /// <param name="externalProjectName">
    /// For: <see cref="UserRole.RegularUser"/> this is the <see cref="Project.ExternalName"/> they are registering for.
    /// 
    /// For: <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/> this is the <see cref="ApplicationConstants.BackEndUrlName"/>.
    /// </param>
    /// <returns></returns>
    public static string GenerateUsername(this UserManager<User> userManager, string email, string externalProjectName)
    {
        return $"{email}_{externalProjectName.ToLower()}";
    } // GenerateUsername.
     
     /// <author> Sander Verheyen </author>
     /// <summary>
     /// Give the fullname of the user if the user was removed return anoniem.
     /// </summary>
     /// <param name="userManager"></param>
     /// <param name="user"></param>
     /// <returns></returns>
     public static string GetFullName(this UserManager<User> userManager, User user)
     {
         return user == null ? "Anoniem" : $"{user.Firstname} {user.Lastname}";
     }

     /// <author>Michiel Verschueren</author>
     /// <summary>
     /// Checks if a user is authenticated and is a <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/>.
     /// </summary>
     /// <param name="userManager"></param>
     /// <param name="userClaims"></param>
     /// <returns></returns>
     public static async Task<bool> IsModerator(this UserManager<User> userManager, ClaimsPrincipal userClaims)
     {
         // Check if the user is authenticated.
         var user = await userManager.GetUserAsync(userClaims);
         if (user == null)
             return false;
         
         // Check if the user is a moderator.
         var isAdmin = await userManager.IsInRoleAsync(user, UserRole.Admin.ToString());
         var isProjectManager = await userManager.IsInRoleAsync(user, UserRole.ProjectManager.ToString());
         
         return isAdmin || isProjectManager;
     }
}