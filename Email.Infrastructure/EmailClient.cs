using Email.Services.Emails;
using Email.Services.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Email.Infrastructure
{
    public class EmailClient : IEmailClient
    {
        private readonly SmtpOptions appSettings;

        public EmailClient(IOptions<SmtpOptions> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task Send(EmailEntity email)
        {
            var smtpClient = new SmtpClient(appSettings.Host)
            {
                Port = appSettings.Port,
                Credentials = new NetworkCredential(appSettings.Username, appSettings.Password),
                EnableSsl = true,
            };

            //smtpClient.Send("email", "recipient", "subject", "body");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email.GetSender().Address, email.GetSender().Name),
                Subject = email.Subject,
                Body = email.Body
            };

            foreach (var r in email.GetRecipients())
            {
                mailMessage.To.Add(new MailAddress(r.Address,r.Name));
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
