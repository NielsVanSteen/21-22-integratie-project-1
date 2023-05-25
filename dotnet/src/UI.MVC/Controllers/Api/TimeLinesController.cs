using BL.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class TimeLinesController : ControllerBase
{
    // Fields.
    private readonly ITimeLineManager _timeLineManager;

    // Constructor.
    public TimeLinesController(ITimeLineManager timeLineManager)
    {
        _timeLineManager = timeLineManager;
    }

    // Methods.

    [HttpDelete("DeletePhase/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult DeletePhase(int id)
    {
        bool result;
        try
        {
           result = _timeLineManager.RemoveTimeLinePhase(id);
        } catch(Exception)
        {
            return NotFound();
        }

        if (result)
            return NoContent();

        return NotFound();
    } // DeletePhase.
}