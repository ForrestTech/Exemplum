namespace Exemplum.Domain.Common
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; }
    }
}