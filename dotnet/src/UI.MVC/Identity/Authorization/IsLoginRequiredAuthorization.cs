using Domain.DocReview;
using Microsoft.AspNetCore.Authorization;

namespace UI.MVC.Identity.Authorization;

/// <author> Niels Van Steen </author>
/// <summary>
/// Checks if a doc-reviews required users to be authenticated and the user is authenticated.
/// </summary>
public class IsLoginRequiredAuthorization : AuthorizationHandler<IsLoginRequired, DocReview>
{
    // Methods.
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsLoginRequired requirement, DocReview resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false) && resource.DocReviewSettings.IsLogInRequired)
            return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    } // HandleRequirementAsync.
}

public class IsLoginRequired : IAuthorizationRequirement
{
    public IsLoginRequired()
    {
    }

    public IsLoginRequired(DocReview docReview) => DocReview = docReview;

    public DocReview DocReview { get; }
}