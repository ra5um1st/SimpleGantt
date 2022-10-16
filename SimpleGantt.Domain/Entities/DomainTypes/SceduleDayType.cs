﻿using System;
using SimpleGantt.Domain.Entities.Abstractions;
using SimpleGantt.Domain.ValueObjects;

namespace SimpleGantt.Domain.Entities;

public record SceduleDayType(Guid Id, EntityName Name, DayOfWeekType WeekDayType, bool IsWorkingDay) : DomainType(Id, Name);