using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Email.Services.Emails;
using Email.Services.Emails.Commands;
using Email.Services.Emails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmailWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly EmailServiceResponder responder;

        public EmailController(IMediator mediator, IEmailServiceResponder responder)
        {
            this.mediator = mediator;
            this.responder = responder as EmailServiceResponder;
        }

        [HttpGet("all")]
        [Produces(typeof(IEnumerable<EmailDto>))]
        public async Task<IActionResult> GetAll()
        {
            var res = await mediator.Send(new GetAllEmailsQuery());
            return Ok(res);
        }

        [HttpPut]        
        public async Task<IActionResult> CreateEmail([FromBody]CreateEmailCommand inputData )
        {        
            var res = await mediator.Send(inputData);
            return responder.GetHttpResult();
        }

        [HttpGet("details/{id}")]
        [Produces(typeof(EmailDto))]
        public async Task<IActionResult> GetEmail(string id)
        {
            var res = await mediator.Send(new GetEmailDetailsQuery(id));
            return Ok(res);
        }

        [HttpGet("status/{id}")]
        [Produces(typeof(EmailDto))]
        public async Task<IActionResult> GetEmailStatus(string id)
        {
            var res = await mediator.Send(new GetEmailStatusQuery(id));
            return Ok(res);
        }

        [HttpPost("recipients")]
        public async Task<IActionResult> DefineRecipients([FromBody]AddRecipientCommand inputData)
        {
            var res = await mediator.Send(inputData);
            return responder.GetHttpResult();
        }

        [HttpPost("sender")]
        public async Task<IActionResult> DefineSender([FromBody]AddSenderCommand inputData)
        {
            var res = await mediator.Send(inputData);
            return responder.GetHttpResult();
        }

        [HttpPost("send-all-pending")]
        public async Task<IActionResult> SendAllPending([FromBody]SendAllPendingCommand inputData)
        {
            var res = await mediator.Send(inputData);
            return responder.GetHttpResult();
        }
        
    }
}
