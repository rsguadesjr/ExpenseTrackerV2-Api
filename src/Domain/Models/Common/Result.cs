namespace ExpenseTracker.Domain.Models.Common
{
    public class Result<T>
    {
        public Result()
        { }

        public Result(List<Error> errors, T? value = default)
        {
            Errors = errors;
            Value = value;
        }

        public T? Value { get; }
        public List<Error> Errors { get; } = [];
        public bool IsSuccess => Errors.Count == 0;
        public static Result<T> Success() => new(new List<Error>());
        public static Result<T> Success(T value) => new(new List<Error>(), value);
        public static Result<T> Failure(List<Error> errors) => new(errors);
        public static Result<T> Failure(Error error) => new(new List<Error> { error });
    }


}
