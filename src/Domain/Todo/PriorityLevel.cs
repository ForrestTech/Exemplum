namespace Domain.Todo
{
    using SmartEnum;
    using System;

    public abstract partial class PriorityLevel : SmartEnum<PriorityLevel, string>
    {
        public static readonly PriorityLevel None = new NonePriorityLevel();
        public static readonly PriorityLevel Low = new LowPriorityLevel();
        public static readonly PriorityLevel Medium = new MediumPriorityLevel();
        public static readonly PriorityLevel High = new HighPriorityLevel();
        
        public abstract TimeSpan? DefaultReminder { get; }

        private PriorityLevel(string name, string value) : base(name, value)
        {
        }

        // You can split these up into different files by creating extra partial versions of the PriorityLevel class
        private sealed class HighPriorityLevel : PriorityLevel
        {
            public HighPriorityLevel() : base(nameof(None), nameof(None))
            { }

            public override TimeSpan? DefaultReminder { get; } = TimeSpan.FromDays(1);
        }
        
        private sealed class MediumPriorityLevel : PriorityLevel
        {
            public MediumPriorityLevel() : base(nameof(None), nameof(None))
            {
            }

            public override TimeSpan? DefaultReminder { get; } = TimeSpan.FromDays(3);
        }
        
        
        private sealed class LowPriorityLevel : PriorityLevel
        {
            public LowPriorityLevel() : base(nameof(None), nameof(None))
            {
            }

            public override TimeSpan? DefaultReminder { get; } = TimeSpan.FromDays(5);
        }
        
        private sealed class NonePriorityLevel : PriorityLevel
        {
            public NonePriorityLevel() : base(nameof(None), nameof(None))
            {
            }

            public override TimeSpan? DefaultReminder { get; } = null;
        }
    }
}