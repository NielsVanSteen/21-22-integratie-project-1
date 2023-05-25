using BL.Project;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UI.MVC.Controllers;

public class ErrorController : Controller
{
    
    public ErrorController(IUserManager userService, IProjectManager projectService, UserManager<User> userManager, SignInManager<User> signInManager) 
    {
        
    }

    public IActionResult Forbidden403()
    {
        return View();
    }
    
     public IActionResult NotFound404()
    {
        return View();
    }
}