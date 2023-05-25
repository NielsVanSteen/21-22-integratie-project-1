using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.TimeLine;

namespace UI.MVC.Controllers;

[Authorize(Policy = ApplicationConstants.IsModerator)]
public class TimeLineController : Controller
{
    // Methods.
    private readonly IProjectManager _projectManager;
    private readonly ITimeLineManager _timeLineManager;


    // Constructor.
    public TimeLineController(ITimeLineManager timeLineManager, IProjectManager projectManager)
    {
        _projectManager = projectManager;
        _timeLineManager = timeLineManager;
    } // TimeLineController.

    // Methods.

    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        return View();
    } // Index.

    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index(TimeLinePhaseModel phaseModel)
    {
        if (!ModelState.IsValid)
            return View(phaseModel);
        
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        var timeLine = _timeLineManager.GetTimeLineByProject(project);

        var phase = new TimeLinePhase
        {
            TimeLineId = timeLine.TimeLineId,
            Name = phaseModel.Name,
            Description = phaseModel.Description,
            DocReviewId = phaseModel.DocReviewId < 0 ? null : phaseModel.DocReviewId ?? 0,
            BeginDate = DateOnly.FromDateTime((DateTime) phaseModel.BeginDate)
        };

        _timeLineManager.AddTimeLinePhase(phase);
        return RedirectToAction("Index");
    } // Create.

    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Edit(TimeLinePhaseModel phaseModel)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", phaseModel);
        
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        var timeLine = _timeLineManager.GetTimeLineByProject(project);

        var phase = new TimeLinePhase
        {
            TimeLineId = timeLine.TimeLineId,
            Name = phaseModel.Name,
            Description = phaseModel.Description,
            DocReviewId = phaseModel.DocReviewId < 0 ? null : phaseModel.DocReviewId ?? 0,
            BeginDate = DateOnly.FromDateTime((DateTime) phaseModel.BeginDate)
        };
        
        // Change an existing phase.
        phase.TimeLinePhaseId = phaseModel.TimeLinePhaseId ?? 0;
        _timeLineManager.ChangeTimeLinePhase(phase);
        
         return RedirectToAction("Index", phaseModel);
    } // Edit.
}