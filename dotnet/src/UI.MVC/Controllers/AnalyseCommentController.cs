using BL.Comment;
using BL.Project;
using Domain.Comment;
using Domain.User;
using Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models;
using UI.MVC.Models.AnalyseComments;
using UI.MVC.Models.Shared;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller manages the pages where a <see cref="UserRole.ProjectManager"/> or <see cref="UserRole.Admin"/> can analyse and moderate the comments.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class AnalyseCommentController : Controller
{
    private readonly ICommentManager _commentManager;
    private readonly IProjectTagManager _projectTagManager;
    private readonly IProjectManager _projectService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    // Constructor.
    /// <author> Sander Verheyen</author>
    public AnalyseCommentController(UserManager<User> userManager, IProjectTagManager projectTagManager,
        IAuthorizationService authorizationService,
        ICommentManager commentManager, IProjectManager projectService)
    {
        _userManager = userManager;
        _projectTagManager = projectTagManager;
        _authorizationService = authorizationService;
        _commentManager = commentManager;
        _projectService = projectService;
    }

    // Methods.

    /// <author> Sander Verheyen</author>
    /// <summary>
    /// Gets all <see cref="Reaction"/> of a project.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index()
    {
        // Get name of project
        string name = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);

        var pageSize = 5;
        var pageNumber = 1;

        // Get all the comments of this doc-review with all relevant information
        var filterModel = new CommentsFilterModel()
        {
            SortOn = SortOn.Date,
            SortOrder = SortOrder.Ascending,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
        ViewBag.Comments = _commentManager.GetCommentsOfProject(project.ProjectId, filterModel, true, true, true, true, true, true);
        
        int totalItems = _commentManager.GetCommentTotalByProject(project);
        var totalPages = (int) Math.Ceiling(totalItems / (double) pageSize);
        ViewBag.PaginationModel = new PaginationNavigationModel(1, totalPages, pageSize, totalItems);
        ViewBag.HasFilterChanged = false;
        return View(new AnalyseCommentsFilterModel()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            HasFilterChanged = false,
        });
    } // Index.

    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Index(AnalyseCommentsFilterModel commentsFilterModel)
    {
        // Get name of project
        string name = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);

        if (commentsFilterModel.HasFilterChanged)
            commentsFilterModel.PageNumber = 1;
        commentsFilterModel.HasFilterChanged = false;
        
        if (commentsFilterModel.PageSize < 1)   
            commentsFilterModel.PageSize = 1;
        
        if (commentsFilterModel.PageNumber < 1)
            commentsFilterModel.PageNumber = 1;
            

        var filterModel = commentsFilterModel.ToCommentsFilterModel();
        ViewBag.Comments = _commentManager.GetCommentsOfProject(project.ProjectId, filterModel, true, true, true, true, true, true);

        int totalItems = _commentManager.GetCommentTotalByProject(project, filterModel);
        var totalPages = (int) Math.Ceiling(totalItems / (double) (filterModel.PageSize ?? 1));
        ViewBag.PaginationModel = new PaginationNavigationModel(commentsFilterModel.PageNumber, totalPages, commentsFilterModel.PageSize, totalItems);
        ViewBag.HasFilterChanged = false;
        
        return View(commentsFilterModel);
    } // Index.
}