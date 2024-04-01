using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CS451_Team_Project.Models
{
    public class EmailTemplate
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        [ValidateNever]
        public string AttachmentPath { get; set; }
    }
}
