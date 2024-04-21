using CS451_Team_Project.Models;

namespace CS451_Team_Project.Services.EmailTemplateService
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly Dictionary<string, EmailTemplate> _templates;

        public EmailTemplateService()
        {

            _templates = new Dictionary<string, EmailTemplate>();

            // Add your email templates here
            _templates["Template1"] = new EmailTemplate
            {
                Subject = "Password Rest",
                Body = "<td class=\"content-cell\">\r\n" +
                "                      <h1>Hi {{name}},</h1>\r\n                      " +
                "<p>You recently requested to reset your password for your [Product Name] account." +
                " Use the button below to reset it. <strong>This password reset is only valid for the next 24 hours.</strong></p>\r\n" +
                "                      <!-- Action -->\r\n" +
                "                      <table class=\"body-action\" align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\r\n" +
                "                        <tbody><tr>\r\n" +
                "                          <td align=\"center\">\r\n" +
                "                            " +
                "                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n" +
                "                              <tbody><tr>\r\n" +
                "                                <td align=\"center\">\r\n" +
                "                                  <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n" +
                "                                    <tbody><tr>\r\n" +
                "                                      <td>\r\n" +
                "<a href='{{action_url}}' class='btn btn-primary' style='background-color: #3498db; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px;'>Reset your password</a>" +
                "                                      </td>\r\n" +
                "                                    </tr>\r\n                                  " +
                "</tbody></table>\r\n                                " +
                "</td>\r\n                              </tr>\r\n" +
                "                            </tbody></table>\r\n                          " +
                "</td>\r\n                        </tr>\r\n" +
                "                      </tbody></table>\r\n                      " +
                "<p>For security, this request was received from a {{operating_system}} device using {{browser_name}}." +
                " If you did not request a password reset, please ignore this email or <a href=\"{{support_url}}\">contact support</a> if you have questions.</p>\r\n" +
                "                      <p>Thanks,\r\n" +
                "                        <br>The [Product Name] Team</p>\r\n" +
                "                      <!-- Sub copy -->\r\n" +
                "                      <table class=\"body-sub\">\r\n" +
                "                        <tbody><tr>\r\n" +
                "                          <td>\r\n" +
                "                            <p class=\"sub\">If you’re having trouble with the button above, copy and paste the URL below into your web browser.</p>\r\n" +
                "                            <p class=\"sub\">{{action_url}}</p>\r\n" +
                "                          </td>\r\n" +
                "                        </tr>\r\n" +
                "                      </tbody></table>\r\n" +
                "                    </td>"
            };

            _templates["Template2"] = new EmailTemplate
            {

            };

            _templates["Template3"] = new EmailTemplate
            {

            };
        }

        public EmailTemplate GetTemplate(string templateName)
        {
            if (_templates.ContainsKey(templateName))
            {
                return _templates[templateName];
            }
            else
            {
                // Handle case where template with given name does not exist
                throw new ArgumentException($"Template with name '{templateName}' not found.");
            }
        }
    }
}