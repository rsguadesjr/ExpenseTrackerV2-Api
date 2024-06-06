using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.TransactionDate)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(250);

            RuleFor(x => x.AccountId)
                .NotEmpty();
        }
    }
}
