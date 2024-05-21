using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Commands.Common;
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
    public class GetTransactionByIdQuery : IRequest<Result<TransactionDto?>>
    {
        public Guid TransactionId { get; set; }
    }

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto?>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;

        public GetTransactionByIdQueryHandler(IExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<TransactionDto?>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            // query the database for the transaction with the given id
            try
            {
                var transaction = await _dbContext.Transactions
                                .ProjectToType<TransactionDto>()
                                .SingleOrDefaultAsync(x => x.Id == request.TransactionId);

                return Result<TransactionDto?>.Success(transaction);
            }
            catch (ArgumentNullException)
            {
                return Result<TransactionDto?>.Failure(TransactionError.NotFound);
            }
            catch (Exception)
            {
                // logger here
                return Result<TransactionDto?>.Failure(new Error("General", "An error occurred while fetching the transaction"));
            }

        }
    }
}
