using System;

namespace SimpleGantt.Core
{
    public struct Result<T>
    {
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

        public static Result<T> Failure(string message) => new Result<T>() 
        {
            Exception = new FailureResultException(message),
            Successed = false
        };
    }

    public sealed class FailureResultException : Exception
    {
        public FailureResultException(string message) : base(message) { }
    }
}
