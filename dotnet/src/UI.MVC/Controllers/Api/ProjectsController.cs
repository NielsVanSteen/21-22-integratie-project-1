using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.Dto;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    // Fields.
    private readonly IProjectManager _projectService;
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    // Constructor.
    public ProjectsController(UserManager<User> userManager, IAuthorizationService authorizationService,
        IProjectManager projectService, IUserManager userService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
        _projectService = projectService;
        _userService = userService;
    } // ProjectModerationController.

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get a single project given the id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var project = _projectService.GetProjectById(id);
        
        if (project == null)
            return NoContent();
        
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, project, ApplicationConstants.CanViewProjectAuthorization);
        if (!authorizationResult.Succeeded)
            return NoContent();

        var projectDto = new ProjectDto(project);

        return Ok(projectDto);
    } // GetProjectByFilter.
    
}