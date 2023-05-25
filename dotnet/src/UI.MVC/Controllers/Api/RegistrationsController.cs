using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.Dto;

namespace UI.MVC.Controllers.Api;

/// <author> Niels Van Steen</author>
/// <summary>
/// Counterpart of the <see cref="RegistrationController"/> for web API.
/// </summary>
[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class RegistrationsController : ControllerBase
{
    // Fields.
    private readonly IUserManager _userService;
    private readonly IUserPropertyManager _userPropertyService;
    private readonly IProjectManager _projectService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

    // Constructor.
    public RegistrationsController(UserManager<User> userManager, IUserPropertyManager userPropertyService, IAuthorizationService authorizationService, IUserManager userService, IProjectManager projectService)
    {
        _userManager = userManager;
        _authorizationService = authorizationService;
        _userService = userService;
        _userPropertyService = userPropertyService;
        _projectService = projectService;
    } // ProjectModerationController.
    
    // Methods.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Creates a new <see cref="UserPropertyName"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost("Create")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Post([FromBody] UserPropertyNameDto userPropertyNameDto)
    {
        
        // Create ProjectTag.
        string name = userPropertyNameDto.ProjectExternalName ?? ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);
        var userPropertyName = userPropertyNameDto.ConvertToUserPropertyName(project);
        
        if (project == null)
            return NotFound("Project doesn't exist");

        if (_userPropertyService.GetUserPropertyNameByProjectAndLabel(project, userPropertyNameDto.UserPropertyLabel) != null)
            return Conflict("Name must be unique!");
        
        // Create the user property name.
        _userPropertyService.AddUserPropertyName(userPropertyName);

        userPropertyNameDto.UserPropertyNameId = userPropertyName.UserPropertyNameId;
        return Created("/Registration/Index", userPropertyNameDto);
    } // Post.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Deletes a <see cref="UserPropertyName"/>.
    /// </summary>
    /// <param name="id">The id of the <see cref="UserPropertyName"/> that should be deleted.</param>
    /// <returns></returns>
    [HttpDelete("Delete/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Delete(int id)
    {
        var result = _userPropertyService.RemoveUserPropertyName(id);

        if (result)
            return NoContent();

        return NotFound();
    } // Delete.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Update a <see cref="UserPropertyName"/>
    /// </summary>
    /// <param name="id">The id of the </param>
    /// <param name="userPropertyNameDto"></param>
    /// <returns></returns>
    [HttpPut("Update/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Put(int id, [FromBody] UserPropertyNameDto userPropertyNameDto)
    {
        if (id != userPropertyNameDto.UserPropertyNameId)
            return BadRequest("Id doesn't match");

        var userPropertyNameInDb = _userPropertyService.GetUserPropertyName(id);
        
        if (userPropertyNameInDb == null)
            return NotFound("Property does not exist!");
        
        string name = userPropertyNameDto.ProjectExternalName ?? ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(name);
        var projectTag = userPropertyNameDto.ConvertToUserPropertyName(project);

        if (project == null)
            return NotFound("Project doesn't exist");

        if (_userPropertyService.GetUserPropertyNameByProjectAndLabel(project, userPropertyNameDto.UserPropertyLabel) != null && userPropertyNameInDb.UserPropertyLabel.ToLower() != userPropertyNameDto.UserPropertyLabel.ToLower())
            return Conflict("Property name must be unique");

        _userPropertyService.ChangeUserPropertyName(projectTag);

        return Ok(userPropertyNameDto);
    } // Put.
}