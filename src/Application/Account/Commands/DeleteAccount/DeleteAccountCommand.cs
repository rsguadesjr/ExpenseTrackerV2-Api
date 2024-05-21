using ExpenseTracker.Application.Account.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Account.Commands.DeleteAccount
{
    public class DeleteAccountCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public bool ForceDelete { get; set; } = false;
    }

    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result<string>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;

        // readonly fields for IrequestContext and IExpenseTrackerDbContext and constructor
        public DeleteAccountCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<Result<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts.SingleOrDefaultAsync(x => x.UserId == _requestContext.UserId && x.Id == request.Id);
            if (account == null)
            {
                return Result<string>.Failure(AccountError.NotFound);
            }

            var accounts = await _dbContext.Accounts
                .Where(x => x.UserId == _requestContext.UserId && x.Id != request.Id)
                .ToListAsync(cancellationToken);

            // at least one account exists
            if (accounts.Count == 0)
            {
                return Result<string>.Failure(AccountError.AtLeastOneAccountIsActive);
            }

            // throw error if account has transactions and force delete is not set
            if (!request.ForceDelete)
            {
                var transactionCount = await _dbContext.Transactions
                    .CountAsync(x => x.AccountId == request.Id, cancellationToken);
                if (transactionCount > 0)
                {
                       return Result<string>.Failure(AccountError.AccountHasTransactions);
                }
            }

            // set another account to default if the deleted account was default
            if (account.IsDefault)
            {
                var acc = accounts.FirstOrDefault();
                if (acc != null)
                {
                    acc.IsDefault = true;
                }
            }


            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            return Result<string>.Success("");
        }
    }
}
