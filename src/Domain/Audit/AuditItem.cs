namespace Exemplum.Domain.Audit;

using Common;
using Common.DateAndTime;

public class AuditItem : BaseEntity<Guid>
{
    /// <summary>
    /// EF constructor
    /// </summary>
    private AuditItem()
    {
    }

    public AuditItem(IClock clock)
    {
        EventTime = clock.Now;
    }

    public DateTime EventTime { get; set; }

    public string EventType { get; init; } = string.Empty;

    public string EventData { get; init; } = string.Empty;
}