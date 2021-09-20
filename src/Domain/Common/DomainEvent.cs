namespace Exemplum.Domain.Common
{
    using System;

    public abstract class DomainEvent
    {
        public DateTimeOffset DateOccurred { get; set; }
    }
}