namespace Exemplum.Domain.Todo;

using Common;
using Extensions;

/// <summary>
/// Example of using a clean way to model tagged enums in c# without using the limited built in enum class
/// The options for PriorityLevel have to be implemented as private sub types
/// The only way to create one is to either parse for the tag or use the factory properties e.g. PriorityLevel.High  
/// </summary>
public abstract record PriorityLevel(string Name, TimeSpan ReminderTime) : TaggedEnum<PriorityLevel>(Name)
{
    public static bool IsValid(string name) => Levels.Any(x => x.Name.IsTheSameAs(name));

    public static PriorityLevel Parse(string name) => Levels.First(x => x.Name.IsTheSameAs(name));
    
    public static (bool Success, PriorityLevel? Level) TryParse(string name)
    {
        var level = Levels.FirstOrDefault(x => x.Name.IsTheSameAs(name));
        return level is null ? (false, null) : (true, Level: level);
    }

    // You could do this via reflection on the base class but its has performance implications if not done correctly
    private static IEnumerable<PriorityLevel> Levels => new List<PriorityLevel> { None, Low, Medium, High };

    public static PriorityLevel None { get; } = new NoPriority();
    public static PriorityLevel Low { get; } = new LowPriority();
    public static PriorityLevel Medium { get; } = new MediumPriority();
    public static PriorityLevel High { get; } = new HighPriority();

    private record NoPriority() : PriorityLevel(nameof(None), TimeSpan.FromDays(1));

    private record LowPriority() : PriorityLevel(nameof(Low), TimeSpan.FromDays(1));

    private record MediumPriority() : PriorityLevel(nameof(Medium), TimeSpan.FromDays(1));

    private record HighPriority()  : PriorityLevel(nameof(Medium), TimeSpan.FromDays(1));
}


