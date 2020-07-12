using Email.Services.Emails;
using Email.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Infrastructure
{
    public class EmailRepository : IEmailRepository
    {
        int index = 7;
        Dictionary<string, EmailEntity> storage = new Dictionary<string, EmailEntity>();


        public string CreateNew(string subjet, string author, string body)
        {
            string newId = (index++).ToString();
            var e = new EmailEntity(newId, author, subjet, body);
            storage.Add(newId, e);
            return newId;
        }

        public IEnumerable<EmailEntity> GetAllEmails()
        {
            return storage.Values;
        }

        public EmailEntity GetById(string emailId)
        {
            if (!storage.ContainsKey(emailId))
                return null;

            return storage[emailId];
        }

        public IEnumerable<EmailEntity> GetPendingEmails()
        {
            return storage.Values.Where(x => x.Status == EmailStatus.pending);
        }

        public Task Update(EmailEntity entity)
        {
            return Task.CompletedTask;
            //in memory so nothing to do
        }
    }
}
