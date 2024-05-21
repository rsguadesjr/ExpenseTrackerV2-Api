using ExpenseTracker.Application.Account.Common;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Account.Queries.GetAccounts
{
    public class GetAccountsQuery : IRequest<Result<List<AccountDto>>>
    {
    }

    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, Result<List<AccountDto>>>
    {
        private readonly IRequestContext _requestContext;
        private readonly IExpenseTrackerDbContext _dbContext;

        public GetAccountsQueryHandler(IRequestContext requestContext, IExpenseTrackerDbContext dbContext)
        {
            _requestContext = requestContext;
            _dbContext = dbContext;
        }

        public async Task<Result<List<AccountDto>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.Accounts
                .Where(a => a.UserId == _requestContext.UserId)
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IsActive = a.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result<List<AccountDto>>.Success(accounts);
        }
    }
}
