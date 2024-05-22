using FluentValidation;

namespace ExpenseTracker.Application.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required")
                .MinimumLength(3)
                .WithMessage("First Name must be at least 3 characters")
                .MaximumLength(50)
                .WithMessage("First Name must not exceed 50 characters")
                .Matches("^[a-zA-Z0-9]*$")
                .WithMessage("Name must contain only letters and numbers.");

            // rule for last name with numbers and letters only and dashes

            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z0-9]*$")
                .WithMessage("Name must contain only letters and numbers.");


            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters");

        }
    }
}
