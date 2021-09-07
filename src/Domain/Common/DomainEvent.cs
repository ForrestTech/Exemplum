namespace Domain.Common
{
    using System;

    public abstract class DomainEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}