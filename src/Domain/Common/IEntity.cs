namespace Exemplum.Domain.Common;

public interface IEntity<out TKey>
{
    public TKey Id { get; }
}