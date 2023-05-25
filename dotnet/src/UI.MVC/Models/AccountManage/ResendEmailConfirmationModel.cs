using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.AccountManage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Model to resend an email confirmation link to confirm the user's account.
    /// </summary>
    public class ResendEmailConfirmationModel
    {
        // Properties.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The email to send the confirmation link to.
        /// </summary>
        [Display(Name = "E-mail")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        // Constructor.
        public ResendEmailConfirmationModel() {}
    }
}