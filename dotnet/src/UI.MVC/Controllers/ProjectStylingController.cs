using BL.Project;
using Domain.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <author> Niels Van Steen</author>
/// <summary>
/// This controller used to change the project styling.
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectStylingController : Controller
{
    // Fields.
    private readonly IThemeStylesManager _themeStylesManager;
    private readonly IProjectManager _projectManager;

    // Constructor.
    public ProjectStylingController(IThemeStylesManager themeStylesManager, IProjectManager projectManager)
    {
        _themeStylesManager = themeStylesManager;
        _projectManager = projectManager;
    }

    // Methods.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Returns the page where the user can edit the project styling attributes.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        // Get the current style
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectManager.GetProjectByExternalName(projectName);
        var curStyleId = _projectManager.GetProjectByExternalName(projectName, false, false, true).ProjectStyling.ThemeStylesId;

        // Get all the styles.
        var styles = _themeStylesManager.GetAllThemeStyles();
        var projectStyles = _themeStylesManager.GetThemeStylesByProject(project);
        
        // Return the view with the data.
        ViewBag.GlobalStyles = styles;
        ViewBag.ProjectStyles = projectStyles;
        return View(curStyleId);
    } // Index.
}