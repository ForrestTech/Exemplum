namespace Exemplum.Domain.Common;

public abstract class DomainEvent
{
    public DateTimeOffset DateOccurred { get; set; }
}