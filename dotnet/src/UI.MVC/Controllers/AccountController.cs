using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BL.Project;
using BL.User;
using Domain.Project;
using Domain.User;
using identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.AccountManage;
using UI.MVC.Models.Shared;
using RegisterModel = UI.MVC.Models.Account.RegisterModel;

namespace UI.MVC.Controllers
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    ///  Handles all the authentication, including what comes with it. E.g., Login, Register, External login, email confirmation, reset password, ..
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IProjectManager _projectService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserPropertyManager _userPropertyService;
        private readonly IEmailSender _emailSender;
        private readonly IMarkedEmailManager _markedEmailService;
        private readonly IUserManager _userService;
        private readonly IAuthorizationService _authorizationService;

        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager, IEmailSender emailSender, IUserManager userService,
            IMarkedEmailManager markedEmailService, IUserPropertyManager userPropertyService,
            IProjectManager projectService, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _markedEmailService = markedEmailService;
            _signInManager = signInManager;
            _userPropertyService = userPropertyService;
            _projectService = projectService;
            _emailSender = emailSender;
            _authorizationService = authorizationService;
            _userService = userService;
        } // AccountController.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Check the user against the isModerator policy.
            var isModerator = (await _authorizationService.AuthorizeAsync(User, ApplicationConstants.IsModerator)).Succeeded;
            await _signInManager.SignOutAsync();

            // If the check succeeded redirect the user to the backend login page.
            if (isModerator)
                return RedirectToAction("Login", new {project = ApplicationConstants.BackEndUrlName});

            // Redirect to the normal login page.
            return RedirectToAction("Login");
        } // Logout.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Displays the Login view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            // When the user is already logged in redirect.
            if (User.Identity is {IsAuthenticated: true})
            {
                if (ApplicationConstants.GetProjectName(RouteData) == ApplicationConstants.BackEndUrlName)
                    return RedirectToAction("Index", "ProjectModeration");
                return RedirectToAction("Index", "Project");
            }


            return View();
        } // Login.

        /// <author>Niels Van Steen</author>
        /// <summary>
        ///  Login method, gets called when the user wants to log in. A direct log in, not via an external login provider.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            // Model not valid, return the login view.
            if (!ModelState.IsValid)
                return View(loginModel);

            // Get the user based on email.
            var projectName = ApplicationConstants.GetProjectName(RouteData);
            var user = _userService.GetUserByUserName(_userManager.GenerateUsername(loginModel.Email,
                projectName.ToLower()));

            // Check if the password is correct, if not return the user.
            bool passwordOk = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!passwordOk)
            {
                ModelState.AddModelError(String.Empty, "Deze combinatie van e-mail en wachtwoord zijn niet correct!");
                return View(loginModel);
            }

            var result =
                await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

            if (result.Succeeded)
            {
                // Return the user to the correct page after login or registration.
                return ReturnUserAfterLoginOrRegistration();
            }

            return View(loginModel);
        } // Login.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Displays the register view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            // When the user is already logged in redirect.
            if (User.Identity is {IsAuthenticated: true})
            {
                if (ApplicationConstants.GetProjectName(RouteData) == ApplicationConstants.BackEndUrlName)
                    return RedirectToAction("Index", "ProjectModeration");
                return RedirectToAction("Index", "Project");
            }

            // Pass all the userPropertyNames to the view.
            var project = _projectService.GetProjectByExternalName(ApplicationConstants.GetProjectName(RouteData));
            ViewBag.userPropertyNames = _userPropertyService.GetUserPropertyNamesByProject(project);
            return View();
        } // Register.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The normal registration method, when a user registers via the website, not via an external login provider.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            return await Register(registerModel, false);
        } // Register.

        /// <summary>
        /// Private register method. Called by Both <see cref="Register(RegisterModel)"/> as well as <see cref="CallBackExternalProvider(RegisterModel)"/>
        /// -> code for registering with a local account or with an external provider is now done in the same method.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <param name="isExternalLoginAccount">Is the method called from an external login or from a local account.</param>
        /// <returns></returns>
        private async Task<IActionResult> Register(RegisterModel registerModel, bool isExternalLoginAccount)
        {
            // Get the information about the user from the external login provider -> only when isExternalLoginAccount is true.
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null && isExternalLoginAccount)
            {
                ModelState.AddModelError(String.Empty, "Error loading external login information during confirmation.");
                return Redirect(ApplicationConstants.LoginReturnUrl(Url, RouteData));
            }

            // Get project information.
            string projectName = ApplicationConstants.GetProjectName(RouteData) ??
                                 ApplicationConstants.BackEndUrlName;
            Project project = _projectService.GetProjectByExternalName(projectName);
            ViewBag.userPropertyNames = _userPropertyService.GetUserPropertyNamesByProject(project);

            // Check the registerModel for validity.
            if (_userManager.ValidateUserToRegister(_markedEmailService, _userPropertyService, projectName, project,
                    registerModel, ModelState))
                return View(registerModel);

            // Create user object. and Try to create it in the database.
            var markedEmail = _markedEmailService.GetMarkedEmailByEmail(registerModel.Email, true);
            var user = _userService.CreateUserToRegister(registerModel.Firstname, registerModel.Lastname,
                registerModel.Email, project, projectName);
            if (markedEmail?.Projects != null)
                user.RegisteredForProjects.AddRange(markedEmail?.Projects);
            var result = await _userManager.CreateUserForRegistration(projectName, project, registerModel, user);

            if (result.Succeeded)
            {
                // Add the user to role.
                await _userManager.AddToCustomRole(projectName, user, markedEmail);

                if (markedEmail != null)
                    _markedEmailService.RemoveMarkedEmail(markedEmail.MarkedEmailId);

                // Add the external login. If isExternalLoginAccount is true.
                if (isExternalLoginAccount)
                {
                    info.ProviderKey = _userManager.GenerateUsername(info.ProviderKey, projectName);
                    result = await _userManager.AddLoginAsync(user, info);
                    if (!result.Succeeded)
                    {
                        ViewBag.ProviderDisplayName = info.ProviderDisplayName;
                        return View(registerModel);
                    } // If.
                } // If.

                // Check if email confirmation is required. if so send email and show confirm page. Else sign the user in.
                return await SignUserInWithConfirmationCheck(user, info?.LoginProvider ?? null);
            } // Register.

            // Check for registration errors. And return the view with the errors.
            await RegisterCheckModelErrors(result, registerModel, ModelState);
            ViewBag.ProviderDisplayName = info?.ProviderDisplayName;
            return View(registerModel);
        } // Register.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method gets called when the user presses 'register/login with google/facebook/...'.
        /// </summary>
        /// <param name="provider">the name of the external provider e.g., 'Google' -> with capital.</param>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> AuthenticateExternalProvider(string provider)
        {
            string returnUrl = ApplicationConstants.RegisterReturnUrl(Url, RouteData);

            // Call the external authentication provider, and pass a redirectUrl to it, (the Url will redirect to a method 'CalBackExternalProvider' in this controller class.
            var redirectUrl = Url.Action("CallBackExternalProvider", "Account", new {returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Task.FromResult<IActionResult>(new ChallengeResult(provider, properties));
        } // AuthenticateExternalProvider.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method gets called after <see cref="AuthenticateExternalProvider"/> when the user selected a profile on the external login provider.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CallBackExternalProvider(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= ApplicationConstants.LoginReturnUrl(Url, RouteData);

            // Check if there were any errors from the external login provider, if so redirect to the login page.
            if (remoteError != null)
            {
                ModelState.AddModelError(String.Empty, $"De externe login provider gaf een foutmelding: {remoteError}");
                return Redirect(returnUrl);
            }

            // Check if there's an error with loading the external login provider, if so redirect to the login page.
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(String.Empty,
                    "De login data van de externe provider kon niet worden geladen!");
                return Redirect(returnUrl);
            }

            // Check if the user already exists (in this project).
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);
            string projectName = ApplicationConstants.GetProjectName(RouteData);

            var user = _userService.GetUserByUserName(_userManager.GenerateUsername(email, projectName));
            if (user != null)
            {
                // Sign in the user with this external login provider if the user already has a login.
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                    _userManager.GenerateUsername(info.ProviderKey, projectName), isPersistent: false,
                    bypassTwoFactor: true);
                if (result.Succeeded)
                    return ReturnUserAfterLoginOrRegistration();
            }

            // If the user does not have an account, then ask the user to create an account.
            ViewBag.ProviderDisplayName =
                info.ProviderDisplayName; // Set the provider's name in the viewBag, so it can be displayed on the screen.
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var project = _projectService.GetProjectByExternalName(projectName);
                var userPropertyNames = _userPropertyService.GetUserPropertyNamesByProject(project);
                ViewBag.userPropertyNames = userPropertyNames;
                return View(new RegisterModel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    Lastname = info.Principal.FindFirstValue(ClaimTypes.Surname)
                });
            }

            return Redirect(returnUrl);
        } // CallBackExternalProvider.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// After a user has authenticated himself with an external authentication provider the user is redirected to <see cref="AccountController.CallBackExternalProvider(RegisterModel)"/>
        /// Where the user has to fill in more information, when the user presses register on that page then this method will be called.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CallBackExternalProvider(RegisterModel registerModel)
        {
            return await Register(registerModel, true);
        } // CallBackExternalProvider.

        /// <summary>
        /// Sends an email confirmation link to a user.
        /// </summary>
        /// <param name="user">User to send the email confirmation to.</param>
        private async Task SendConfirmationEmail(User user)
        {
            // Generate data to send the user a confirmation email.
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId, code}, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        } // SendConfirmationEmail.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method checks if the account requires a confirmed email. If not it signs the user in and redirects.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="authenticationMethod">Is by default null, This is only used when signing in with an external provider. and it is the name of the external provider E.g., 'Google'. </param>
        /// <returns>Returns the redirect url.</returns>
        private async Task<IActionResult> SignUserInWithConfirmationCheck(User user, string authenticationMethod = null)
        {
            // If email Confirmation is required, redirect to that page.
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await SendConfirmationEmail(user);
                return RedirectToAction("RegisterConfirmation", "Account");
            }

            // Email confirmation is not required, sign the user in and redirect.
            await _signInManager.SignInAsync(user, isPersistent: false, authenticationMethod);

            // Return the user to the correct page after login or registration.
            return ReturnUserAfterLoginOrRegistration();
        } // SignUserInWithConfirmationCheck.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Returns the user after they've logged in or registered.
        /// -> When a normal user logs in he's returned to the home page.
        /// -> when a <see cref="UserRole.ProjectManager"/> or <see cref="UserRole.Admin"/> logs in they're returned to a general overview of all the <see cref="Project"/>
        /// </summary>
        /// <returns></returns>
        private IActionResult ReturnUserAfterLoginOrRegistration()
        {
            string projectName = ApplicationConstants.GetProjectName(RouteData);

            // When logging in with the admin login -> redirect to a general project overview.
            if (projectName.ToLower() == ApplicationConstants.BackEndUrlName.ToLower())
                return RedirectToAction("Index", "ProjectModeration");

            return Redirect(ApplicationConstants.LoginReturnUrl(Url, RouteData));
        } // ReturnUserAfterLoginOrRegistration.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Check if the registration yielded any errors, if so this method adds them to the modelstate.
        /// </summary>
        /// <param name="result">The registration result</param>
        /// <param name="registerModel">The registration model containing the user inputted values.</param>
        /// <param name="modelState">The ModelState used to add errors to.</param>
        private Task RegisterCheckModelErrors(IdentityResult result, RegisterModel registerModel,
            ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                // Since we append '_projectName' to the username' the username is not the email the user entered so replace that error with a nicer one.
                if (error.Code.ToLower().Equals("duplicateusername"))
                    modelState.AddModelError("Duplicate Email",
                        "The email " + registerModel.Email + " is already taken!");
                else
                    modelState.AddModelError(string.Empty, error.Description);
            }

            return Task.CompletedTask;
        } // RegisterCheckModelErrors.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method display the page showing the user a message to confirm their email.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        } // RegisterConfirmation.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method confirms the email address of a user.
        /// </summary>
        /// <param name="userId">The id of the user who's email should be confirmed.</param>
        /// <param name="code">The confirmation code, to confirm the email address.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Validation.
            if (userId == null || code == null)
                return Redirect(ApplicationConstants.LoginReturnUrl(Url, RouteData));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Redirect(ApplicationConstants.LoginReturnUrl(Url, RouteData));

            // Confirm the email address.
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            // Display the appropriate message in the view.
            var emailConfirmedModel = new ConfirmStatusMessageModel();
            if (result.Succeeded)
            {
                emailConfirmedModel.Title = "E-mail bevestigd.";
                emailConfirmedModel.Description = "Danku voor het bevestigen van uw e-mail.";
                emailConfirmedModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            }
            else
            {
                emailConfirmedModel.Title = "E-mail kon niet worden bevestigd!";
                emailConfirmedModel.Description = "Er trad een ondebenkde fout op, probeer het later nog eens opnieuw.";
                emailConfirmedModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
            }

            return View(emailConfirmedModel);
        } // ConfirmEmail.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Displays the page where the usr can enter their email, to reset their password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        } // ForgotPassword.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Sends the user an email to recover their password.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            // Validation.
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            // Get the user based on email.
            string projectName = ApplicationConstants.GetProjectName(RouteData);
            var user = _userService.GetUserByUserName(_userManager.GenerateUsername(forgotPasswordModel.Email,
                projectName));

            // Show the success page.
            if (user == null)
            {
                ModelState.AddModelError("User", "Er werd geen account gevonden met dit e-mail adres!");
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ResetPassword", "Account", new {code}, Request.Scheme);

            await _emailSender.SendEmailAsync(forgotPasswordModel.Email, "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction("ConfirmedEmail", "Account", new ConfirmStatusMessageModel
            {
                Title = "E-mail verzonden!",
                Description = "We hebben u zo net een e-mail verstuurd waarmee u uw wachtwoord opnieuw kunt instellen.",
                Type = ConfirmStatusMessageModel.NotificationType.Type.Info
            });
        } // ForgotPassword.


        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Returns the view to reset the password. It also puts the code in the model.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            if (code == null)
                return BadRequest("A code must be supplied for password reset.");

            // Return the view with the code.
            return View(new ResetPasswordModel {Code = code});
        } // ResetPassword.'

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Resets the password by the password the user has given.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            // Validation.
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            // For some weird as reason When decoding the code in the 'ResetPassword' method with 'HttpGet' it returns an error 'invalid token'
            // Even though in the identity example they do it in the get method. There goes 3hours of my time, I'm totally fine ;(
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordModel.Code));

            // Get the user based on email.
            string projectName = ApplicationConstants.GetProjectName(RouteData);
            var user = _userService.GetUserByUserName(_userManager.GenerateUsername(resetPasswordModel.Email,
                projectName));

            if (user == null)
            {
                ModelState.AddModelError("User", "Er werd geen account gevonden met dit e-mail adres!");
                return View(resetPasswordModel);
            }

            // Reset the password.
            var result = await _userManager.ResetPasswordAsync(user, code, resetPasswordModel.Password);
            if (result.Succeeded)
                return RedirectToAction("PasswordReset", "Account", new ConfirmStatusMessageModel
                {
                    Title = "Success!", Description = "Uw wachtwoord is veranderd!",
                    Type = ConfirmStatusMessageModel.NotificationType.Type.Success
                });

            // Add possible model errors.
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(resetPasswordModel);
        } // ResetPassword.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Shows the view displaying the password has been reset.
        /// </summary>
        /// <param name="statusMessageModel">Model containing data to display a bootstrap alert-box</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PasswordReset(ConfirmStatusMessageModel statusMessageModel)
        {
            ViewData["Title"] = "Password Reset";
            return View(statusMessageModel);
        } // AccountConfirmation.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Shows the view displaying the email has been confirmed!
        /// </summary>
        /// <param name="statusMessageModel">Model containing data to display a bootstrap alert-box</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConfirmedEmail(ConfirmStatusMessageModel statusMessageModel)
        {
            ViewData["Title"] = "Email Confirmed";
            return View(statusMessageModel);
        } // AccountConfirmation.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Shows the view displaying a form to resend the email confirmation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        } // AccountConfirmation.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Sends a new email with a link to confirm the email address.
        /// </summary>
        /// <param name="resendEmailConfirmationModel">The model containing the email address, to send the confirmation email to.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(
            ResendEmailConfirmationModel resendEmailConfirmationModel)
        {
            // Validation.
            if (!ModelState.IsValid)
                return View(resendEmailConfirmationModel);

            var accountConfirmationModel = new ConfirmStatusMessageModel();
            string projectName = ApplicationConstants.GetProjectName(RouteData);
            var user = _userService.GetUserByUserName(_userManager.GenerateUsername(resendEmailConfirmationModel.Email,
                projectName));

            if (user == null)
            {
                accountConfirmationModel.Title = "E-mail niet verzonden!";
                accountConfirmationModel.Description = "Er werd geen account gevonden met dit e-mail adres.";
                accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
                ViewBag.AccountConfirmation = accountConfirmationModel;
                return View();
            }

            // Send a new email.
            await SendConfirmationEmail(user);

            // Display the message.
            accountConfirmationModel.Title = "E-mail verzonden!";
            accountConfirmationModel.Description = "We hebben u zo net een nieuwe bevestigings e-mail gestuurd.";
            accountConfirmationModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Success;
            ViewBag.AccountConfirmation = accountConfirmationModel;
            return View();
        } // ResendEmailConfirmation.
    }
}