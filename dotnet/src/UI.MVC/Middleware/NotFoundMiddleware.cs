using Microsoft.AspNetCore.Mvc.Controllers;
using UI.MVC.Identity;

namespace UI.MVC.Middleware;

/// <author>Niels Van Steen</author>
/// <summary>
/// This middleware redirect 404 statusCodes (Not Found) to a custom page.
/// </summary>
public class NotFoundMiddleware
{
    // Fields.
    private readonly RequestDelegate _next;
    
    // Constructor.
    public NotFoundMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This method is invoked on each request, and checks for a 404 StatusCode, if so -> redirect the user to a custom page.
    /// </summary>
    /// <param name="httpContext"></param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);
        
        if (httpContext.Response.StatusCode == 404 && !httpContext.Response.HasStarted)
        {
            string originalPath = httpContext.Request.Path.Value;
            httpContext.Items["originalPath"] = originalPath;
            httpContext.Request.Path = "/" + ApplicationConstants.GetProjectName(httpContext.GetRouteData()) + "/error/NotFound404";
            //httpContext.Response.Redirect("/error/NotFound404");
            await _next(httpContext);
        }
    } // InvokeAsync.
}
