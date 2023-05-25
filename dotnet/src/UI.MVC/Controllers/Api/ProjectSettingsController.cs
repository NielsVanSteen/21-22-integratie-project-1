using System.ComponentModel.DataAnnotations;
using BL.Project;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Attributes;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Dto;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Authorize(Policy = ApplicationConstants.IsModerator)]
[Route("/api/{Project}/[controller]")]
public class ProjectSettingsController : ControllerBase
{
    private readonly IProjectManager _projectManager;
    private readonly UserManager<User> _userManager;
    private readonly ICloudStorage _cloudStorage;
    private readonly IProjectFooterLogoManager _footerLogoManager;

    // Constructor
    public ProjectSettingsController(IProjectManager projectManager, UserManager<User> userManager,
        ICloudStorage cloudStorage,
        IProjectFooterLogoManager footerLogoManager)
    {
        _projectManager = projectManager;
        _userManager = userManager;
        _footerLogoManager = footerLogoManager;
        _cloudStorage = cloudStorage;
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method to add privacy and accessibility statements to a proejct
    /// </summary>
    /// <param name="projectSettingsDto">DTO which holds the statements</param>
    /// <returns></returns>
    [HttpPost("AddPrivacyAndAccessibilityStatement")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult AddPrivacyAndAccessibilityStatement([FromBody] ProjectSettingsDto projectSettingsDto)
    {
        //get values from DTO
        var accessibility = projectSettingsDto.Accessibility;
        var privacy = projectSettingsDto.Privacy;

        // Get project from database
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        
        project.PrivacyStatement = privacy;
        project.AccessibilityStatement = accessibility;
        
        // Push the updated project to the database
        _projectManager.ChangeProject(project);

        //When everything is run as expected a NoContent is returned
        return NoContent();
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Get method for the different statements for a project
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAccessibilityAndPrivacyStatement")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult GetAccessibilityAndPrivacyStatement()
    {
        //Get project from routedata
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);

        //Create DTO containing statements from project instance
        var statements = new ProjectSettingsDto(project);

        //return statements
        return Ok(statements);
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Post method to archive a project
    /// </summary>
    /// <returns></returns>
    [HttpPost("ArchiveProject")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ArchiveProject()
    {
        //Get current user
        var user = await _userManager.GetUserAsync(User);

        //Get current project from routedata
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, true);

        // If the latest history is not published return BadRequest(400) because the project can not be archived
        if (project.GetLatestProjectHistory().ProjectStatus != ProjectStatus.Published)
        {
            return BadRequest("Cannot archive a project that is not published");
        }

        // Add a new ProjectHistory to the project
        project.ProjectHistories.Add(new ProjectHistory
        {
            EditedBy = user,
            EditedOn = DateTime.Now,
            Project = project,
            ProjectStatus = ProjectStatus.Archived
        });

        // Push the updated project to the database
        _projectManager.ChangeProject(project);

        return NoContent();
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Post method to publish a project
    /// </summary>
    /// <returns></returns>
    [HttpPost("PublishProject")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> PublishProject()
    {
        var user = await _userManager.GetUserAsync(User);

        //Get current project from routedata
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, true);

        // If the latest status is not Created then return BadRequest(400) because it is already published
        if (project.GetLatestProjectHistory().ProjectStatus != ProjectStatus.Created)
            return BadRequest("Project has already been published");
        

        // If the the accessibility or privacy statement are empty return BadRequest(400)
        if (String.IsNullOrWhiteSpace(project.AccessibilityStatement) ||
            String.IsNullOrWhiteSpace(project.PrivacyStatement))
        {
            return BadRequest("Can not publish project with empty Accessibility or privacy statement!");
        }

        // Add History to project
        project.ProjectHistories.Add(new ProjectHistory
        {
            EditedBy = user,
            EditedOn = DateTime.Now,
            Project = project,
            ProjectStatus = ProjectStatus.Published
        });

        // Push the updated project to the database
        _projectManager.ChangeProject(project);
        
        return NoContent();
    } // PublishProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Uploads a project footer logo image to the cloud storage.
    /// </summary>
    /// <returns></returns>
    [HttpPost("UploadFooterLogo")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        // Check the file size.
        if (image.Length > (MaxFileSizeAttribute.DefaultMaxFileSizeInBytes))
            return Conflict("Max file size is 5MB");

        // Check the extension.
        if (!AllowedExtensionsAttribute.ImageDefaultAllowedExtension.Contains(Path.GetExtension(image.FileName)
                .ToLower()))
            return Conflict(AllowedExtensionsAttribute.GetErrorMessage());

        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        var footerLogo = new FooterLogo {ProjectId = project.ProjectId};
        var filename = footerLogo.GenerateFooterLogoName(project);
        footerLogo.ImageName = filename;

        _footerLogoManager.AddFooterLogo(footerLogo);

        var name = await _cloudStorage.UploadFileAsync(image, filename,
            new CachingTime(CachingTime.CacheDefaults.Long));

        return CreatedAtAction("Index", "ProjectSetting", new {ImageLink = name, Id = footerLogo.FooterLogoId},
            new {ImageLink = footerLogo.GetFooterLogoFullLink(project, SquareImageSize.MD), Id = footerLogo.FooterLogoId});
    } // Upload.

    [HttpDelete("DeleteLogo/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Delete(int id)
    {
        var footerLogo = _footerLogoManager.GetFooterLogo(id);
        var result = _footerLogoManager.RemoveFooterLogo(id);

        _cloudStorage.DeleteFileAsync(footerLogo.ImageName);
        
        if (result)
            return NoContent();

        return NotFound();
    } // Delete.
}