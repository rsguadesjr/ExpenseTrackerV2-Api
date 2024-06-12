using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
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

namespace ExpenseTracker.Application.Transactions.Queries.GetTransactionsByMonthAndYear
{
    public class GetTransactionsByMonthAndYearQuery : IRequest<Result<List<TransactionDto>>>
    {
        public int? Month { get; set; }
        public int Year { get; set; }
    }
    public class GetTransactionsByMonthAndYearQueryHandler : IRequestHandler<GetTransactionsByMonthAndYearQuery, Result<List<TransactionDto>>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;
        public GetTransactionsByMonthAndYearQueryHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public async Task<Result<List<TransactionDto>>> Handle(GetTransactionsByMonthAndYearQuery request, CancellationToken cancellationToken)
        {
            DateTime startDate;
            DateTime endDate;

            var query = _dbContext.Transactions
                                            .AsNoTracking()
                                            .Where(x => x.Account.UserId == _requestContext.UserId);
            if (request.Month.HasValue)
            {
                // date params will be the whole month
                startDate = new DateTime(request.Year, request.Month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
                endDate = new DateTime(request.Year, request.Month.Value, DateTime.DaysInMonth(request.Year, request.Month.Value), 0, 0, 0, DateTimeKind.Utc);
            }
            else
            {
                // date params will be the whole year
                startDate = new DateTime(request.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                endDate = new DateTime(request.Year, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            }

            query = query.Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate);

            var result = await query
                                .ProjectToType<TransactionDto>()
                                .ToListAsync(cancellationToken);

            return Result<List<TransactionDto>>.Success(result);
        }
    }
}
