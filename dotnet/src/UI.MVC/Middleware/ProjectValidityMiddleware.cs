using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using SendGrid;
using ServiceStack;
using UI.MVC.Controllers;
using UI.MVC.Identity;

namespace UI.MVC.Middleware;

/// <author>Niels Van Steen</author>
/// <summary>
/// Checks is the project name in the url is valid
/// </summary>
public class ProjectValidityMiddleware
{
    // Fields.
    private RequestDelegate _next;

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The strings in this array are lowercase controller names, these controllers can have the <see cref="ApplicationConstants.BackEndUrlName"/> as project name in their url.
    /// </summary>
    private string[] acceptedAdminControllers = {"account", "accountmanage", "projectmoderation", "error", "users", "projectsmoderation", "accounts"};

    private string[] acceptedSignalR = {"docreviewhub"};

    // Constructor.
    public ProjectValidityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Methods.
    public async Task InvokeAsync(HttpContext httpContext, IUserManager userService, IProjectManager projectService,
        UserManager<User> userManager, SignInManager<User> signInManager)
    {
        // Get necessary values.
        string urlProjectName = ApplicationConstants.GetProjectName(httpContext.GetRouteData());
        Project urlProject = projectService.GetProjectByExternalName(urlProjectName);
        var user = userService.GetUser(userManager.GetUserId(httpContext.User), true);

        var controller = httpContext.GetRouteData().Values["Controller"]?.ToString() ?? String.Empty;

        if (controller.ToLower() == "projectstylings")
        {
            await _next(httpContext);
            return;
        }

        // Execute checks.
        if (await CheckIfUserShouldBeLoggedOut(urlProject, user, urlProjectName, controller, userManager,
                signInManager))
        {
            httpContext.Response.Redirect("/" + ApplicationConstants.GetProjectName(httpContext.GetRouteData()) +
                                          "/Account/Login");
            await _next(httpContext);
            return;
        }

        if (CheckProjectValidity(urlProject, urlProjectName, controller))
        {
            httpContext.Response.Redirect("/" + ApplicationConstants.GetProjectName(httpContext.GetRouteData()) +
                                          "/error/NotFound404");
            await _next(httpContext);
            return;
        }

        if (CheckOnlyBackEndValidity(httpContext, controller))
        {
            httpContext.Response.Redirect("/" + ApplicationConstants.GetProjectName(httpContext.GetRouteData()) +
                                          "/error/NotFound404");
            await _next(httpContext);
            return;
        }

        await _next(httpContext);
    } // InvokeAsync.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Checks if the user's login is valid. E.g., the user logs in to project 1. Then changes the url to project 2 -> then the user should get logged out.
    /// This method makes sure of that.
    /// </summary>
    private async Task<bool> CheckIfUserShouldBeLoggedOut(Project urlProject, User user, string urlProjectName,
        string controller, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        if (acceptedSignalR.Contains(controller.ToLower()) || controller.IsEmpty())
        {
            return false;
        }

        if (user == null)
        {
            //await _signInManager.SignOutAsync();
            return false;
        }

        // Admins are not bound to a single project -> they should not be logged out.
        if (await userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
            return false;

        // Check if the currently logged in user is logged in for the correct project otherwise log out.
        // user.RegisteredForProjects.All(p => p.ExternalName.ToLower() != urlProjectName.ToLower()
        if (!user?.RegisteredForProjects?.Contains(urlProject) ?? false)
        {
            if (urlProjectName.ToLower() == ApplicationConstants.BackEndUrlName &&
                acceptedAdminControllers.Contains(controller.ToLower()))
                return false;

            await signInManager.SignOutAsync();

            return true;
        }

        return false;
    } // CheckIfUserShouldBeLoggedOut.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Checks if the project entered in the URL exists in the database. And if the currently logged in user is logged in for the correct project (the project in the url).
    /// </summary>
    private bool CheckProjectValidity(Project urlProject, string urlProjectName, string controller)
    {
        // When project doesn't exist check if the pages are in the accepted lists -> otherwise redirect to 404NotFound.
        var acceptedNames = new string[] {"error"};

        if (acceptedSignalR.Contains(controller.ToLower()) || controller.IsEmpty())
        {
            return false;
        }

        // Return when project exists.
        if (urlProject != null)
            return false;

        // Return when the projectName is in the accepted area (error-page)
        if (acceptedNames.Contains(controller.ToLower()))
            return false;

        // Return when the projectName is admin and the controller is accepted (AccountController, AccountManageController).
        if (urlProjectName.ToLower() == ApplicationConstants.BackEndUrlName.ToLower() &&
            acceptedAdminControllers.Contains(controller.ToLower()))
            return false;

        // Project does not exist and URL is not accepted -> redirect to 404NotFound.
        return true;
    } // CheckProjectValidity.  

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Some controllers are not project specific like E.g., <see cref="ProjectModerationController"/>. Meaning the only valid urlProjectName is the name of <see cref="ApplicationConstants.BackEndUrlName"/>
    /// This method will check if the user is accessing that controller, and if so -> check if the urlProjectName equals <see cref="ApplicationConstants.BackEndUrlName"/>.
    /// </summary>
    private bool CheckOnlyBackEndValidity(HttpContext httpContext, string controller)
    {
        // Specify the controller that are bound by this rule.
        string[] requireAdminControllers = new string[] {"projectmoderation"};

        if (acceptedSignalR.Contains(controller.ToLower()) || controller.IsEmpty())
        {
            return false;
        }

        // The current controller is not in the list -> return.
        if (!requireAdminControllers.Contains(controller.ToLower()))
            return false;

        var urlProjectName = ApplicationConstants.GetProjectName(httpContext.GetRouteData()).ToLower();
        var urlBackEndName = ApplicationConstants.BackEndUrlName.ToLower();

        if (urlProjectName != urlBackEndName)
            return true;
        return false;
    } // CheckOnlyBackEndValidity.
}