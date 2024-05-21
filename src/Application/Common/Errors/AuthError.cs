using ExpenseTracker.Domain.Models.Common;

namespace ExpenseTracker.Application.Common.Errors
{
    public class AuthError
    {
        public static readonly Error InvalidCredential = new("InvalidCredential", "Invalid email or password.", ErrorType.Conflict);
        public static readonly Error InvalidToken = new("InvalidToken", "Invalid token.", ErrorType.Conflict);
        public static readonly Error UserNotFound = new("UserNotFound", "User not found.", ErrorType.NotFound);
        public static readonly Error UserAlreadyExists = new("UserAlreadyExists", "User already exists.", ErrorType.Conflict);
        public static readonly Error DuplicateEmail = new("DuplicateEmail", "Email already exists.", ErrorType.Conflict);
    }
}
