using BL.Project;
using BL.ProjectStatistics;
using Domain.Comment;
using Domain.DocReview;
using Domain.ProjectStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using UI.MVC.Identity;
using UI.MVC.Models.Android;
using UI.MVC.Models.ProjectStatistics;

namespace UI.MVC.Controllers.Api;

/// <summary>
/// The web api controller for the project statistics.
/// </summary>
[ApiController]
[Authorize(Policy = ApplicationConstants.IsModerator)]
[Route("/api/{project}/[controller]")]
public class ProjectStatisticsController : ControllerBase
{
    // Fields.
    private readonly IProjectStatisticsManager _projectStatisticsManager;
    private readonly IProjectManager _projectManager;

    // Constructor.
    public ProjectStatisticsController(IProjectStatisticsManager projectStatisticsManager,
        IProjectManager projectManager)
    {
        _projectStatisticsManager = projectStatisticsManager;
        _projectManager = projectManager;
    } // ProjectStatisticsController.

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns an overview of all the project statistics.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Overview")]
    public IActionResult GetProjectStatisticsOverview([FromQuery]StatisticsFilterModel statisticsFilterModel)
    {
        // Get the project.
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        
        // Validation.
        if (statisticsFilterModel.EndDate > DateTime.Now)
            statisticsFilterModel.EndDate = DateTime.Now;
        
        // Get the project statistics.
        List<ProjectStatistics> projectStatistics;
        if (statisticsFilterModel.UseBeginDate || statisticsFilterModel.UseEndDate)
        {
            var beginDate = statisticsFilterModel.BeginDate ?? DateTime.Today.AddMonths(-3);
            var endDate = statisticsFilterModel.EndDate ?? DateTime.Today;
             projectStatistics = _projectStatisticsManager.GetProjectStatisticsByProjectAndTimeFrame(project, statisticsFilterModel.Detail, beginDate, endDate).ToList();   
        }
        else
        {
             projectStatistics = _projectStatisticsManager.GetProjectStatisticsByProject(project, statisticsFilterModel.Detail).ToList();   
        }

        // Check if the statistics are present.
        if (!projectStatistics.Any())
            return NotFound("No statistics found!");

        // Create dto.
        var projectStatisticsDto = new List<ProjectStatisticsDto>();
        projectStatistics.ForEach(s => projectStatisticsDto.Add(new ProjectStatisticsDto(s)));

        // Check to add a new statistics record.
        AddStatisticsRecord();
        
        return Ok(projectStatisticsDto);
    } // GetProjectStatisticsOverview.

    /// <author> Bjorn Straetemans </author>
    /// <summary>
    /// Returns an overview of the last project statistics.
    /// </summary>
    /// <param name="id">
    /// The id of the project.
    /// </param>
    /// <returns></returns>
    [HttpGet("GetProjectStatisticsAndroid/{id:Int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult GetProjectStatisticsAndroid(int id)
    {
        var project = _projectManager.GetProjectById(id);
        var projectStatistics = _projectStatisticsManager.GetLastProjectStatisticByProject(project);
        var surveyStatistics = _projectStatisticsManager.GetSurveyStatisticsByProject(project);

        if (projectStatistics == null)
        {
            return NotFound("No statistics found!");
        }

        var projectStatisticsDto = new StatisticAndroidDto(projectStatistics);

        return Ok(projectStatisticsDto);
    } // GetProjectStatisticsOverview.
    
    [HttpPost("AddStatisticsRecord")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult AddStatisticsRecord()
    {
        // https://stackoverflow.com/questions/58535975/cron-expression-run-task-once-per-day-in-asp-net-core.
        
        var projects = _projectManager.GetProjects().ToList();
        
        foreach(var project in projects)
        {
            // Check if the current date already has a record.
            if (_projectStatisticsManager.GetProjectStatisticsByProjectAndDay(project, DateTime.Today) != null)
                continue;
            
            // Create the statistics.
            var statistics = new ProjectStatistics
            {
                LastUpdated = DateTime.Today,
                ProjectId = project.ProjectId,
                ReactionGroupAmount = _projectStatisticsManager.GetReactionGroupAmountByProject(project),
                EmojiAmount = _projectStatisticsManager.GetEmojiAmountByProject(project),
                UsersAmount = _projectStatisticsManager.GetUsersAmountByProject(project, project.ExternalName),
                ManagersAmount = _projectStatisticsManager.GetProjectManagersAmountByProject(project, ApplicationConstants.BackEndUrlName),
                DocReviewsAmount = _projectStatisticsManager.GetDocReviewsAmountByProject(project),
                EmojiTypeAmount = _projectStatisticsManager.GetEmojiTypesAmountByProject(project).ToList(),
                CommentStatusTypeAmount = _projectStatisticsManager.GetCommentsStatusAmountByProject(project).ToList(),
                DocReviewStatusTypeAmount = _projectStatisticsManager.GetDocReviewsStatusAmountByProject(project).ToList()
            };
            
            // Add the statistics.
            _projectStatisticsManager.AddProjectStatistics(statistics);
            
        } // Foreach.
        
        return CreatedAtAction("AddStatisticsRecord", "ProjectStatistics", null, null);
    } // AddStatisticsRecord.
}