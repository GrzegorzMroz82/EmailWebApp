using Email.Services.Emails.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Services.Validation
{
    public class CreateEmailCommandValidator : AbstractValidator<CreateEmailCommand>
    {
        public CreateEmailCommandValidator()
        {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
            RuleFor(x => x.Subjet).NotEmpty();
        }
    }
}
