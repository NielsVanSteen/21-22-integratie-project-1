using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.Account
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Model containing the user's email to reset their password.
    /// The email address is the address the reset-password-email will be send to.
    /// </summary>
    public class ForgotPasswordModel
    {
        // Properties.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The email belonging to the account.
        /// </summary>
        [EmailAddress]
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        // Constructors.
        public ForgotPasswordModel() {}
        public ForgotPasswordModel(string email)
        {
            Email = email;
        } // ForgotPasswordModel.
    }
}