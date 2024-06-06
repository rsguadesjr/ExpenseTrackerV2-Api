using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            // rule for id must not be an empty guid
            RuleFor(x => x.Id)
                .NotEmpty();

            // rule for name, not empty, min 3 chars, max 20 characters, and only letters and numbers and dashes with error messages
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20)
                .Matches("^[A-Za-z0-9-_.@ ]*$")
                .WithMessage("Name must contain only letters, numbers, hyphens, dots, underscores and at sign");


            RuleFor(x => x.Description)
                .MaximumLength(100);
        }
    }
}
