using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.AccountManage;
using UI.MVC.Models.Shared;

namespace UI.MVC.Controllers
{
    public class AccountManageController : Controller
    {
        // Fields.
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserManager _userService;
        private readonly IUserPropertyManager _userPropertyService;
        private readonly IProjectManager _projectService;
        public readonly IEmailSender _emailSender;
        public readonly ICloudStorage _cloudStorage;

        // Constructor.
        public AccountManageController(SignInManager<User> signInManager,
            UserManager<User> userManager, IUserManager userService, IUserPropertyManager userPropertyService, IProjectManager projectService, IEmailSender emailSender, ICloudStorage cloudStorage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _userPropertyService = userPropertyService;
            _projectService = projectService;
            _emailSender = emailSender;
            _cloudStorage = cloudStorage;
        } // AccountManageController.

        // Methods.

        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Called when returning the view to edit a profile.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Redirect(ApplicationConstants.LoginUrl(Url, RouteData));
            
            var changeProfileSettingsModel = new ChangeProfileSettingsModel();
            changeProfileSettingsModel.PhoneNumber = user.PhoneNumber;
            changeProfileSettingsModel.Firstname = user.Firstname;
            changeProfileSettingsModel.Lastname = user.Lastname;
            
            return View(changeProfileSettingsModel);
        } // Profile.
        
        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Method is executed when the user changes his basic profile information.
        /// </summary>
        /// <param name="changeProfileSettingsModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ChangeProfileSettingsModel changeProfileSettingsModel)
        {
            // Validation.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Redirect(ApplicationConstants.LoginUrl(Url, RouteData));
            
            // Parse the extra information properties.
            string projectName = ApplicationConstants.GetProjectName(RouteData) ?? ApplicationConstants.BackEndUrlName;
            Project project = _projectService.GetProjectByExternalName(projectName);
            var userPropertyNames = _userPropertyService.GetUserPropertyNamesByProject(project);
            changeProfileSettingsModel.ParseUserPropertyValues(userPropertyNames.ToList(), ModelState);

            // Check The modelState.
            if (!ModelState.IsValid)
                return View(changeProfileSettingsModel);

            var accountConfirmationModel = new ConfirmStatusMessageModel();
            
            // Check if the PhoneNumber needs to be changed.
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (changeProfileSettingsModel.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, changeProfileSettingsModel.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    accountConfirmationModel.Title = "Informatie is niet veranderd!";
                    accountConfirmationModel.Description = "Uw profiel gegevens konden niet worden veranderd.";
                    accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
                    ViewBag.AccountConfirmation = accountConfirmationModel;
                    return View(changeProfileSettingsModel);
                }
            }

            // Change the user settings.
            user.Firstname = changeProfileSettingsModel.Firstname;
            user.Lastname = changeProfileSettingsModel.Lastname;
            user.UserPropertyValues = changeProfileSettingsModel.UserPropertyValues;
            
            // Update user and refresh the login.
            _userService.ChangeUser(user);
            await _signInManager.RefreshSignInAsync(user);
            
            // Show success message.
            accountConfirmationModel.Title = "Informatie is veranderd!";
            accountConfirmationModel.Description = "Uw profiel gegevens zijn succesvol veranderd.";
            accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            ViewBag.AccountConfirmation = accountConfirmationModel;

            return View();
        } // Profile.
        
        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Display the view to change the user's password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            
            return View();
        } // ChangePassword.

        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Method called when the user changes his password.
        /// </summary>
        /// <param name="changePasswordModel"> Model containing the data to change the user's password. </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            // Validation.
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmNewPassword)
                ModelState.AddModelError(String.Empty, "De nieuwe wachtwoorden komen niet overeen!");
            
            if (!ModelState.IsValid)
                return View(changePasswordModel);
            
            // Check if the user can be found. otherwise redirect to the login page.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Redirect(ApplicationConstants.LoginUrl(Url, RouteData));
            
            // Change the password.
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(changePasswordModel);
            }
            
            await _signInManager.RefreshSignInAsync(user);
            
            // Display success message.
            var accountConfirmationModel = new ConfirmStatusMessageModel();
            accountConfirmationModel.Title = "Wachtwoord is veranderd!";
            accountConfirmationModel.Description = "Uw wachtwoord is veranderd, u kunt dit wachtwoord nu gebruiken om aan te melden!";
            accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            ViewBag.AccountConfirmation = accountConfirmationModel;
            return View();
        } // ChangePassword.

        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Displays the view where the user can add or delete login external login providers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult ExternalLogins(ConfirmStatusMessageModel accountConfirmationModel = null)
        {
            if (accountConfirmationModel != null)
                ViewBag.AccountConfirmation = accountConfirmationModel;
            return View();
        } // ExternalLogins.
        
        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Removes an external login provider. This is only possible when the user has multiple login options.
        /// </summary>
        /// <param name="loginProvider"> The name of the provider that should be deleted. e.g., 'Google' </param>
        /// <param name="providerKey">The key of the external login provider.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveExternalLogin(string loginProvider, string providerKey)
        {
            var accountConfirmationModel = new ConfirmStatusMessageModel();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Redirect(ApplicationConstants.LoginUrl(Url, RouteData));

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                accountConfirmationModel.Title = "Login niet verwijderd!";
                accountConfirmationModel.Description = "De externe login kon niet worden verwijderd.";
                accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
                return RedirectToAction("ExternalLogins", "AccountManage", accountConfirmationModel);
            }

            await _signInManager.RefreshSignInAsync(user);
            accountConfirmationModel.Title = "Login verwijderd!";
            accountConfirmationModel.Description = "De externe login is succesvol verwijderd!";
            accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            return RedirectToAction("ExternalLogins", "AccountManage", accountConfirmationModel);
        } // ExternalLogins.
        
        ///<author>Niels Van Steen</author>
        /// <summary>
        /// Adds a login provider when the user clicks the add button.
        /// </summary>
        /// <param name="provider">The name of the external login provider. E.g., 'Google'.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddExternalLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action("ExternalLoginsAddCallback", "AccountManage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        } // ExternalLogins.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method gets called after <see cref="ExternalLogins()"/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [Authorize]
        public async Task<IActionResult> ExternalLoginsAddCallback()
        {
            // Validation.
            var accountConfirmationModel = new ConfirmStatusMessageModel();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Redirect(ApplicationConstants.LoginUrl(Url, RouteData));

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
                throw new InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            
            info.ProviderKey = _userManager.GenerateUsername( info.ProviderKey, ApplicationConstants.GetProjectName(RouteData));
            
            // Add the login.
            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                accountConfirmationModel.Title = "Login kon niet worden toegevoegd!";
                accountConfirmationModel.Description = "De externe login Kon niet worden toegevoegd. Externe logins kunnen maar aan 1 account gekoppeld worden.";
                accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
                return RedirectToAction("ExternalLogins", "AccountManage", accountConfirmationModel);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            accountConfirmationModel.Title = "Login toegevoegd!";
            accountConfirmationModel.Description = "De externe login is toegevoegd, u kunt deze nu gebruiken om aan te melden.";
            accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            return RedirectToAction("ExternalLogins", "AccountManage", accountConfirmationModel);
        } // ExternalLoginsAddCallback.
    }
}