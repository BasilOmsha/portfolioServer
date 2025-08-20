namespace Portfolio.Domain.Common
{
    public class Result<T>
    {
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(T? value, bool isSuccess, string? errorMessage)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null);
        public static Result<T> Failure(string error) => new Result<T>(default, false, error);
    }

    public class Result
    {
        public string? ErrorMessage { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
    }
}