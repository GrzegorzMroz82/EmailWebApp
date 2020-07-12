using Email.Services.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.Emails.Queries
{
    public class GetEmailStatusQuery : IRequest<Response<string>>
    {
        public GetEmailStatusQuery(string emailId)
        {
            EmailId = emailId;
        }

        public string EmailId { get; }
    }

    public class GetEmailStatusQueryHandler : IRequestHandler<GetEmailStatusQuery, Response<string>>
    {
        private readonly IEmailRepository emailRepo;

        public GetEmailStatusQueryHandler(IEmailRepository emailRepo)
        {
            this.emailRepo = emailRepo;
        }
        public async Task<Response<string>> Handle(GetEmailStatusQuery request, CancellationToken cancellationToken)
        {
            EmailEntity email = emailRepo.GetById(request.EmailId);

            if (email == null)
            {
                return Response.Fail<string>(EmailResult.NotExists);
            }
            return Response.Ok<string>(email.Status.ToString());

        }
    }
}
