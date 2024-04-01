using CS451_Team_Project.Models;

namespace CS451_Team_Project.Services.EmailTemplateService
{
    public interface IEmailTemplateService
    {
        EmailTemplate GetTemplate(string templateName);
    }
}
