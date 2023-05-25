using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UI.MVC.Identity;
using UI.MVC.Models.ProjectSetting;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller contains the Project Settings, The project Styling, and the project footer information (Accessibility & Privacy Statement).
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectSettingController : Controller
{
    private readonly UserManager<User> _userManager;

    private readonly IProjectManager _projectManager;

    // Constructor.
    public ProjectSettingController(IUserManager userService, UserManager<User> userManager,
        SignInManager<User> signInManager, IProjectManager projectManager)
    {
        _userManager = userManager;
        _projectManager = projectManager;
    }

    /// <summary>
    /// Page for changing Project settings 
    /// </summary>
    /// <returns></returns>
    // GET
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Index()
    {
        // Get current User
        var user = await _userManager.GetUserAsync(User);
        ViewBag.User = user;

        //Get current project
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, true, true);
        ViewBag.Project = project;
        return View();
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Delete project
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Index(ConfirmStringModel confirmStringModel)
    {
        //Get project from routedata
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, true, true);

        if (confirmStringModel.ConfirmString != null && confirmStringModel.ConfirmString.Equals("Confirm"))
        {
            _projectManager.RemoveProject(project);
            return RedirectToAction("index", "ProjectModeration", new {Project = ApplicationConstants.BackEndUrlName}, null);
        }
        ModelState.AddModelError(nameof(ConfirmStringModel.ConfirmString), "The provided text didn't match the Confirm text");
        
        // Get current User
        var user = await _userManager.GetUserAsync(User);
        ViewBag.User = user;
        ViewBag.Project = project;
        return View(confirmStringModel);
    }
}