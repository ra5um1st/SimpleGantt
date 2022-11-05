using System;

namespace SimpleGantt.Core.Result
{
    public struct Result : IResult
    {
        public static Result Empty { get; } = Success();

        public bool Successed { get; private set; }
        public Exception Exception { get; private set; }

        public static Result Success() => new Result()
        {
            Successed = true
        };

        public static Result Failure(Exception exception) => new Result()
        {
            Exception = exception,
            Successed = false
        };

        public static Result Failure(string message, Exception innerException) => new Result()
        {
            Exception = new FailureResultException(message, innerException),
            Successed = false
        };

        public static Result Failure(string message) => new Result()
        {
            Exception = new FailureResultException(message),
            Successed = false
        };
    }

    public struct Result<T> : IResult<T>
    {
        public static Result<T> Empty { get; } = Result<T>.Success(default!);

        public bool Successed { get; private set; }
        public Exception Exception { get; private set; }
        public T Value { get; private set; }

        public static Result<T> Success(T value) => new Result<T>()
        {
            Value = value,
            Successed = true
        };

        public static Result<T> Failure(Exception exception) => new Result<T>()
        {
            Exception = exception,
            Successed = false
        };

        public static Result<T> Failure(string message, Exception innerException) => new Result<T>()
        {
            Exception = new FailureResultException(message, innerException),
            Successed = false
        };

        public static Result<T> Failure(string message) => new Result<T>()
        {
            Exception = new FailureResultException(message),
            Successed = false
        };
    }

    public sealed class FailureResultException : Exception
    {
        public FailureResultException(string message) : base(message) { }
        public FailureResultException(string message, Exception innerException) : base(message, innerException) { }
    }
}
