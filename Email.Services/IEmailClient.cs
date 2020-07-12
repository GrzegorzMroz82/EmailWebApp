using Email.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Email.Services.Emails
{
    public interface IEmailClient
    {
        Task Send(EmailEntity email);
    }
}
