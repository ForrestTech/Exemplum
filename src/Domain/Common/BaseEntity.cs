namespace Exemplum.Domain.Common
{
    using System;
    using System.Collections.Generic;

    public abstract class BaseEntity: BaseEntity<int>
    {
       
    }

    public abstract class BaseEntity<TKey> : IEntity<TKey>, IAuditableEntity, IHaveDomainEvents
    {
        public TKey Id { get; private set; } = default!;

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}