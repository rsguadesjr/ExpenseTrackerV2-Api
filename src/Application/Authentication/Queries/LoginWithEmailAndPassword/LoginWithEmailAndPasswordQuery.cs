using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Authentication.Queries.LoginWithEmailAndPassword
{
    public record LoginWithEmailAndPasswordQuery(string Email, string Password) : IRequest<Result<AuthenticationResult>>;
    public class LoginWithEmailAndPasswordQueryHandler : IRequestHandler<LoginWithEmailAndPasswordQuery, Result<AuthenticationResult>>
    {
        private readonly IAuthService _authService;
        private readonly IExpenseTrackerDbContext _dbContext;

        public LoginWithEmailAndPasswordQueryHandler(IAuthService authService, IExpenseTrackerDbContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }
        public async Task<Result<AuthenticationResult>> Handle(LoginWithEmailAndPasswordQuery query, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == query.Email);
            if (user == null)
            {
                return Result<AuthenticationResult>.Failure(AuthError.InvalidCredential);
            }

            var token = await _authService.LoginWithEmailAndPasswordAsync(query.Email, query.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Result<AuthenticationResult>.Failure(AuthError.InvalidCredential);
            }

            var result = await _authService.VerifyTokenAsync(token);
            if (result == null)
            {
                return Result<AuthenticationResult>.Failure(AuthError.InvalidToken);
            }

            if (!result.EmailVerified)
            {
                return Result<AuthenticationResult>.Failure(AuthError.EmailNotVerified);
            }

            return Result<AuthenticationResult>.Success(new AuthenticationResult
            {
                AccessToken = token,
                IsEmailVerified = result.EmailVerified,
            });
        }
    }
}
