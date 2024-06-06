using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
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

            var query = _dbContext.Transactions.AsNoTracking().Where(x => x.Account.UserId == _requestContext.UserId);
            //if (request.Month.HasValue)
            //{
            //    // filter query by startDate and endDate of month
            //    startDate = new DateTime(request.Year, request.Month.Value, 1);
            //    endDate = new DateTime(request.Year, request.Month.Value, DateTime.DaysInMonth(request.Year, request.Month.Value));
            //}
            //else
            //{
            //    // filter query by startDate and endDate
            //    startDate = new DateTime(request.Year, 1, 1);
            //    endDate = new DateTime(request.Year, 12, 31);
            //}

            //query = query.Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate);

            try
            {
                var result = await query.ProjectToType<TransactionDto>()
                                        .ToListAsync(cancellationToken);
                return Result<List<TransactionDto>>.Success(result);
            }
            catch (Exception ex)
            {
            }


            return Result<List<TransactionDto>>.Success(null);
        }
    }
}
