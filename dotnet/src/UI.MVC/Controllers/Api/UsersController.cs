using System.Security.Claims;
using System.Text.Json.Nodes;
using BL.User;
using Domain.User;
using identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.Android;
using UI.MVC.Models.Hub;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/{project}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UsersController(IUserManager userService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var user = _userService.GetUser(id);
        return Ok(user);
    }

    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Get the username, email and profile picture from an <see cref="User"/>
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    [HttpGet("AndroidUser")]
    public async Task<IActionResult> AndroidUser()
    {
        // Get the user based on email.
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NoContent();
        }

        var userDto = new UserAndroidDto(user);
        return Ok(userDto);
    }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Check if the email and password are correct and if the user role is <see cref="UserRole.ProjectManager"/> or <see cref="UserRole.Admin"/>
    /// </summary>
    /// <param name="loginModel"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("AndroidLogin/")]
    public async Task<IActionResult> AndroidLogin([FromBody] LoginModel loginModel)
    {
        // Get the user based on email.
        
        var userName = _userManager.GenerateUsername(loginModel.Email, ApplicationConstants.BackEndUrlName);
        var user = _userService.GetUserByUserName(userName);
        if (user == null)
        {
            return NoContent();
        }

        // Check if the password is correct, if not return the user.
        bool passwordOk = await _userManager.CheckPasswordAsync(user, loginModel.Password);
        if (!passwordOk)
        {
            return NoContent();
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
        if (result.Succeeded)
        {
            var cookies = HttpContext.Response.GetTypedHeaders();
            foreach (var cookie in cookies.SetCookie)
            {
                if (cookie.Name.ToString() == ".AspNetCore.Identity.Application")
                {
                    string value = cookie.Value.ToString();
                    return Ok(value);
                }
            }
        }
        return NoContent();
    }
    
    [HttpPost]
    public IActionResult Post([FromBody]User user)
    {
        _userService.AddUser(user);
        return NoContent();
    } // Post.

    
}