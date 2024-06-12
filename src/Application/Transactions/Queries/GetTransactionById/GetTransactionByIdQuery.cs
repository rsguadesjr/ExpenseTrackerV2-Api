using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IRequest<Result<TransactionDto>>
    {
        public Guid TransactionId { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;

        public GetTransactionByIdQueryHandler(IExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<TransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions
                            .AsNoTracking()
                            .ProjectToType<TransactionDto>()
                            .SingleOrDefaultAsync(x => x.Id == request.TransactionId, cancellationToken);

            if (transaction == null)
            {
                return Result<TransactionDto>.Failure(TransactionError.NotFound);
            }

            return Result<TransactionDto>.Success(transaction);
        }
    }
}
