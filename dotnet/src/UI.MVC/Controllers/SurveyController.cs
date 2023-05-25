using BL.Project;
using BL.ProjectStatistics;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller manages the page where a <see cref="ProjectManager"/> or <see cref="UserRole.Admin"/> can edit the registration information a user has to enter.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class SurveyController : Controller
{
    // Fields.
    private readonly IProjectStatisticsManager _projectStatisticsManager;
    private readonly IProjectManager _projectManager;
    
    // Constructor.
    public SurveyController(IProjectStatisticsManager projectStatisticsManager, IProjectManager projectManager)
    {
        _projectStatisticsManager = projectStatisticsManager;
        _projectManager = projectManager;
    }

    // Methods.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Return the view displaying all the survey results.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);

        ViewBag.Project = project;
        return View();
    } // Index.
}