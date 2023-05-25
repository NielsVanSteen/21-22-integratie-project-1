using System.Text;
using System.Text.Encodings.Web;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace UI.MVC.Identity
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Class used to configure the email sender service.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        // Fields.
        private readonly IConfiguration _configuration;

        // Constructor.
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string content)
        {
            var apiKey = _configuration["SEND_GRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("SoftwareApplicationManager@gmail.com", "SoftwareApplication Manager");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
