using Email.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Email.Services.Emails
{
    public interface IEmailRepository
    {
        string CreateNew(string subjet, string author,  string body);
        EmailEntity GetById(string emailId);
        Task Update(EmailEntity entity);
        IEnumerable<EmailEntity> GetAllEmails();
        IEnumerable<EmailEntity> GetPendingEmails();

    }
}