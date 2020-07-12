using Email.Services.Emails;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Email.Services.PipelineBehaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        private readonly IEmailServiceResponder responder;

        public ValidationBehavior ( IEnumerable<IValidator<TRequest>> validators, IEmailServiceResponder responder) {
            this.validators = validators;
            this.responder = responder;
        }
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                responder.SetResult(EmailResult.ValidationError);
                return Task.FromResult<TResponse>(default);
            }
            else 
            {
                return next();
            }
        }
    }
}
