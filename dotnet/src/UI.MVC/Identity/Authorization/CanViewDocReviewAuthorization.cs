using BL.User;
using Domain.DocReview;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UI.MVC.Extensions;

namespace UI.MVC.Identity.Authorization;

/// <author> Niels Van Steen </author>
/// <summary>
/// Resource based authorization, that checks whether a <see cref="UserRole.RegularUser"/> can view the <see cref="DocReview"/>.
/// </summary>
public class CanViewDocReviewAuthorization : AuthorizationHandler<IsDocReviewVisibleRequirement, DocReview>
{
    // Methods.
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDocReviewVisibleRequirement requirement, DocReview resource)
    {
        var isManager = context.User.Identity?.Name?.ToLower().EndsWith(ApplicationConstants.BackEndUrlName.ToLower()) ?? false;
        
        // Check if a normal user can view the doc-review.
        if (!isManager && resource.IsDocReviewVisibleForNormalUsers())
            context.Succeed(requirement);

        // Managers can always view the doc-review.
        if (isManager)
            context.Succeed(requirement);

        return Task.CompletedTask;
    } // HandleRequirementAsync.
}

public class IsDocReviewVisibleRequirement : IAuthorizationRequirement
{
    public IsDocReviewVisibleRequirement() {}

    public IsDocReviewVisibleRequirement(DocReview docReview) => DocReview = docReview;

    public DocReview DocReview { get; }
}