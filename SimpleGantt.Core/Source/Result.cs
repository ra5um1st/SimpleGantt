using System;

namespace SimpleGantt.Core
{
    public struct Result<T>
    {
        public bool IsSuccess { get; private set; }
        public Exception Exception { get; private set; }
        public T Value { get; private set; }

        public static Result<T> Success(T value) => new Result<T>()
        {
            Value = value,
            IsSuccess = true
        };

        public static Result<T> Failure(Exception exception) => new Result<T>()
        { 
            Exception = exception, 
            IsSuccess = false 
        };

        public static Result<T> Failure(string message) => new Result<T>() 
        {
            Exception = new FailureResultException(message),
            IsSuccess = false
        };
    }

    public sealed class FailureResultException : Exception
    {
        public FailureResultException(string message) : base(message) { }
    }
}
