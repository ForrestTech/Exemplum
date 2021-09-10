namespace Infrastructure.DateAndTime
{
    using Application.Common.DateAndTime;
    using System;

    public class Clock : IClock
    {
        public DateTime Now
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}