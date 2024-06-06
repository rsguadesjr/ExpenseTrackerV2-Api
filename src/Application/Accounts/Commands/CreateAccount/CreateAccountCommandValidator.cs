using FluentValidation;

namespace ExpenseTracker.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            // create rule for name, not empty, min 3 chars, max 20 characters, and only letters and numbers 
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
