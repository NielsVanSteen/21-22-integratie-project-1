using Domain.User;
using identity.Models;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Identity.Authorization;
using UI.MVC.Models.Account;

namespace UI.MVC.Identity
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Class containing some constant values, used to avoid hard-coding or ease of use.
    /// </summary>
    public static class ApplicationConstants
    {
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// A user can register per project, -> the Url is unique per project.
        /// This constant holds the Value for 'project'. In theory it could be changed to another name
        /// </summary>
        public const string Project = "project";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This is the value in the url for the <see cref="UserRole.Admin"/> and <see cref="UserRole.ProjectManager"/> pages. (backend login page, ...)
        /// </summary>
        public const string BackEndUrlName = "admin";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Name of our company. -> By using this constant it can easily be chagned.
        /// </summary>
        public const string CompanyName = "Groenpunt";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// A constant for the policy 'IsModerator'. -> Checks whether a user is either a <see cref="UserRole.ProjectManager"/>. or an <see cref="UserRole.Admin"/>.
        /// </summary>
        public const string IsModerator = "IsModerator";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// A constant for the policy 'IsAdmin'. -> Checks whether a user is an <see cref="UserRole.Admin"/>.
        /// </summary>
        public const string IsAdmin = "IsAdmin";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// A constant for the policy 'IsAdmin'. -> Checks whether a user is a <see cref="UserRole.ProjectManager"/>.
        /// </summary>
        public const string IsProjectManager = "IsProjectManager";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Name of the policy used to check if a given user is allowed to view a project.
        /// <see cref="Authorization.CanViewProjectAuthorization"/>
        /// </summary>
        public const string CanViewProjectAuthorization = "CanViewProject";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Name of the policy used to check if a given user is allowed to view a doc-review.
        /// <see cref="Authorization.CanViewDocReviewAuthorization"/>
        /// </summary>
        public const string CanViewDocReviewAuthorization = "CanViewDocReview";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Checks whether a doc-review is only visible for authenticated users.
        /// <see cref="Authorization.IsLoginRequiredAuthorization"/>
        /// </summary>
        public const string IsLoginRequiredAuthorization = "IsLoginRequiredPolicyRequirement";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The basic URL the google cloud bucket. all the image urls start with this url.
        /// </summary>
        public const string CloudStorageBasicUrl = "https://storage.googleapis.com/docreview-bpart-groenpunt/";

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This constant is defined here since it's also used in the view model (<see cref="RegisterModel.Password"/>, <see cref="ResetPasswordModel.Password"/>).
        /// This was one of identity's errors when you edited the minimum length the view model kept saying the
        /// minimum length was '6'. So we'll use a constant to make sure the values are the same and will remain so.
        /// </summary>
        public const int MinimumPasswordLength = 8;

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Get the <see cref="Project"/> name from the Url.
        /// </summary>
        public static string GetProjectName(RouteData routeData) =>
            routeData.Values[Project]?.ToString() ?? ApplicationConstants.BackEndUrlName;

        public static string RegisterReturnUrl(IUrlHelper Url, RouteData routeData) =>
            Url.Content("~/") + routeData.Values[Project] + "/Project/Index";

        public static string LoginReturnUrl(IUrlHelper Url, RouteData routeData) =>
            Url.Content("~/") + routeData.Values[Project] + "/Project/Index";

        public static string LoginUrl(IUrlHelper Url, RouteData routeData) =>
            Url.Content("~/") + routeData.Values[Project] + "/Project/Index";
    }
}