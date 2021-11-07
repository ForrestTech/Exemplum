namespace Exemplum.Domain.Common;

public interface IHaveDomainEvents
{
    public List<DomainEvent> DomainEvents { get; set; }
}