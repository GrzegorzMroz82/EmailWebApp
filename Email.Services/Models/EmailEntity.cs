using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Services.Models
{
    public enum EmailStatus { pending, sent }
    public class EmailEntity
    {
        List<EmailAddress> ToEmails = new List<EmailAddress>();
        EmailAddress FromEmail;
        public string Id { get; }
        public string Author { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailStatus Status { get; private set; }

        public EmailEntity(string id, string author, string subject,string body)
        {
            Id = id;
            Author = author;
            Subject = subject;
            Body = body;
            Status = EmailStatus.pending;
        }


        public void AddRecipients(List<EmailAddress> toEmails)
        {
            ToEmails.AddRange(toEmails);
        }

        public IEnumerable<EmailAddress> GetRecipients()
        {
            return ToEmails;
        }

        public EmailAddress GetSender()
        {
            return FromEmail;
        }

        internal void AddSender(EmailAddress emailAddress)
        {
            this.FromEmail = emailAddress;
        }

        internal void SetStatus(EmailStatus sent)
        {
            this.Status = sent;
        }
    }
}
