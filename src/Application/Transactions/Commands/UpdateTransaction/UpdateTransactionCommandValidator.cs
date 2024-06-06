using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Commands.CreateTransaction;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.TransactionDate)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(250);

        }
    }
}
