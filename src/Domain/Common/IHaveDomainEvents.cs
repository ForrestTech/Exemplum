namespace Domain.Common
{
    using System.Collections.Generic;
    using Todo;

    public interface IHaveDomainEvents
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}