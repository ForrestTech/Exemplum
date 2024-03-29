﻿namespace Exemplum.Infrastructure.DateAndTime;

using Domain.Common.DateAndTime;

public class Clock : IClock
{
    private readonly DateTime? _fixedTime;

    public Clock()
    {
    }

    public Clock(DateTime fixedTime)
    {
        _fixedTime = fixedTime;
    }

    public DateTime Now
    {
        get
        {
            return _fixedTime ?? DateTime.UtcNow;
        }
    }
}