using System.ComponentModel.DataAnnotations;
using BL.User;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UI.MVC.Identity;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.Account
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Model class for registration.
    /// Includes the basic registration inputs which are uniform for all projects.
    /// As well as project specific information. <see cref="UserPropertyValues"/>
    /// </summary>
    public class RegisterModel
    {
        // Properties.
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De {0} moet tussen {2} en {1} karakters lang zijn.")]
        [Display(Name = "Voornaam")]
        public string Firstname { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De {0} moet tussen {2} en {1} karakters lang zijn.")]
        [Display(Name = "Achternaam")]
        public string Lastname { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} Moet minstens {2} en maximum {1} karakters lang zijn.", MinimumLength = ApplicationConstants.MinimumPasswordLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// List containing all the project specific registration information in a string format.
        /// All the optional inputs are mapped into the the <see cref="UserPropertyValues"/>
        /// </summary>
        public IList<string> ExtraStringValues { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This list has the mapped values from <see cref="ExtraStringValues"/>. And is populating during parsing by <see cref="ParseUserPropertyValues"/>
        /// </summary>
        public IEnumerable<UserPropertyValue> UserPropertyValues { get; set; } = new List<UserPropertyValue>();
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        ///  Parses the <see cref="ExtraStringValues"/> into <see cref="UserPropertyValues"/>
        /// </summary>
        private ExtraUserInfoParser UserInfoParseModel { get; set; }
        
        // Constructor.
        public RegisterModel()
        {
            UserInfoParseModel = new ExtraUserInfoParser();
        } // RegisterModel.


        // Methods.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// <see cref="ExtraUserInfoParser.ParseUserPropertyValues"/>
        /// </summary>
        public void ParseUserPropertyValues(IList<UserPropertyName> userPropertyNames, ModelStateDictionary modelState)
        {
            UserPropertyValues = UserInfoParseModel.ParseUserPropertyValues(ExtraStringValues, userPropertyNames, modelState);
        } // ParseUserPropertyValues.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Adds the user to the property values.
        /// When parsing the values, the user may not exist yet.
        /// </summary>
        /// <param name="user">The user owning the property values.</param>
        public void AddUserToProperties(User user)
        {
            if (UserPropertyValues == null)
                return;
            
            foreach (var userPropertyValue in UserPropertyValues)
                userPropertyValue.User = user;
        } // AddUserToProperties.
        
        
       
        
        
        
    }
}