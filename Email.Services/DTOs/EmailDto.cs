
using Email.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Email.Services.Emails.Queries
{
    public class EmailDto
    {
        public EmailAddress From { get; set; }
        public List<EmailAddress> To { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public string ID { get; set; }

        public static EmailDto Make(EmailEntity email)
        {
            return new EmailDto
            {
                ID = email.Id,
                Author = email.Author,
                Body = email.Body,
                From = email.GetSender(),
                Status = email.Status.ToString(),
                Subject = email.Subject,
                To = email.GetRecipients().ToList()
            };
        }
    }
}

