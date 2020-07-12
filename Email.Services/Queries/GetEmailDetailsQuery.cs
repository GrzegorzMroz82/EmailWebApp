
using Email.Services.Models;
using MediatR;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.Emails.Queries
{
    public class GetEmailDetailsQuery : IRequest<Response<EmailDto>>
    {
        public GetEmailDetailsQuery(string emailId)
        {
            EmailId = emailId;
        }

        public string EmailId { get; }
    }


    public class GetEmailDetailsQueryHandler : IRequestHandler<GetEmailDetailsQuery, Response<EmailDto>>
    {
        private readonly IEmailRepository emailRepo;

        public GetEmailDetailsQueryHandler(IEmailRepository emailRepo)
        {
            this.emailRepo = emailRepo;

        }
        public async Task<Response<EmailDto>> Handle(GetEmailDetailsQuery request, CancellationToken cancellationToken)
        {
            EmailEntity email = emailRepo.GetById(request.EmailId);
            if (email == null)
            {
                return Response.Fail<EmailDto>(EmailResult.NotExists);
            }
            return Response.Ok<EmailDto>(EmailDto.Make(email));

        }
    }
}

