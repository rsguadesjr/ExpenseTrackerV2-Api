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
                .Must(x => x.ToString().Length == 4)
                .WithMessage(x => $"'{nameof(x.Year)}' must be a valid 4 digit year.");

            RuleFor(x => x.Month)
                .Must(x => !x.HasValue || (x >= 1 && x <= 12))
                .WithMessage(x => $"'{nameof(x.Month)}' must be within range of 1 to 12. 1 is January and 12 is December");
        }
    }
}
