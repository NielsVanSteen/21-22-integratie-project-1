using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller manages the page where a <see cref="UserRole.ProjectManager"/> or <see cref="UserRole.Admin"/> can edit the registration information a user has to enter.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class RegistrationController : Controller
{
    // Fields.
    private readonly IUserManager _userService;
    private readonly IUserPropertyManager _userPropertyService;
    private readonly IProjectManager _projectService;
    
    // Constructor.
    public RegistrationController(IUserManager userService, IUserPropertyManager userPropertyService ,IProjectManager projectService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userService = userService;
        _projectService = projectService;
        _userPropertyService = userPropertyService;
    }
    
    // Methods
    public IActionResult Index()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        var userPropertyNames = _userPropertyService.GetUserPropertyNamesByProject(project);
        
        return View(userPropertyNames);
    } // Index.
}