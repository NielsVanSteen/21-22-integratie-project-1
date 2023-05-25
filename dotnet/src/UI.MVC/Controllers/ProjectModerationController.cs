using System.ComponentModel.DataAnnotations;
using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Google.Apis.Util;
using identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySqlConnector;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.Hub;
using UI.MVC.Models.ProjectModeration;
using UI.MVC.Models.Shared;

namespace UI.MVC.Controllers;

/// <summary>
/// This controller shows the overview page where <see cref="UserRole.Admin"/> can view all the <see cref="Domain.Project.Project"/>s + create new projects.
/// And <see cref="UserRole.ProjectManager"/>s can browse the projects they're assigned to.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectModerationController : Controller
{
    // Fields.
    private readonly IProjectManager _projectService;
    private readonly IUserManager _userService;
    private readonly IThemeStylesManager _themeStylesManager;
    private readonly UserManager<User> _userManager;
    private readonly IMarkedEmailManager _markedEmailService;
    private readonly ICloudStorage _cloudStorage;
    private readonly IHubContext<DocreviewHub> _hubContext; 
    

    // Constructor.
    public ProjectModerationController(UserManager<User> userManager, IThemeStylesManager themeStylesManager,
        IProjectManager projectService, IUserManager userService, ICloudStorage cloudStorage, IMarkedEmailManager markedEmailService, IHubContext<DocreviewHub> hubContext)
    {
        _userManager = userManager;
        _themeStylesManager = themeStylesManager;
        _projectService = projectService;
        _userService = userService;
        _markedEmailService = markedEmailService;
        _cloudStorage = cloudStorage;
        _hubContext = hubContext;
    } // ProjectModerationController.

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns an overview of all the <see cref="Domain.Project.Project"/>.
    /// Users with the role <see cref="UserRole.Admin"/> can create & edit <see cref="Domain.Project.Project"/>. + view all <see cref="Domain.Project.Project"/>.
    /// Users with the role <see cref="UserRole.ProjectManager"/> can only view the <see cref="Domain.Project.Project"/> they're assigned to.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Project> projects;
        var user = await _userManager.GetUserAsync(User);

        // If user is an admin: get all projects. else (project-manager) get only the projects the manager is assigned to.
        if (await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
            projects = _projectService.GetProjects();
        else
            projects = _userService.GetUser(user.Id, true).RegisteredForProjects;

        return View(projects);
    } // Index.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Displays the view to create a new project.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    [HttpGet]
    public IActionResult CreateProject()
    {
        return View(new CreateProjectModel());
    } // CreateProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Allows an admin to create a new project. with the most basic information.
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectModel createProjectModel)
    {
        // The project name can't be equal to the backEndUrlName.
        var list = new List<string>() { createProjectModel.ExternalName, createProjectModel.InternalName, createProjectModel.ProjectTitle };
        if (list.Contains(ApplicationConstants.BackEndUrlName))
            ModelState.AddModelError(string.Empty, $"The external name, internal name or title can not be {ApplicationConstants.BackEndUrlName}!");

        if (!ModelState.IsValid)
            return View(createProjectModel);

        if (_projectService.GetProjectByExternalName(createProjectModel.ExternalName) != null)
        {
            ModelState.AddModelError("ExternalName", "This external name is already in use!");
            return View(createProjectModel);
        }

        // Create the project object.
        var project = new Project
        {
            ProjectTitle = createProjectModel.ProjectTitle,
            InternalName = createProjectModel.InternalName,
            ExternalName = createProjectModel.ExternalName,
            ProjectStyling = new ProjectStyling()
            {
                ThemeStylesId = _themeStylesManager.GetThemeStyles(ThemeStyles.DefaultProjectTheme).ThemeStylesId
            }
        };

        // Create Project History
        var user = await _userManager.GetUserAsync(User);
        project.ProjectHistories.Add(new ProjectHistory
        {
            EditedBy = user,
            EditedById = user.Id,
            ProjectStatus = ProjectStatus.Created,
            EditedOn = DateTime.Now,
        });

        // Add the project.
        try
        {
            _projectService.AddProject(project);
        }
        catch (Exception)
        {
            // Whe know the internal name is not unique since we've already checked for the external name.
            ModelState.AddModelError("InternalName", "This internal name is already in use!");
            return View(createProjectModel);
        }

        // Upload the images.
        await _cloudStorage.UploadFileAsync(createProjectModel.ProjectLogo, project.GetProjectLogoName(),
            new CachingTime(CachingTime.CacheDefaults.Default));
        await _cloudStorage.UploadFileAsync(createProjectModel.ProjectBannerImage, project.GetProjectBannerImageName(),
            new CachingTime(CachingTime.CacheDefaults.Default));

        return RedirectToAction("Index");
    } // CreateProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the view showing all the <see cref="UserRole.ProjectManager"/> and <see cref="UserRole.Admin"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult ViewModerators()
    {
        // Get all the users with an admin or project manager role.
        var moderators = _userService.GetUsersWithHigherRoleThanNormalUser(ApplicationConstants.BackEndUrlName, true);
        var markedEmails = _markedEmailService.GetMarkedEmails(true);

        return View(new ViewModeratorsModel
        {
            Users = moderators,
            MarkedEmails = markedEmails
        });
    } // ViewModerators.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The view to create a moderator (marked email).
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult CreateModerator()
    {
        return View(new CreateModeratorModel());
    } // CreateModerator.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a new marked-email entry.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult CreateModerator(CreateModeratorModel createModeratorModel)
    {
        // Validation.
        try
        {
            Validator.ValidateObject(createModeratorModel, new ValidationContext(createModeratorModel), validateAllProperties: true);
        }
        catch (ValidationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return View(createModeratorModel);
        }

        // Add the project id's to the project list. So when validation fails assigned projects won't be deleted in the view.
        var projects = new List<Project>();
        foreach (var id in createModeratorModel.AssignedProjectIds ?? Enumerable.Empty<int>())
            projects.Add(_projectService.GetProjectById(id));
        createModeratorModel.Projects = projects;

        if (!ModelState.IsValid)
            return View(createModeratorModel);

        // Check if the marked email doesn't exist already.
        if (_markedEmailService.GetMarkedEmailByEmail(createModeratorModel.Email) != null)
        {
            ModelState.AddModelError("Email", $"This Email is already assigned to a marked email!");
            return View(createModeratorModel);
        }

        if (_userService.GetUserByUserName(createModeratorModel.Email + "_" + ApplicationConstants.BackEndUrlName) != null)
        {
            ModelState.AddModelError("Email", $"This Email is already assigned to an {UserRole.Admin.ToString()} or {UserRole.ProjectManager.ToString()}!");
            return View(createModeratorModel);
        }

        // Convert the project Ids to actual projects.
        var assignedProjects = new List<Project>();
        foreach (var id in createModeratorModel.AssignedProjectIds ?? Enumerable.Empty<int>())
            assignedProjects.Add(_projectService.GetProjectById(id));

        // Create & add the marked-email.
        var markedEmail = new MarkedEmail
        {
            Email = createModeratorModel.Email,
            UserRole = createModeratorModel.UserRole,
            Projects = assignedProjects
        };

        _markedEmailService.AddMarkedEmail(markedEmail);

        return RedirectToAction("ViewModerators");
    } // CreateModerator.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Displays the view to edit a moderator.
    /// </summary>
    /// <param name="id">The id of the moderator to edit.</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public async Task<IActionResult> EditModerator(string id)
    {
        ICollection<Project> projects;
        bool isAdmin;

        // Check if the id belongs to a marked-email or a user. Then get the object, get the list of projects and whether it's an admin or not.
        if (int.TryParse(id, out int tmp))
        {
            var markedEmail = _markedEmailService.GetMarkedEmail(tmp, true);
            if (markedEmail == null) return View("ViewModerators");
            isAdmin = markedEmail.UserRole == UserRole.Admin;
            projects = markedEmail.Projects;
        }
        else
        {
            var user = _userService.GetUser(id, true);
            if (user == null) return View("ViewModerators");
            isAdmin = await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString());
            projects = user.RegisteredForProjects;
        }

        // Check if the user/marked email is not an admin.
        var messageModel = new ConfirmStatusMessageModel();
        if (isAdmin)
        {
            messageModel.Title = "Not allowed!";
            messageModel.Description = $"You can only edit user with the role {UserRole.ProjectManager}";
            messageModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
            ViewBag.AccountConfirmation = messageModel;
            return RedirectToAction("ViewModerators");
        }

        return View(new EditModeratorModel { Projects = projects });
    } // EditModerator.

    // <author>Niels Van Steen</author>
    /// <summary>
    /// Displays the view to edit a moderator.
    /// </summary>
    /// <param name="editModeratorModel"><see cref="EditModeratorModel"/>.</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult EditModerator(string id, EditModeratorModel editModeratorModel)
    {
        // Add the project id's to the project list. So when validation fails assigned projects won't be deleted in the view.
        var projects = new List<Project>();
        foreach (var projectId in editModeratorModel.AssignedProjectIds ?? Enumerable.Empty<int>())
            projects.Add(_projectService.GetProjectById(projectId));
        editModeratorModel.Projects = projects;

        // Check if the project manager has at least 1 assigned project.
        var messageModel = new ConfirmStatusMessageModel();
        if (!editModeratorModel.Projects.Any())
        {
            messageModel.Title = "No projects!";
            messageModel.Description = $"A {UserRole.ProjectManager} needs to be assigned to at least 1 project.";
            messageModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
            ViewBag.AccountConfirmation = messageModel;
            return View(editModeratorModel);
        }

        if (!ModelState.IsValid)
            return View(editModeratorModel);

        // Update the user.

        if (int.TryParse(id, out int tmp))
        {
            var markedEmail = _markedEmailService.GetMarkedEmail(tmp); 
            markedEmail.Projects = editModeratorModel.Projects.ToList();
            _markedEmailService.ChangeMarkedEmail(markedEmail);
        }
        else
        {
            var user = _userService.GetUser(id, includeProjects:true);
            user.ChangeRegisteredProjects(editModeratorModel.Projects.ToList(), _hubContext);
            _userService.ChangeUserAssignedProjects(user);
        }

        return RedirectToAction("ViewModerators");
    } // EditModerator.
}