using Email.Services.DTOs;
using Email.Services.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.Emails.Commands
{
    public class AddRecipientCommand : IRequest
    {
        public AddRecipientCommand() { }
        public AddRecipientCommand(string emailId, List<EmailAddressDto> recipients)
        {
            EmailId = emailId;
            Recipients = recipients;
        }

        public string EmailId { get; set; }
        public List<EmailAddressDto> Recipients { get; set; }
    }

    public class AddRecipientCommandHandler : IRequestHandler<AddRecipientCommand>
    {
        private readonly IEmailRepository emailRepo;
        private readonly IEmailServiceResponder responder;

        public AddRecipientCommandHandler(IEmailRepository emailRepo,IEmailServiceResponder responder)
        {
            this.emailRepo = emailRepo;
            this.responder = responder;
        }

        public Task<Unit> Handle(AddRecipientCommand request, CancellationToken cancellationToken)
        {
            EmailEntity email = emailRepo.GetById(request.EmailId);
            if(email == null)
            {
                responder.SetResult(EmailResult.NotExists);
                return Unit.Task;
            }

            var existingToEmail = email.GetRecipients();
            var toAdd = request.Recipients.Where(x => !existingToEmail.Any(y => y.Address == x.Address)).ToList();

            if (!toAdd.Any())
            {
                responder.SetResult(EmailResult.AlreadyExists);
                return Unit.Task;
            }
            email.AddRecipients(toAdd.Select(x => new EmailAddress(x.Address, x.Name)).ToList());
            emailRepo.Update(email);

            responder.SetResult(EmailResult.Success);
            return Unit.Task;
        }
    }
}
