namespace Exemplum.Domain.Common;

public abstract record TaggedEnum<TEnum>(string Name) where TEnum : TaggedEnum<TEnum>
{
    public sealed override string ToString()
    {
        return Name;
    }
}