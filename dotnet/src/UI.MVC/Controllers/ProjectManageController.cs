using BL.DocReview;
using BL.Project;
using BL.ProjectStatistics;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.ProjectManage;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller shows the backend project page. Only visible for user whom have one the following roles: <see cref="UserRole.Admin"/> or <see cref="UserRole.ProjectManager"/>
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectManageController : Controller
{
    // Fields.
    private readonly IProjectManager _projectService;
    private readonly IUserManager _userService;
    private readonly ICloudStorage _cloudStorage;
    private readonly IDocReviewManager _docReviewManager;
    private readonly IProjectStatisticsManager _projectStatisticsService;

    // Constructor.
    public ProjectManageController(IUserManager userService, IProjectManager projectService,
        IProjectStatisticsManager projectStatisticsService, ICloudStorage cloudStorage, IDocReviewManager docReviewManager)
    {
        _projectService = projectService;
        _userService = userService;
        _projectStatisticsService = projectStatisticsService;
        _cloudStorage = cloudStorage;
        _docReviewManager = docReviewManager;
    } // ProjectManageController.

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The project page for <see cref="UserRole.ProjectManager"/>.
    ///
    /// The project id is not a parameter since the <see cref="Project.ExternalName"/> is in the url. 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        ViewBag.Project = project;
        ViewBag.DocReviews = _docReviewManager.GetDocReviewsByProject(project.ProjectId, false, false, true);
        return View(new EditProjectModel
        {
            ProjectTitle = project.ProjectTitle,
            Introduction = project.Introduction
        });
    } // Index.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Is executed when the user clicks the save changes button.
    /// </summary>
    /// <param name="editProjectModel"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Index(EditProjectModel editProjectModel)
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        // Validation.
        if (!ModelState.IsValid)
        {
            ViewBag.Project = project;
            ViewBag.DocReviews = _docReviewManager.GetDocReviewsByProject(project.ProjectId, false, false, true);
            return View(editProjectModel);
        }

        // Upload the images.
        if (editProjectModel.ProjectLogo != null)
            await _cloudStorage.UploadFileAsync(editProjectModel.ProjectLogo, project.GetProjectLogoName(),
                new CachingTime(CachingTime.CacheDefaults.Long));
        if (editProjectModel.ProjectBannerImage != null)
            await _cloudStorage.UploadFileAsync(editProjectModel.ProjectBannerImage, project.GetProjectBannerImageName(),
                new CachingTime(CachingTime.CacheDefaults.Long));

        // Update the project.
        project.ProjectTitle = editProjectModel.ProjectTitle;
        project.Introduction = editProjectModel.Introduction;
        _projectService.ChangeProject(project);

        return RedirectToAction("Index");
    } // Index.
}