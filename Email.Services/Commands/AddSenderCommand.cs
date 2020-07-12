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
    public class AddSenderCommand : IRequest
    {
        public AddSenderCommand() { }
        public AddSenderCommand(string emailId, EmailAddressDto sender)
        {
            EmailId = emailId;
            Sender = sender;
        }

        public string EmailId { get; set; }
        public EmailAddressDto Sender { get; set; }
    }

    public class AddSenderCommandHandler : IRequestHandler<AddSenderCommand>
    {
        private readonly IEmailRepository emailRepo;
        private readonly IEmailServiceResponder responder;

        public AddSenderCommandHandler(IEmailRepository emailRepo, IEmailServiceResponder responder)
        {
            this.emailRepo = emailRepo;
            this.responder = responder;
        }

        public Task<Unit> Handle(AddSenderCommand request, CancellationToken cancellationToken)
        {
            EmailEntity email = emailRepo.GetById(request.EmailId);
            if (email == null)
            {
                responder.SetResult(EmailResult.NotExists);
                return Unit.Task;
            }

            EmailAddress sender = email.GetSender();
            if (sender!= null)
            {
                responder.SetResult(EmailResult.AlreadyExists);
                return Unit.Task;
            }

            email.AddSender(new EmailAddress(request.Sender.Address, request.Sender.Name));
            emailRepo.Update(email);

            responder.SetResult(EmailResult.Success);
            return Unit.Task;
        }
    }
}

