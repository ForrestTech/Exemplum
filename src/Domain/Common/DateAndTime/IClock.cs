namespace Domain.Common.DateAndTime
{
    using System;

    public interface IClock
    {
        public DateTime Now { get; }    
    }
}