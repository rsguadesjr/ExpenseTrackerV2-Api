using ExpenseTracker.Application.Authentication.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Interfaces.Authentication;
using ExpenseTracker.Application.Common.Interfaces.Persistence;
using ExpenseTracker.Domain.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      var authResult = new AuthenticationResult
      {
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Token = token
      };

      return Result<AuthenticationResult>.Success(authResult);
    }
  }
}
