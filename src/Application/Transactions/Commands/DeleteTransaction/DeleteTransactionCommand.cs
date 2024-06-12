using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommand : IRequest<Result<string>>
    {
        public Guid TransactionId { get; set; }
    }

    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result<string>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;
        public DeleteTransactionCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<Result<string>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(x => x.Account.UserId == _requestContext.UserId && x.Id == request.TransactionId, cancellationToken);
            if (transaction == null)
            {
                return Result<string>.Failure(TransactionError.NotFound);
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);
            return Result<string>.Success("");
        }
    }
}
