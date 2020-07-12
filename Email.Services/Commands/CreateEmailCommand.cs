using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.Emails.Commands
{
    public class CreateEmailCommand : IRequest
    {
        public CreateEmailCommand() { }

        public CreateEmailCommand(string subjet, string author, string body)
        {
            Subjet = subjet;
            Author = author;
            Body = body;
        }

        public string Subjet { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
    }

    public class CreateEmailCommandHandler : IRequestHandler<CreateEmailCommand>
    {
        private readonly IEmailRepository emailRepo;
        private readonly IEmailServiceResponder responder;

        public CreateEmailCommandHandler(IEmailRepository emailRepo, IEmailServiceResponder responder)
        {
            this.emailRepo = emailRepo;
            this.responder = responder;
        }

        public Task<Unit> Handle(CreateEmailCommand request, CancellationToken cancellationToken)
        {
            var id= emailRepo.CreateNew(request.Subjet, request.Author, request.Body);
            responder.SetResult(EmailResult.Success);
            return Unit.Task;
        }
    }
}
