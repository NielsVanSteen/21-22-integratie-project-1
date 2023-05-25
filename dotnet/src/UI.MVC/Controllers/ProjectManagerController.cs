using BL.Comment;
using BL.DocReview;
using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.CloudStorage;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <author>Niels Van Steen</author>
/// <summary>
/// Controller that shows the activity of project managers per project.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectManagerController : Controller
{
    // Fields.
    private readonly IProjectManager _projectService;
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly IProjectHistoryManager _projectHistoryManager;
    private readonly ICommentHistoryManager _commentHistoryManager;
    private readonly IDocReviewHistoryManager _docReviewHistoryManager;
    
    // Constructor.
    public ProjectManagerController(UserManager<User> userManager, IProjectManager projectService, IUserManager userService,
        IProjectHistoryManager projectHistoryManager, IDocReviewHistoryManager docReviewHistoryManager, ICommentHistoryManager commentHistoryManager)
    {
        _userManager = userManager;
        _projectService = projectService;
        _userService = userService;
        _projectHistoryManager = projectHistoryManager;
        _commentHistoryManager = commentHistoryManager;
        _docReviewHistoryManager = docReviewHistoryManager;
    } // ProjectModerationController.

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the view showing all project managers of a project.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        var projectManagers = _userService.GetProjectManagersByProject(project, ApplicationConstants.BackEndUrlName);
        return View(projectManagers);
    } // Index.

     /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the view showing the project-wide activity.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult AllActivity()
    {
        return View();
    } // AllActivity.
    
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Detail(string id)
    {
        var user = _userService.GetUser(id, true);
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        // Check if the project manager is assigned to the current project.
        if (!user.RegisteredForProjects.Contains(project))
            return RedirectToAction("Index");
        
        return View(user);
    } // Detail.
}