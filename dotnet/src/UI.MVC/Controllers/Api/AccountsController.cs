using System.Net;
using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Attributes;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;

namespace UI.MVC.Controllers.Api;

/// <author> Niels Van Steen</author>
/// <summary>
/// Counterpart of the <see cref="AccountController"/> for web API.
/// </summary>
[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    // Fields.
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IProjectManager _projectService;
    private readonly ICloudStorage _cloudStorage;

    // Constructor.
    public AccountsController(IUserManager userService, UserManager<User> userManager,
        SignInManager<User> signInManager, ICloudStorage cloudStorage, IProjectManager projectService)
    {
        _userService = userService;
        _userManager = userManager;
        _projectService = projectService;
        _cloudStorage = cloudStorage;
        _signInManager = signInManager;
    } // AccountsController.

    // Methods.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Method to delete the profile picture of a user.
    /// The user's identity is not passed because it is contained in the cookie.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("DeleteProfilePicture")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var user = await _userManager.GetUserAsync(User);
        bool hasPicture = user?.HasProfilePicture ?? false;

        if (user == null || !hasPicture)
            return NoContent();

        try
        {
            await _cloudStorage.DeleteFileAsync(user.Id);
            user.HasProfilePicture = false;
            _userService.ChangeUser(user);

            return NoContent();
        }
        catch (Exception)
        {
            return NoContent();
        }
    } // DeleteProfilePicture.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Delete the user's profile.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("DeleteProfile")]
    [Authorize]
    public async Task<IActionResult> DeleteProfile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        try
        {
            _userService.RemoveUser(user.Id);
            await _signInManager.SignOutAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return Conflict("The user could not be deleted!");
        }
    } // DeleteProfile.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Uploads an image to cloud storage <see cref="ICloudStorage"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost("Upload")]
    [Authorize]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        // Check the file size.
        if (image.Length > (MaxFileSizeAttribute.DefaultMaxFileSizeInBytes))
            return Conflict("Max file size is 5MB");

        // Check the extension.
        if (!AllowedExtensionsAttribute.ImageDefaultAllowedExtension.Contains(Path.GetExtension(image.FileName)
                .ToLower()))
            return Conflict(AllowedExtensionsAttribute.GetErrorMessage());

        // Check if the profile picture needs updating.

        // Upload the image.
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _cloudStorage.UploadFileAsync(image, user.GenerateUsrProfilePictureFileName(), new CachingTime(CachingTime.CacheDefaults.Long));

        // If the user didn't have a profile picture before, update the database.
        if (!user.HasProfilePicture)
        {
            user.HasProfilePicture = true;
            _userService.ChangeUser(user);
            await _signInManager.RefreshSignInAsync(user);
        }

        var imageLink = user.GetUserProfilePictureImageLink(SquareImageSize.SM);
        return CreatedAtAction("Profile", "AccountManage", imageLink, imageLink);
    } // Upload.
}