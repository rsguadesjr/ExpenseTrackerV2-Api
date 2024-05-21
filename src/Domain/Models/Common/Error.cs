
namespace ExpenseTracker.Domain.Models.Common
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }
        public Error(string code, string message, ErrorType errorType = ErrorType.General)
        {
            Code = code;
            Description = message;
            ErrorType = errorType;
        }

        public static Error General(string code, string message)
        {
            return new Error(code, message, ErrorType.General);
        }

        public static Error Validation(string code, string message)
        {
            return new Error(code, message, ErrorType.Validation);
        }

        public static Error NotFound(string code, string message)
        {
            return new Error(code, message, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code, string message)
        {
            return new Error(code, message, ErrorType.Unauthorized);
        }

        public static Error Forbidden(string code, string message)
        {
            return new Error(code, message, ErrorType.Forbidden);
        }

        public static Error Conflict(string code, string message)
        {
            return new Error(code, message, ErrorType.Conflict);
        }
    }

    public enum ErrorType
    {
        General,
        Validation,
        NotFound,
        Unauthorized,
        Forbidden,
        Conflict
    }
}
