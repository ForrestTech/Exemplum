namespace Exemplum.Domain.Common.DateAndTime;

public interface IClock
{
    public DateTime Now { get; }
}