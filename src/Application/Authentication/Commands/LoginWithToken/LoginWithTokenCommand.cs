using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public LoginWithTokenCommandHandler(IAuthService authService, ITokenService tokenService, IExpenseTrackerDbContext dbContext)
        {
            _authService = authService;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        public async Task<Result<AuthenticationResult>> Handle(LoginWithTokenCommand command, CancellationToken cancellationToken)
        {
            // verify the token
            var result = await _authService.VerifyTokenAsync(command.IdToken);
            if (result == null)
            {
                return Result<AuthenticationResult>.Failure(AuthError.InvalidToken);
            }

            // add user to db if not exists
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == result.Email);
            if (user == null)
            {
                var newUser = new User
                {
                    Email = result.Email,
                    FirstName = result.Name,
                    IdentityId = result.IdentityId,
                };

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                user = newUser;
            }

            await _authService.SetCustomClaims(result.IdentityId, new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.GivenName, user.FirstName },
                { JwtRegisteredClaimNames.FamilyName, user.LastName },
                { "guid", user.Id },
            });


            return Result<AuthenticationResult>.Success(new AuthenticationResult
            {
                Token = command.IdToken,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsProfileSetupComplete = true
            });
        }
    }
}
