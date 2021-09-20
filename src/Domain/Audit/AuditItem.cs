namespace Exemplum.Domain.Audit
{
    using Common;
    using Common.DateAndTime;
    using System;

    public class AuditItem : BaseEntity<Guid>
    {
        /// <summary>
        /// EF constructor
        /// </summary>
        private AuditItem()
        { }
        
        public AuditItem(IClock clock)
        {
            EventTime = clock.Now;
            EventType = string.Empty;
            EventData = string.Empty;
        }
        
        public DateTime EventTime { get; set; }

        public string EventType { get; init; }

        public string EventData { get; init; }
    }
}