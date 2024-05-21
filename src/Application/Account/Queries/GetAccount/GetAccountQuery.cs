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

namespace ExpenseTracker.Application.Account.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<Result<AccountDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Result<AccountDto>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;

        public GetAccountQueryHandler(IRequestContext requestContext, IExpenseTrackerDbContext dbContext)
        {
            _requestContext = requestContext;
            _dbContext = dbContext;
        }

        public async Task<Result<AccountDto>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.UserId == _requestContext.UserId && a.Id == request.Id)
                .Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IsActive = a.IsActive
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                return Result<AccountDto>.Failure(AccountError.NotFound);
            }

            return Result<AccountDto>.Success(account);
        }
    }

}
