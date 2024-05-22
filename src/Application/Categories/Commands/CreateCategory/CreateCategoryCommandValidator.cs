using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
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
                .MaximumLength(50).WithMessage("Description must not exceed 50 characters.");

            // rule for order must be greater than or equal to 0 and the limit of int
            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Order must be greater than or equal to 0.");
        }
    }
}
