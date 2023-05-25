using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.AccountManage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Model to change the user's password.
    /// </summary>
    public class ChangePasswordModel
    {
        // Properties.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The current password the user has.
        /// </summary>
        [Display(Name = "Huidig Wachtwoord")]
        [Required]
        public string CurrentPassword { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The new password the user wants.
        /// </summary>
        [Display(Name = "Nieuw Wachtwoord")]
        [Required]
        public string NewPassword { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// A confirmation of the new password the user wants.
        /// </summary>
        [Display(Name = "Bevestig Nieuw Wachtwoord")]
        [Required]
        public string ConfirmNewPassword { get; set; }

        // Constructor.
        public ChangePasswordModel()
        {
        }
    }
}