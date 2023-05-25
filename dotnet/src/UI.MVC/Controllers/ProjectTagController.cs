using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <author>Niels Van Steen</author>
/// <summary>
/// This controller manages the project tags (create, edit, delete).
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectTagController : Controller
{
    // Fields.
    private readonly IProjectTagManager _projectTagService;
    private readonly IProjectManager _projectService;
    
    // Constructor.
    public ProjectTagController(IProjectTagManager projectTagService, IProjectManager projectService)
    {
        _projectTagService = projectTagService;
        _projectService = projectService;
    } // ProjectTagController.
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the view that shows an overview of all the tags belonging to the project.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);
        
        var tags = _projectTagService.GetProjectTagsByProject(project);
        
        return View(tags);
    } // Index.
}