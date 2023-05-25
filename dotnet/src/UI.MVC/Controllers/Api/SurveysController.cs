using BL.Project;
using BL.ProjectStatistics;
using Domain.ProjectStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.Survey;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Authorize(Policy = ApplicationConstants.IsModerator)]
[Route("/api/{project}/[controller]")]
public class SurveysController : ControllerBase
{
    // Fields.
    private readonly IProjectStatisticsManager _projectStatisticsManager;
    private readonly IProjectManager _projectManager;

    // Constructor.
    public SurveysController(IProjectStatisticsManager projectStatisticsManager, IProjectManager projectManager)
    {
        _projectStatisticsManager = projectStatisticsManager;
        _projectManager = projectManager;
    } // ProjectStatisticsController.
    
    // Methods.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns a list with all the survey statistics for a specific project.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetStatistics")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult GetStatistics()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        
        if (project == null)
        {
            return NotFound();
        } // if
        
        // Get the statistics from the database.
        var statistics = _projectStatisticsManager.GetSurveyStatisticsByProject(project);
        
        // Map statistics to view model.
        var result = statistics.Select(o => new SurveyStatisticsModel(o));
        
        return Ok(result);
    } // GetStatistics.
}