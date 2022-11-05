using System;

namespace SimpleGantt.Core.Result
{
    public interface IResult
    {
        public bool Successed { get; }
        public Exception Exception { get; }
    }

    public interface IResult<out T> : IResult
    {
        public T Value { get; }
    }
}