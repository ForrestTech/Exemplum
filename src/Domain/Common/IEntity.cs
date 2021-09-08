namespace Domain.Common
{
    using Todo;

    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}