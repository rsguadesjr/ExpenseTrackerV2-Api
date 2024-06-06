using ExpenseTracker.Application.Accounts.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<Result<AccountDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountDto>>
    {
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IRequestContext _requestContext;

        public GetAccountByIdQueryHandler(IRequestContext requestContext, IExpenseTrackerDbContext dbContext)
        {
            _requestContext = requestContext;
            _dbContext = dbContext;
        }

        public async Task<Result<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.UserId == _requestContext.UserId && a.Id == request.Id)
                .ProjectToType<AccountDto>()
                .SingleOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                return Result<AccountDto>.Failure(AccountError.NotFound);
            }

            return Result<AccountDto>.Success(account);
        }
    }

}
