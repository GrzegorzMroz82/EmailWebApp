
using Email.Services.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.Emails.Queries
{
    public class GetAllEmailsQuery : IRequest<Response<IEnumerable<EmailDto>>>
    {
        public GetAllEmailsQuery() { }
    }


    public class GetAllEmailsQueryHandler : IRequestHandler<GetAllEmailsQuery, Response<IEnumerable<EmailDto>>>
    {
        private readonly IEmailRepository emailRepo;

        public GetAllEmailsQueryHandler(IEmailRepository emailRepo)
        {
            this.emailRepo = emailRepo;

        }
        public async Task<Response<IEnumerable<EmailDto>>> Handle(GetAllEmailsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<EmailEntity> emails = emailRepo.GetAllEmails();

            return Response.Ok<IEnumerable<EmailDto>>(emails.Select(x => EmailDto.Make(x)));

        }
    }
}


