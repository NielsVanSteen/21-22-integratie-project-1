using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UI.MVC.Identity;

namespace UI.MVC.Middleware;

/// <author> Niels Van Steen </author>
/// <summary>
/// Middleware that checks whether the current user is allowed to view the project page.
/// </summary>
public class ProjectVisibleMiddleware
{
    // Fields.
    private RequestDelegate _next;

    // Constructor.
    public ProjectVisibleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Checks whether the current user is allowed to view the project page.
    /// </summary>
    public async Task InvokeAsync(HttpContext httpContext, IAuthorizationService authorizationService,
        IProjectManager projectManager)
    {
        var projectName = ApplicationConstants.GetProjectName(httpContext.GetRouteData());
        var project = projectManager.GetProjectByExternalName(projectName, true);

        if (project == null)
        {
            await _next(httpContext);
            return;
        }
        
        var authorizationResult = await authorizationService.AuthorizeAsync(httpContext.User, project, ApplicationConstants.CanViewProjectAuthorization);
        if (!authorizationResult.Succeeded)
        {
             httpContext.Response.Redirect("/" + ApplicationConstants.GetProjectName(httpContext.GetRouteData()) + "/error/NotFound404");
             return;
        }
        
        await _next(httpContext);
    } // InvokeAsync.
}