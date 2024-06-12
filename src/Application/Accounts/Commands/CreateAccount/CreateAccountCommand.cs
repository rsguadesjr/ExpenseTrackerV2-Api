using ExpenseTracker.Application.Accounts.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Result<AccountDto>>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<AccountDto>>
    {
        private readonly IRequestContext _requestContext;
        private readonly IExpenseTrackerDbContext _dbContext;
        public CreateAccountCommandHandler(IExpenseTrackerDbContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public async Task<Result<AccountDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Domain.Entities.Account
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                IsActive = true,
                UserId = _requestContext.UserId,
                IsDefault = request.IsDefault
            };

            var accounts = await _dbContext.Accounts
                                                .Where(x => x.UserId == _requestContext.UserId)
                                                .ToListAsync(cancellationToken);

            // account name must be unique
            if (accounts.Any(x => x.Name == request.Name))
            {
                return Result<AccountDto>.Failure(AccountError.NameNotUnique);
            }

            // if account is default, set all other accounts to not default
            if (request.IsDefault)
            {
                foreach (var acc in accounts)
                {
                    acc.IsDefault = false;
                }
            }

            await _dbContext.Accounts.AddAsync(account, cancellationToken);
            await _dbContext.SaveChangesAsync(_requestContext.UserId, cancellationToken);

            var result = await _dbContext.Accounts
                .AsNoTracking()
                .ProjectToType<AccountDto>()
                .SingleAsync(x => x.Id == account.Id, cancellationToken);

            return Result<AccountDto>.Success(result);
        }
    }
}
