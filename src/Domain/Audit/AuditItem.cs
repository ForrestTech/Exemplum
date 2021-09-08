namespace Domain.Audit
{
    using Common;
    using System;

    public class AuditItem : BaseEntity<Guid>
    {
        public DateTime EventTime { get; set; } = DateTime.Now;

        public string EventType { get; set; } = string.Empty;

        public string EventData { get; set; } = string.Empty;
    }
}