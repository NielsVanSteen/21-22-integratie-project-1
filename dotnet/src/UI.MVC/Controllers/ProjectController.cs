using BL.Comment;
using BL.DocReview;
using BL.Project;
using BL.User;
using Domain.Comment;
using Domain.DocReview;
using Domain.User;
using Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.DocReview;
using UI.MVC.Models.Project;

namespace UI.MVC.Controllers;

/// <summary>
/// Project controller visible containing the views accessible for all the users.
/// </summary>
public class ProjectController : Controller
{
    private readonly IProjectManager _projectManager;
    private readonly IDocReviewManager _docReviewManager;
    private readonly ICommentManager _commentManager;
    private readonly UserManager<User> _userManager;

    private IAuthorizationService _authorizationService;

    // Constructor.
    public ProjectController(IUserManager userService, IProjectManager projectService, UserManager<User> userManager,
        SignInManager<User> signInManager, IProjectManager projectManager,
        IDocReviewManager docReviewManager, ICommentManager commentManager, IAuthorizationService authorizationService)
    {
        _projectManager = projectManager;
        _docReviewManager = docReviewManager;
        _commentManager = commentManager;
        _userManager = userManager;
        _authorizationService = authorizationService;
    }

    // Methods.
    public IActionResult Index()
    {
        //Get current project
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, true);

        //Get all doc-reviews of current project
        var docReviews = _docReviewManager.GetDocReviewByProjectAndStatus(project, DocReviewStatus.Published, includeWrittenBy: true);

        var model = new ProjectModel
        {
            Project = project,
            DocReviews = docReviews
        };

        return View(model);
    } // Index.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Page to read a <see cref="DocReview"/>
    /// </summary>
    /// <param name="id">Id of current doc-review</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> DocReview(int id)
    {
        //Get doc-review image
        var docReview = _docReviewManager.GetDocReview(id, includeWrittenBy:true, includeHistory:true, includeSurveys:true);
        //If an invalid id is given return to index page of the current project
        if (docReview is null)
        {
            return RedirectToAction("Index", "Project");
        }
        
        // Check if user is normal user and doc-review is published.
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, docReview, ApplicationConstants.CanViewDocReviewAuthorization);
        if (!authorizationResult.Succeeded)
            return RedirectToAction("Index", "Project");
        
        // Check if the user is required to be authenticated.
        var isLoginRequired = await _authorizationService.AuthorizeAsync(User, docReview, ApplicationConstants.IsLoginRequiredAuthorization);
        if (!isLoginRequired.Succeeded)
            return RedirectToAction("Login", "Account");
        
        //Return View
        var pageSize = 5;
        var pageNumber = 1;

        // Get all the comments of this doc-review with all relevant information
        var filterModel = new UserCommentsFilter()
        {
            DocReviewId = docReview.DocReviewId,
            OwnComments = false,
            CloseComments = true,
            UserId = "",
            SortOn = SortOn.Date,
            SortOrder = SortOrder.Ascending,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
        var comments = _commentManager.GetCommentsByDocReview(id, filterModel, new List<CommentStatus>() {CommentStatus.Published}, 
            new List<CommentStatus>() {CommentStatus.Removed}, true, true, true, true, true, true).ToList();
        ViewBag.Comments = comments;
        ViewBag.UserCommentFilter = filterModel;
        return View(docReview);
    } // Detail.
    
    [HttpPost]
    public async Task<IActionResult> DocReview(int id, UserCommentsFilterModel filter)
    {
        var user = await _userManager.GetUserAsync(User);
        var docReviewId = id;
        var docReview = _docReviewManager.GetDocReview(docReviewId, includeWrittenBy:true, includeHistory:true, includeSurveys:true);
        var filterModel = new UserCommentsFilter()
        {
            DocReviewId = docReviewId,
            OwnComments = filter.OwnComments,
            CloseComments = false,
            UserId = user.Id,
            SortOn = filter.SortOn,
            SortOrder = filter.SortOrder,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
        };
        var comments = _commentManager.GetCommentsByDocReview(docReviewId, filterModel, new List<CommentStatus>() {CommentStatus.Published}, 
            new List<CommentStatus>() {CommentStatus.Removed}, true, true, true, true, true, true).ToList();
        ViewBag.Comments = comments;
        ViewBag.UserCommentFilter = filterModel;
        return View("DocReview", docReview);
    }
}