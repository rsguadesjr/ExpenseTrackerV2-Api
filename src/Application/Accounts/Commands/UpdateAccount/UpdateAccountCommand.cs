using ExpenseTracker.Application.Accounts.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<Result<AccountDto>>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result<AccountDto>>
    {
        private readonly IRequestContext _requestContext;
        private readonly IExpenseTrackerDbContext _dbContext;
        public UpdateAccountCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task<Result<AccountDto>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == request.Id && x.UserId == _requestContext.UserId, cancellationToken);

            if (account == null)
            {
                return Result<AccountDto>.Failure(AccountError.NotFound);
            }

            var accounts = await _dbContext.Accounts
                .Where(x => x.UserId == _requestContext.UserId && x.Id != request.Id)
                .ToListAsync(cancellationToken);

            // account name must be unique
            if (accounts.Any(x => x.Name == request.Name))
            {
                return Result<AccountDto>.Failure(AccountError.NameNotUnique);
            }

            if (!request.IsActive && request.IsDefault)
            {
                return Result<AccountDto>.Failure(AccountError.DefaultAccountMustBeActive);
            }


            if (account.IsDefault && !request.IsDefault)
            {
                return Result<AccountDto>.Failure(AccountError.CurrentAccountMustRemainDefault);
            }

            // if account is default, set all other accounts to not default
            if (request.IsDefault)
            {
                foreach (var acc in accounts)
                {
                    acc.IsDefault = false;
                }
            }

            account.Name = request.Name;
            account.Description = request.Description ?? string.Empty;
            account.IsDefault = request.IsDefault;
            account.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Accounts
                .AsNoTracking()
                .ProjectToType<AccountDto>()
                .SingleAsync(x => x.Id == account.Id, cancellationToken);

            return Result<AccountDto>.Success(result);
        }
    }
}
