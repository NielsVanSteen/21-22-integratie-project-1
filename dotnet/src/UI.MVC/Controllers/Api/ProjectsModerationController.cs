using System.Collections;
using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.Android;
using UI.MVC.Models.Dto;
using UI.MVC.Models.ProjectModeration;

namespace UI.MVC.Controllers.Api;

/// <summary>
/// The counterpart controller of <see cref="ProjectModerationController"/> for Web API.
/// </summary>
[ApiController]
[Authorize(Policy = ApplicationConstants.IsModerator)]
[Route("/api/{project}/[controller]")]
public class ProjectsModerationController : ControllerBase
{
    // Fields.
    private readonly IProjectManager _projectService;
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly IMarkedEmailManager _markedEmailManager;
    private readonly IAuthorizationService _authorizationService;
    
    // Constructor.
    public ProjectsModerationController(UserManager<User> userManager, IAuthorizationService authorizationService, IMarkedEmailManager markedEmailManager,IProjectManager projectService, IUserManager userService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
        _markedEmailManager = markedEmailManager;
        _projectService = projectService;
        _userService = userService;
    } // ProjectModerationController.
    
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get all the projects given the filter criteria.
    /// </summary>
    /// <param name="filterProjectsModel"></param>
    /// <returns></returns>
    [HttpGet("GetProjectByFilter")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> GetProjectByFilter([FromQuery]FilterProjectsModel filterProjectsModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        // Admin will view all the projects thus make user null.
        if (await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
            user = null;
        
        // Return all the projects.
        var projects = _projectService.GetProjectByFilters(filterProjectsModel.Name, filterProjectsModel.MapSortOrder(), user);

        ICollection<ProjectDto> projectsDtos = new List<ProjectDto>();

        // Convert to DTO variant.
        foreach (var project in projects)
            projectsDtos.Add(new ProjectDto(project));

        return Ok(projectsDtos);
    } // GetProjectByFilter.
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Check if the email and password are correct and if the user role is <see cref="UserRole.ProjectManager"/> or <see cref="UserRole.Admin"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetProjectsByUser")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> GetProjectsByUser()
    {
        var user = await _userManager.GetUserAsync(User);

        if (await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
            user = null;

        var projects = _projectService.GetProjectByFilters(null, SortOrder.Ascending, user, true);
        var projectsDto = new List<ProjectAndroidDto>();

        if (projects == null || !projects.Any())
        {
            return NoContent();
        }
        
        foreach (var project in projects)
        {
            projectsDto.Add(new ProjectAndroidDto(project));
        }
        
        return Ok(projectsDto);
    }//GetProjectByUser
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Deletes a moderator.
    /// </summary>
    /// <param name="id">The id of the moderator that should be deleted.</param>
    /// <returns></returns>
    [HttpDelete("DeleteModerator/{id}")]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult DeleteModerator(string id)
    {
        if (id == Domain.User.User.RootUserId)
            return Forbid("Root user can't be deleted!");
        
        var result = _userService.RemoveUser(id);

        if (result)
            return NoContent();

        return NotFound();
    } // Delete.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Deletes a marked email.
    /// </summary>
    /// <param name="id">The id of the moderator that should be deleted.</param>
    /// <returns></returns>
    [HttpDelete("DeleteMarkedEmail/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsAdmin)]
    public IActionResult DeleteMarkedEmail(int id)
    {
        var result = _markedEmailManager.RemoveMarkedEmail(id);

        if (result)
            return NoContent();

        return NotFound();
    } // Delete.
}