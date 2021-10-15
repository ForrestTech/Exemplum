namespace Exemplum.UnitTests.Domain.Todo
{
    using Exemplum.Domain.Audit;
    using FluentAssertions;
    using Infrastructure.DateAndTime;
    using System;
    using Xunit;

    public class AuditItemTests
    {
        [Fact]
        public void AuditItem_event_time_should_match_clock()
        {
            var clock = new Clock(DateTime.UtcNow);
            
            var sut = new AuditItem(clock);

            sut.EventTime.Should().Be(clock.Now);
            sut.EventData.Should().Be(string.Empty);
            sut.EventType.Should().Be(string.Empty);
        }
    }
}