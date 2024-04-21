using CS451_Team_Project.Models;
using CS451_Team_Project.Pages;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using CS451_Team_Project.Services.EmailTemplateService;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;


namespace CS451_Team_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IEmailTemplateService _templateService;
        private readonly ILogger<EmailController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailController(ILogger<EmailController> logger, IConfiguration config, IEmailTemplateService templateService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _config = config;
            _templateService = templateService;
            _userManager = userManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] string To)
        {
            _logger.LogInformation("Was called.");
            _logger.LogInformation("SendEmail action was called with input data: To={To}", To);
            var user = await _userManager.FindByEmailAsync(To);
            if (user == null)
            {
                _logger.LogError("User not found for email: {Email}", To);
                return BadRequest("User not found.");
            }

            var emailTemplate = _templateService.GetTemplate("Template1");
            // Generate a password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Encode the token for URL use
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = $"https://localhost:7050/ForgotPassword?token={encodedToken}&email={Uri.EscapeDataString(user.Email)}";
            // Assuming you have stored the user's name, operating system, and browser name in your ApplicationUser model
            // For demonstration, I'm using placeholders. You should replace them with actual property names.
            var userName = user.UserName; // Replace with actual property if available
            var operatingSystem = "Windows"; // This information needs to be captured or passed by the user, not typically stored in user management
            var browserName = "Chrome"; // Similarly, this is not typically stored in user management

            emailTemplate.Body = emailTemplate.Body
                .Replace("{{name}}", userName)  // Replace with actual name based on user's email
                .Replace("{{operating_system}}", operatingSystem)  // Replace with actual operating system based on user's email
                .Replace("{{browser_name}}", browserName)// Replace with actual browser name based on user's email
                .Replace("[Product Name]", "Financial Tracker")
                .Replace("{{action_url}}", callbackUrl);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailConfig:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(To));
            email.Subject = emailTemplate.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = emailTemplate.Body
            };

            // Check if there is an attachment path specified
            if (!string.IsNullOrEmpty(emailTemplate.AttachmentPath))
            {
                // Add the PDF document as an attachment
                builder.Attachments.Add(emailTemplate.AttachmentPath);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailConfig:EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailConfig:EmailUsername").Value, _config.GetSection("EmailConfig:EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
            return RedirectToPage("/NewLogin");
        }
    }
}
