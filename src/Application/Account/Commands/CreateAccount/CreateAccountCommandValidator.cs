using FluentValidation;

namespace ExpenseTracker.Application.Account.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            // create rule for name, not empty, min 3 chars, max 20 characters, and only letters and numbers 
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters.")
                .MaximumLength(20)
                .WithMessage("Name must not exceed 20 characters.")
                .Matches("^[A-Za-z0-9-_.@ ]*$")
                .WithMessage("Name must contain only letters, numbers, hyphens, dots, underscores and at sign");

            RuleFor(x => x.Description)
                .MaximumLength(100)
                .WithMessage("Description must not exceed 100 characters.");
        }
    }
}
