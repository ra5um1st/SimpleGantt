using System;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.Events.Common;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities.Common;

public class Task : NamedEntity
{
    private DateTimeOffset _startDate;
    public DateTimeOffset StartDate
    {
        get => _startDate;
        set
        {
            AddDomainEvent(new EntityModifiedEvent(this, nameof(StartDate), _startDate, value));
            _startDate = value;
        }
    }

    private DateTimeOffset _finishDate;
    public DateTimeOffset FinishDate
    {
        get => _finishDate;
        set
        {
            AddDomainEvent(new EntityModifiedEvent(this, nameof(FinishDate), _finishDate, value));
            _finishDate = value;
        }
    }

    private Percentage _completionPercentage;
    public Percentage CompletionPercentage
    {
        get => _completionPercentage;
        set
        {
            AddDomainEvent(new EntityModifiedEvent(this, nameof(CompletionPercentage), _completionPercentage, value));
            _completionPercentage = value;
        }
    }

    public Task(string name, DateTimeOffset startDate, DateTimeOffset finishDate, Percentage percentage) : base(name)
    {
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        CompletionPercentage = percentage ?? throw new ArgumentNullException(nameof(percentage));
    }
}
