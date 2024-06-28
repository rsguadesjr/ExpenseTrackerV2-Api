using ExpenseTracker.Domain.Models.Common.Authentication;

namespace ExpenseTracker.Application.Common.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string email, string password, string firstName, string lastName, string avatar);
        Task<string?> LoginWithEmailAndPasswordAsync(string email, string password);
        Task<VerifyTokenResult?> VerifyTokenAsync(string token);
        Task SetCustomClaims(string uid, IReadOnlyDictionary<string, object> customClaims);
    }
}
