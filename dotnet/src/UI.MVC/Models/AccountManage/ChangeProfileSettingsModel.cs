using System.ComponentModel.DataAnnotations;
using Domain.User;
using identity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UI.MVC.Attributes;
using UI.MVC.Models.Account;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.AccountManage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Model containing all the values to change the user's profile information.
    /// (Password not included). -> <see cref="ForgotPasswordModel"/>
    /// </summary>
    public class ChangeProfileSettingsModel
    {
        // Properties.
        
        [Required]
        [StringLength(50, MinimumLength = 3,  ErrorMessage = "De {0} moet tussen de {2} end {1} karakters lang zijn.")]
        [Display(Name = "Voornaam")]
        public string Firstname { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De {0} moet tussen de {2} end {1} karakters lang zijn.")]
        [Display(Name = "Achternaam")]
        public string Lastname { get; set; }
        
        /// <summary>
        /// Optional phoneNumber. -> is not asked during registration.
        /// </summary>
        [Display(Name = "GSM-nummer")]
        public string PhoneNumber { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Contains the profile picture.
        /// </summary>
        [MaxFileSize(5)]
        [AllowedExtensions(".png", ".jpg", ".jpeg", ".svg", ".gif")]
        public IFormFile ProfilePicture { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// List containing all the project specific registration information in a string format.
        /// All the optional inputs are mapped into the <see cref="UserPropertyValues"/>
        /// </summary>
        public IList<string> ExtraStringValues { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This list has the mapped values from <see cref="ExtraStringValues"/>. And is populating during parsing by <see cref="ParseUserPropertyValues"/>
        /// </summary>
        public IEnumerable<UserPropertyValue> UserPropertyValues { get; set; } = new List<UserPropertyValue>();
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Parses the <see cref="ExtraStringValues"/> into <see cref="UserPropertyValues"/>
        /// </summary>
        private ExtraUserInfoParser UserInfoParseModel { get; set; }

        // Constructor.
        public ChangeProfileSettingsModel()
        {
            UserInfoParseModel = new ExtraUserInfoParser();
        } // ChangeProfileSettingsModel.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// <see cref="ExtraUserInfoParser.ParseUserPropertyValues"/>
        /// </summary>
        public void ParseUserPropertyValues(IList<UserPropertyName> userPropertyNames, ModelStateDictionary modelState)
        {
            UserPropertyValues = UserInfoParseModel.ParseUserPropertyValues(ExtraStringValues, userPropertyNames, modelState);
        } // ParseUserPropertyValues.
    }
}