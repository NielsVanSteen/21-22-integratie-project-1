using BL.Project;
using Domain.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;
using UI.MVC.Models.ProjectStyling;

namespace UI.MVC.Controllers.Api;

/// <author> Niels Van Steen </author>
/// <summary>
/// The web api controller for the project statistics.
/// </summary>
[ApiController]
[Authorize(Policy = ApplicationConstants.IsModerator)]
[Route("/api/{project}/[controller]")]
public class ProjectStylingsController : ControllerBase
{
    // Fields
    private readonly IThemeStylesManager _themeStylesManager;
    private readonly IProjectManager _projectManager;

    // Constructor.
    public ProjectStylingsController(IThemeStylesManager themeStylesManager, IProjectManager projectManager)
    {
        _themeStylesManager = themeStylesManager;
        _projectManager = projectManager;
    }

    // Methods.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Returns the style for a project.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetProjectStyle")]
    [AllowAnonymous]
    public IActionResult GetProjectStyle()
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, false, false, true);

        if (project == null)
            return NoContent();

        var style = project.ProjectStyling.ThemeStyle;
        style.Project = null;
        
        return Ok(style);
    } // GetProjectStyle.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Changes the projectStyles.
    /// </summary>
    /// <param name="newId">The new ThemeStyles id.</param>
    /// <returns></returns>
    [HttpPut("Update/{newId:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Put(int newId)
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName, false, false, true);
        var themeStyles = _themeStylesManager.GetThemeStyles(newId);

        if (project == null)
            return NotFound("Project doesn't exist!");

        if (project.ProjectStyling == null)
            return NotFound("Project Styling doesn't exist!");

        if (themeStyles == null)
            return NotFound("Theme doesn't exist!");

        if (newId == project.ProjectStyling.ThemeStylesId)
            return Ok("Style has been updated!");

        var projectStyling = project.ProjectStyling;
        projectStyling.ThemeStylesId = themeStyles.ThemeStylesId;
        var newStyle = _themeStylesManager.ChangeProjectStyling(projectStyling);
        project.ProjectStylingId = newStyle.ProjectStylingId;
        _projectManager.ChangeProject(project);

        return Ok("Style has been updated!");
    } // Put.

    [HttpPost("CreateStyle")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Create(CreateStyleModel createStyleModel)
    {
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        var prevCreatedStyle = _themeStylesManager.GetThemeStylesByProject(project).ToList();
        var id = 0;
        
        if (prevCreatedStyle.Any())
        {
            id = prevCreatedStyle.First().ThemeStylesId;
            _themeStylesManager.RemoveThemeStyles(prevCreatedStyle.First().ThemeStylesId);
        }
        
        var themeStyle = new ThemeStyles
        {
            GenericName = createStyleModel.GenericName,
            DisplayName = createStyleModel.DisplayName,
            ColorLight = createStyleModel.ColorLight.Replace("#", ""),
            ColorMedium = createStyleModel.ColorMedium.Replace("#", ""),
            ColorDark = createStyleModel.ColorDark.Replace("#", ""),
            ColorDarkest = createStyleModel.ColorDarkest.Replace("#", ""),
            ProjectId = project.ProjectId
        };
        
        _themeStylesManager.AddThemeStyles(themeStyle);
        themeStyle.Project = null;

        return Created("", themeStyle);
    } // Create.
}