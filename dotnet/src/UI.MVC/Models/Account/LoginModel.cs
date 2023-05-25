using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
     
        [Display(Name = "Aangemeld blijven?")]
        public bool RememberMe { get; set; }
    }
}