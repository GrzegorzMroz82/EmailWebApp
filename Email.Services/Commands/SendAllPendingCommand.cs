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
    public class SendAllPendingCommand : IRequest
    {
        public SendAllPendingCommand() { }
    }

    public class SendAllPendingCommandHandler : IRequestHandler<SendAllPendingCommand>
    {
        private readonly IEmailRepository emailRepo;
        private readonly IEmailServiceResponder responder;
        private readonly IEmailClient emailClient;

        public SendAllPendingCommandHandler(IEmailRepository emailRepo, IEmailServiceResponder responder, IEmailClient emailClient)
        {
            this.emailRepo = emailRepo;
            this.responder = responder;
            this.emailClient = emailClient;
        }

        public async Task<Unit> Handle(SendAllPendingCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<EmailEntity> emails = emailRepo.GetPendingEmails();
            if (!emails.Any())
            {
                responder.SetResult(EmailResult.NothingToSend);
                return await Unit.Task;
            }

            foreach (var e in emails)
            {
                await emailClient.Send(e);
                e.SetStatus(EmailStatus.sent);
                await emailRepo.Update(e);
            }

            responder.SetResult(EmailResult.Success);
            return await Unit.Task;
        }
    }
}


