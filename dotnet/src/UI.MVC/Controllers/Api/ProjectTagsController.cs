using System.Net;
using BL.Project;
using BL.User;
using Domain.DocReview;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using UI.MVC.Identity;
using UI.MVC.Models.Dto;

namespace UI.MVC.Controllers.Api;

/// <author> Niels Van Steen</author>
/// <summary>
/// Counterpart of the <see cref="ProjectTagController"/> for web API.
/// </summary>
[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectTagsController : Controller
{
    // Fields.
    private readonly IProjectTagManager _projectTagService;
    private readonly IProjectManager _projectService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    // Constructor.
    public ProjectTagsController(UserManager<User> userManager, IAuthorizationService authorizationService,
        IProjectTagManager projectTagService, IProjectManager projectService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
        _projectTagService = projectTagService;
        _projectService = projectService;
    } // ProjectModerationController.

    // Methods.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Creates a new <see cref="ProjectTag"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Post([FromBody] ProjectTagDto projectTagDto)
    {
        // Check whether the developer exists.
        if (_projectTagService.GetProjectTag(projectTagDto.ProjectTagId) != null)
            return Conflict(projectTagDto);
            
        // Create ProjectTag.
        string name = projectTagDto.ProjectExternalName ?? ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);
        var projectTag = projectTagDto.ConvertToProjectTag(project);
        
        if (project == null)
            return NotFound("Project doesn't exist");
        
        if (projectTagDto.Name.IsNullOrEmpty())
            return Conflict("Name can't be empty!");

        if (_projectTagService.GetProjectTagByProjectAndName(project, projectTagDto.Name) != null)
            return Conflict("Tag name must be unique");

        // Create the project tag.
        _projectTagService.AddProjectTag(projectTag);

        projectTagDto.ProjectTagId = projectTag.ProjectTagId;
        return Created("/ProjectTag/Index", projectTagDto);
    } // Post.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Deletes a project tag.
    /// </summary>
    /// <param name="tagId">The id of the tag that should be deleted.</param>
    /// <returns></returns>
    [HttpDelete("Delete/{tagId:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Delete(int tagId)
    {
        var result = _projectTagService.RemoveProjectTag(tagId);

        if (result)
            return NoContent();

        return NotFound();
    } // Delete.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Updates a project tag.
    /// </summary>
    /// <param name="tagId"></param>
    /// <param name="projectTagDto"></param>
    /// <returns></returns>
    [HttpPut("Update/{tagId:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Put(int tagId, [FromBody] ProjectTagDto projectTagDto)
    {
        if (tagId != projectTagDto.ProjectTagId)
            return BadRequest("Id doesn't match");

        var tagInDb = _projectTagService.GetProjectTag(tagId);
        
        if (tagInDb == null)
            return NotFound("Tag does not exist!");
        
        string name = projectTagDto.ProjectExternalName ?? ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);
        var projectTag = projectTagDto.ConvertToProjectTag(project);

        if (project == null)
            return NotFound("Project doesn't exist");

        if (_projectTagService.GetProjectTagByProjectAndName(project, projectTagDto.Name) != null && tagInDb.Name.ToLower() != projectTagDto.Name.ToLower())
            return Conflict("Tag name must be unique");

        _projectTagService.ChangeProjectTag(projectTag);

        return Ok(projectTagDto);
    } // Put.
}