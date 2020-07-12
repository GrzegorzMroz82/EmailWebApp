using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Services.Emails
{
    public enum EmailResult {
        NotSet, 
        Success, 
        NotExists,
        AlreadyExists,
        NothingToSend,
        ValidationError
    }
    public interface IEmailServiceResponder
    {
        void SetResult(EmailResult result);

    }
}
