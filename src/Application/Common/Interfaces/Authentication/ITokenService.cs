namespace ExpenseTracker.Application.Common.Interfaces.Authentication
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId, string email, string firstName, string lastName, string avatar);
    }
}
