using Email.Services.Emails;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmailWebApp
{
    internal class EmailServiceResponder : IEmailServiceResponder
    {
        EmailResult result;

        public void SetResult(EmailResult result)
        {
            this.result = result;
        }

        public StatusCodeResult GetHttpResult()
        {

            switch (result)
            {
                case EmailResult.Success:
                    return new OkResult();
                case EmailResult.AlreadyExists:
                case EmailResult.NothingToSend:                
                    return new NoContentResult();
                case EmailResult.NotSet:
                case EmailResult.NotExists:
                case EmailResult.ValidationError:
                    return new BadRequestResult();
                

                default:
                    throw new NotImplementedException();
            }
        }

    }
}