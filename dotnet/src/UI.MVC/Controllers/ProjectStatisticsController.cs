using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;

namespace UI.MVC.Controllers;

/// <author> Niels Van Steen</author>
/// <summary>
/// This controller shows the project statistics page. (charts with all the project information).
/// </summary>
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class ProjectStatisticsController : Controller
{
    // Fields.
    
    // Constructor.
    public ProjectStatisticsController()
    {
    }

    // Methods.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// returns the view with all the project statistics (those charts are shown with javascript and thus handled with fetch api).
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Dashboard()
    {
        return View();
    } // Dashboard.
}