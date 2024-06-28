using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using OneOf;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string FirstName,
                                  string LastName,
                                  string Email,
                                  string Password) : IRequest<Result<AuthenticationResult>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly IExpenseTrackerDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(ITokenService tokenService,
                                      IExpenseTrackerDbContext dbContext,
                                      IAuthService authService,
                                      IConfiguration configuration)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            // Validate user already if exists
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == command.Email);
            if (user != null)
            {
                return Result<AuthenticationResult>.Failure(AuthError.UserAlreadyExists);
            }

            // register to authentication service
            var uid = await _authService.RegisterAsync(command.Email, command.Password, command.FirstName, command.LastName, string.Empty);

            // Create User
            user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                IdentityId = uid,
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await CreateDefaultAccount(user, cancellationToken);
            await CreateDefaultCategories(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);


            // create custom claims
            await _authService.SetCustomClaims(uid, new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.GivenName, command.FirstName },
                { JwtRegisteredClaimNames.FamilyName, command.LastName },
                { "guid", user.Id },
            });

            // get access token
            var accessToken = await _authService.LoginWithEmailAndPasswordAsync(command.Email, command.Password);

            // verify email
            var authResult = await _authService.VerifyTokenAsync(accessToken);
            var isEmailVerified = authResult?.EmailVerified ?? false;
            if (isEmailVerified)
            {
                return Result<AuthenticationResult>.Success(new AuthenticationResult
                {
                    AccessToken = accessToken,
                    IsEmailVerified = true
                });
            }

            return Result<AuthenticationResult>.Success(new AuthenticationResult
            {
                IsEmailVerified = false
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
