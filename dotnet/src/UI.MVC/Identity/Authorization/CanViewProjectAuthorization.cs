using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using UI.MVC.Extensions;

namespace UI.MVC.Identity.Authorization;

/// <author> Niels Van Steen </author>
/// <summary>
/// Resource based authorization, that checks whether a <see cref="UserRole.RegularUser"/> can view the project.
/// </summary>
public class CanViewProjectAuthorization : AuthorizationHandler<IsProjectVisibleRequirement, Project>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsProjectVisibleRequirement requirement, Project resource)
    {
         var isManager = context.User.Identity?.Name?.ToLower().EndsWith(ApplicationConstants.BackEndUrlName.ToLower()) ?? false;
        
        // Check if a normal user can view the project.
        if (!isManager && resource.IsProjectVisibleForNormalUsers())
            context.Succeed(requirement);

        // Managers can always view the project.
        if (isManager)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

public class IsProjectVisibleRequirement : IAuthorizationRequirement
{
    public IsProjectVisibleRequirement() {}

    public IsProjectVisibleRequirement(Project project) => Project = project;

    public Project Project { get; }
}