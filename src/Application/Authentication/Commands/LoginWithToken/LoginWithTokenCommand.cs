using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Authentication.Commands.LoginWithToken
{

    public record LoginWithTokenCommand(string IdToken) : IRequest<Result<AuthenticationResult>>;
    public class LoginWithTokenCommandHandler : IRequestHandler<LoginWithTokenCommand, Result<AuthenticationResult>>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginWithTokenCommandHandler(IAuthService authService, ITokenService tokenService, IExpenseTrackerDbContext dbContext, IConfiguration configuration)
        {
            _authService = authService;
            _tokenService = tokenService;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<Result<AuthenticationResult>> Handle(LoginWithTokenCommand command, CancellationToken cancellationToken)
        {
            // verify the token
            var result = await _authService.VerifyTokenAsync(command.IdToken);
            if (result == null)
            {
                return Result<AuthenticationResult>.Failure(AuthError.InvalidToken);
            }

            if (!result.EmailVerified)
            {
                return Result<AuthenticationResult>.Failure(AuthError.EmailNotVerified);
            }

            // add user to db if not exists
            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Email == result.Email, cancellationToken: cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = result.Email,
                    FirstName = result.Name,
                    IdentityId = result.IdentityId,
                };

                await _dbContext.Users.AddAsync(user, cancellationToken);
                await CreateDefaultAccount(user, cancellationToken);
                await CreateDefaultCategories(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            await _authService.SetCustomClaims(result.IdentityId, new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.GivenName, user.FirstName ?? string.Empty },
                { JwtRegisteredClaimNames.FamilyName, user.LastName?? string.Empty },
                { "guid", user.Id },
            });


            return Result<AuthenticationResult>.Success(new AuthenticationResult
            {
                AccessToken = command.IdToken,
                IsEmailVerified = true
            });
        }

        private async Task CreateDefaultAccount(User user, CancellationToken cancellationToken)
        {
            // create default account
            var account = new Domain.Entities.Account
            {
                Id = Guid.NewGuid(),
                Name = user.FirstName!,
                Description = "Default Account",
                UserId = user.Id,
                IsActive = true,
                IsDefault = true
            };
            await _dbContext.Accounts.AddAsync(account, cancellationToken);
        }

        private async Task CreateDefaultCategories(User user, CancellationToken cancellationToken)
        {
            // create default categories
            var defaultCategories = _configuration.GetSection("Defaults:Categories").Get<List<string>>() ?? [];
            var categories = defaultCategories.Select((x, i) => new Category
            {
                Id = Guid.NewGuid(),
                Name = x,
                UserId = user.Id,
                Order = i + 1
            }).ToList();
            await _dbContext.Categories.AddRangeAsync(categories, cancellationToken);
        }
    }
}
