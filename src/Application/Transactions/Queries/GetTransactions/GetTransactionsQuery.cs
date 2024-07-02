using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Application.Transactions.Common;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Transactions.Queries.GetTransactions
{
    public class GetTransactionsQuery : IRequest<Result<List<TransactionDto>>>
    {
        public int? Month { get; set; }
        public int Year { get; set; }
        public int TimezoneOffset { get; set; } = 0;
        public Guid? AccountId { get; set; }
    }
    public class GetTransactionsByMonthAndYearQueryHandler : IRequestHandler<GetTransactionsQuery, Result<List<TransactionDto>>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;
        public GetTransactionsByMonthAndYearQueryHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public async Task<Result<List<TransactionDto>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset startDate;
            DateTimeOffset endDate;

            var query = _dbContext.Transactions
                                            .AsNoTracking()
                                            .Where(x => x.Account.UserId == _requestContext.UserId);

            if (request.Month.HasValue)
            {
                startDate = new DateTimeOffset(request.Year, request.Month.Value, 1, 0, 0, 0, TimeSpan.FromMinutes(request.TimezoneOffset));
                endDate = new DateTimeOffset(request.Year, request.Month.Value, DateTime.DaysInMonth(request.Year, request.Month.Value), 0, 0, 0, TimeSpan.FromMinutes(request.TimezoneOffset));
            }
            else
            {
                startDate = new DateTimeOffset(request.Year, 1, 1, 0, 0, 0, TimeSpan.FromMinutes(request.TimezoneOffset));
                endDate = new DateTimeOffset(request.Year, 12, 31, 0, 0, 0, TimeSpan.FromMinutes(request.TimezoneOffset));
            }

            if (request.AccountId.HasValue)
            {
                query = query.Where(x => x.AccountId == request.AccountId.Value);
            }

            var sDate = startDate.UtcDateTime;
            var eDate = endDate.UtcDateTime;
            query = query.Where(x => x.TransactionDate >= sDate && x.TransactionDate <= endDate);

            var result = await query
                                .ProjectToType<TransactionDto>()
                                .OrderByDescending(x => x.TransactionDate).ThenByDescending(x => x.CreatedDate)
                                .ToListAsync(cancellationToken);

            return Result<List<TransactionDto>>.Success(result);
        }
    }
}
