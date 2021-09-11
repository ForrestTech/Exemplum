namespace Domain.Todo
{
    using Ardalis.SmartEnum;
    using System;

    public abstract partial class PriorityLevel : SmartEnum<PriorityLevel, string>
    {
        public static readonly PriorityLevel None = new NoPriority();
        public static readonly PriorityLevel Low = new LowPriorityLevel();
        public static readonly PriorityLevel Medium = new MediumPriority();
        public static readonly PriorityLevel High = new HighPriority();
        
        // You could change this so that the priority level had a method that took in IConfiguration or an Option set and returned a configurable value so this was not hard coded e.g  
        //public abstract TimeSpan GetReminderTime(IConfiguration configuration)
        
        
        public abstract TimeSpan ReminderTime { get; }

        private PriorityLevel(string name, string value) : base(name, value)
        {
        }

        // You can split these up into different files by creating extra partial versions of the PriorityLevel class
        private sealed class HighPriority : PriorityLevel
        {
            public HighPriority() : base(nameof(High), nameof(High))
            { }

            public override TimeSpan ReminderTime { get; } = TimeSpan.FromDays(1);
        }
        
        private sealed class MediumPriority : PriorityLevel
        {
            public MediumPriority() : base(nameof(Medium), nameof(Medium))
            {
            }

            public override TimeSpan ReminderTime { get; } = TimeSpan.FromDays(3);
        }
        
        
        private sealed class LowPriorityLevel : PriorityLevel
        {
            public LowPriorityLevel() : base(nameof(Low), nameof(Low))
            {
            }

            public override TimeSpan ReminderTime { get; } = TimeSpan.FromDays(5);
        }
        
        private sealed class NoPriority : PriorityLevel
        {
            public NoPriority() : base(nameof(None), nameof(None))
            {
            }

            public override TimeSpan ReminderTime { get; } = TimeSpan.Zero;
        }
    }
}