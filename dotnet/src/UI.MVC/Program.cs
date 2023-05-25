using System.Diagnostics;
using BL.Comment;
using BL.DocReview;
using BL.Project;
using BL.ProjectStatistics;
using BL.User;
using DAL;
using DAL.Repositories.Comment;
using DAL.Repositories.DocReview;
using DAL.Repositories.Project;
using DAL.Repositories.ProjectStatistics;
using DAL.Repositories.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UI.MVC.CloudStorage;
using UI.MVC.Identity;
using UI.MVC.Identity.Authorization;
using UI.MVC.Middleware;
using UI.MVC.Models.Hub;
using AuthorizationMiddleware = UI.MVC.Middleware.AuthorizationMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add DB context.
var connectionString = $"server={builder.Configuration["MYSQL_SERVER"]};uid={builder.Configuration["MYSQL_USER"]};pwd={builder.Configuration["MYSQL_PASSWORD"]};database={builder.Configuration["MYSQL_NAME"]}";

var version = new MySqlServerVersion(new Version(8, 0, 28));

builder.Services.AddDbContext<DocReviewDbContext>(options => options
        .UseMySql(connectionString, version)
        .LogTo(message => Debug.WriteLine(message))
);

// Dependency injection.
builder.AddDependencyInjectionClasses();

// Register the GoogleCloudStorage class for DI.
builder.Services.AddTransient<ICloudStorage, GoogleCloudStorage>();

// Add identity.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true; // Require email confirmation.
        options.User.RequireUniqueEmail = false; // User can register per project!
    })
    .AddRoles<IdentityRole>() // Add roles -> must explicitly be added when using .AddDefaultIdentity() instead of .AddIdentity().
    .AddEntityFrameworkStores<DocReviewDbContext>();

// Redis connection string
var redisConnectionString = $"{builder.Configuration["REDIS_SERVER"]}:6379";

// Adding SignalR service
builder.Services.AddSignalR()
    .AddStackExchangeRedis(redisConnectionString, options =>
    {
        options.Configuration.ChannelPrefix = "DocreviewHub";
    });

// Add data protection and store cookies in redis cache to achieve authentication and authorization accros multiple instances
builder.Services
    .AddDataProtection()
    .PersistKeysToStackExchangeRedis(
        ConnectionMultiplexer.Connect(redisConnectionString),
        "DataProtection-Keys"
    );

// Password requirements.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = ApplicationConstants.MinimumPasswordLength;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// NOTE: this is not standard middleware but my own middleware! 
// this adds all the authorization handlers & policies.
builder.AddAuthorizationHandlers();

// Allow Recompilation at runtime. Package: Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add google and facebook authentication.
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["GOOGLE_AUTHENTICATION_CLIENT_ID"];
        options.ClientSecret = builder.Configuration["GOOGLE_AUTHENTICATION_CLIENT_SECRET"];
        options.CallbackPath = "/signin-google";
    }).AddFacebook(options =>
    {
        options.AppId = builder.Configuration["FACEBOOK_AUTHENTICATION_APP_ID"];
        options.AppSecret = builder.Configuration["FACEBOOK_AUTHENTICATION_APP_SECRET"];
    });

// Must be after google and facebook authentication.
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Policies.
builder.Services.AddAuthorization(options =>
{
    // Policies, to check: | whether a user is either an admin or project-manager | if a user is an admin | if a user is a project-manager. | respectively in that order.
    options.AddPolicy(ApplicationConstants.IsModerator, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.ProjectManager.ToString()));
    options.AddPolicy(ApplicationConstants.IsAdmin, policy => policy.RequireRole(UserRole.Admin.ToString()));
    options.AddPolicy(ApplicationConstants.IsProjectManager, policy => policy.RequireRole(UserRole.ProjectManager.ToString()));
});

var app = builder.Build();

// Add forwarding headers for reverse proxy.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Home/Error");
    app.UseHsts();
}

// Custom 404 redirect middleware.
// app.CheckNotFound(); // Must be before app.CheckProjectValidity(). (And also before .UseRouting() but this I don't know for sure).

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



// This middleware checks if the project name in the url is valid.
app.CheckProjectValidity(); // Must be between UseRouting and UseEndPoints. And after UseAuthentication() and UseAuthorization().
app.CheckProjectVisibleForUser(); // Same as for .CheckProjectValidity AND (preferably) after .CheckProjectValidity()!

app.MapHub<DocreviewHub>("/DocreviewHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{" + ApplicationConstants.Project.ToLower() + "}/{controller}/{action}/{id?}");

app.Run();