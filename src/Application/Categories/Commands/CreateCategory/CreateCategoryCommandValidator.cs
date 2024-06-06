using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpenseTracker.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20)
                .Matches("^[A-Za-z0-9-_.@ ]*$")
                .WithMessage("Name must contain only letters, numbers, hyphens, dots, underscores and at sign");

            RuleFor(x => x.Description)
                .MaximumLength(50);

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0);
        }
    }
}
