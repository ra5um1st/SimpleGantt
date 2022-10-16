namespace SimpleGantt.Domain.Interfaces;

public interface IValueObject<T> where T : notnull
{
    public T Value { get; }
}
