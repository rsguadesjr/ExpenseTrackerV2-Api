using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Transactions.Queries.GetTransactionsByMonthAndYear
{
    public class GetTransactionsByMonthAndYearQueryValidator : AbstractValidator<GetTransactionsByMonthAndYearQuery>
    {
        public GetTransactionsByMonthAndYearQueryValidator()
        {
            // rule for year must be a valid year with 4 digits
            RuleFor(x => x.Year)
                .NotEmpty()
                .WithMessage("Year is required.")
                .Must(x => x.ToString().Length == 4)
                .WithMessage("Year must be a valid year with 4 digits.");

            RuleFor(x => x.Month)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Month must be greater than or equal to 1.")
                .LessThanOrEqualTo(12)
                .WithMessage("Month must be less than or equal to 12.");
        }
    }
}
