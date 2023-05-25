using BL.Comment;
using BL.DocReview;
using BL.Project;
using BL.ProjectStatistics;
using BL.User;
using DAL.Repositories.Comment;
using DAL.Repositories.DocReview;
using DAL.Repositories.Project;
using DAL.Repositories.ProjectStatistics;
using DAL.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using UI.MVC.Identity;
using UI.MVC.Identity.Authorization;

namespace UI.MVC.Middleware;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class holds all the extension methods so they can easily get invoked.
/// </summary>
public static class MiddlewareExtensions
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Middleware for <see cref="NotFoundMiddleware"/>
    /// </summary>
    public static IApplicationBuilder CheckNotFound(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NotFoundMiddleware>();
    } // CheckNotFound.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Middleware for <see cref="ProjectValidityMiddleware"/>
    /// </summary>
    public static IApplicationBuilder CheckProjectValidity(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ProjectValidityMiddleware>();
    } // CheckProjectValidity.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Middleware for <see cref="ProjectVisibleMiddleware"/>
    /// </summary>
    public static IApplicationBuilder CheckProjectVisibleForUser(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ProjectVisibleMiddleware>();
    } // CheckProjectValidity.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds all the custom authorization handlers.
    /// </summary>
    /// <remarks>
    /// <see cref="CanViewProjectAuthorization"/>
    /// <see cref="CanViewDocReviewAuthorization"/>
    /// <see cref="IsLoginRequiredAuthorization"/>
    /// <see cref="AuthorizationMiddleware"/>
    /// </remarks>
    /// <param name="builder"></param>
    public static void AddAuthorizationHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            // Authorization policy to check if a user can view a project.
            options.AddPolicy(ApplicationConstants.CanViewProjectAuthorization, policy =>
                policy.Requirements.Add(new IsProjectVisibleRequirement()));

            // Authorization policy to check if a user can view a doc-review.
            options.AddPolicy(ApplicationConstants.CanViewDocReviewAuthorization, policy =>
                policy.Requirements.Add(new IsDocReviewVisibleRequirement()));

            // Authorization policy to check if a user is authenticated in if the doc-reviews required authenticated users.
            options.AddPolicy(ApplicationConstants.IsLoginRequiredAuthorization, policy =>
                policy.Requirements.Add(new IsLoginRequired()));
        });

        builder.Services.AddSingleton<IAuthorizationHandler, CanViewProjectAuthorization>();
        builder.Services.AddSingleton<IAuthorizationHandler, CanViewDocReviewAuthorization>();
        builder.Services.AddSingleton<IAuthorizationHandler, IsLoginRequiredAuthorization>();

        // Custom Middleware to handle Authorization response codes. (403).
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddleware>();
    } // AddAuthorizationHandlers.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Adds all the managers & repositories for dependency injection. -> keeps program.cs cleaner.
    /// </summary>
    public static void AddDependencyInjectionClasses(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMarkedEmailRepository, MarkedEmailRepository>();
        builder.Services.AddScoped<IUserPropertyRepository, UserPropertyRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IProjectHistoryRepository, ProjectHistoryRepository>();
        builder.Services.AddScoped<IDocReviewHistoryRepository, DocReviewHistoryRepository>();
        builder.Services.AddScoped<ICommentHistoryRepository, CommentHistoryRepository>();
        builder.Services.AddScoped<IProjectTagRepository, ProjectTagRepository>();
        builder.Services.AddScoped<IDocReviewRepository, DocReviewRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<ITimeLineRepository, TimeLineRepository>();
        builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
        builder.Services.AddScoped<IProjectStatisticsRepository, ProjectStatisticsRepository>();
        builder.Services.AddScoped<ICommentTagRepository, CommentTagRepository>();
        builder.Services.AddScoped<IProjectFooterLogoRepository, ProjectFooterLogoRepository>();
        builder.Services.AddScoped<IThemeStylesRepository, ThemeStylesRepository>();

        builder.Services.AddScoped<IUserManager, UserManager>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMarkedEmailManager, MarkedEmailManager>();
        builder.Services.AddScoped<IUserPropertyManager, UserPropertyManager>();
        builder.Services.AddScoped<IProjectHistoryManager, ProjectHistoryManager>();
        builder.Services.AddScoped<IDocReviewHistoryManager, DocReviewHistoryManager>();
        builder.Services.AddScoped<ICommentHistoryManager, CommentHistoryManager>();
        builder.Services.AddScoped<IProjectManager, ProjectManager>();
        builder.Services.AddScoped<IProjectTagManager, ProjectTagManager>();
        builder.Services.AddScoped<IDocReviewManager, DocReviewManager>();
        builder.Services.AddScoped<ICommentManager, CommentManager>();
        builder.Services.AddScoped<ISurveyManager, SurveyManager>();
        builder.Services.AddScoped<ITimeLineManager, TimeLineManager>();
        builder.Services.AddScoped<IEmojiRepository, EmojiRepository>();
        builder.Services.AddScoped<IEmojiManager, EmojiManager>();
        builder.Services.AddScoped<IProjectStatisticsManager, ProjectStatisticsManager>();
        builder.Services.AddScoped<ICommentTagManager, CommentTagManager>();
        builder.Services.AddScoped<IProjectFooterLogoManager, ProjectFooterLogoManager>();
        builder.Services.AddScoped<IThemeStylesManager, ThemeStylesManager>();
    } // AddDependencyInjectionClasses.
}