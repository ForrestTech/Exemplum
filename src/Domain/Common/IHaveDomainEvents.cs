namespace Exemplum.Domain.Common
{
    using System.Collections.Generic;

    public interface IHaveDomainEvents
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}