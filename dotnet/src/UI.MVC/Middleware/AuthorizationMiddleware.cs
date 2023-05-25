using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using UI.MVC.Identity;

namespace UI.MVC.Middleware;

/// <author>Niels Van Steen</author>
/// <summary>
/// This middleware redirect 403 statusCodes (forbidden/access denied) to a custom page.
/// </summary>
public class AuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This method gets invoked on each request and checks if the StatusCode is 403, if so -> it redirects the user to a custom page.
    /// </summary>
    public async Task HandleAsync(RequestDelegate requestDelegate, HttpContext httpContext, AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult policyAuthorizationResult)
    {
        // Access is forbidden (user is authenticated but doesn't have access).
        if ( policyAuthorizationResult.Forbidden)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var projectName = ApplicationConstants.GetProjectName(httpContext.GetRouteData());
            httpContext.Response.Redirect("/" + projectName + "/error/Forbidden403");
            return;
        }

        // Challenged (user is NOT authenticated, but tries to access an authorized endpoint).
        if (policyAuthorizationResult.Challenged)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            var projectName = ApplicationConstants.GetProjectName(httpContext.GetRouteData());
            httpContext.Response.Redirect("/" + projectName + "/account/login");
            return;
        }
        
        // Fallback to the default implementation.
        await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, 
            policyAuthorizationResult);
    } // HandleAsync.

}