using Microsoft.AspNetCore.Mvc;

namespace UI.MVC.Models.Error
{
    public class ErrorController : Controller
    {
        [Route("/error/NotFound404")]
        public IActionResult NotFound404()
        {
            return View();
        }
    }
}