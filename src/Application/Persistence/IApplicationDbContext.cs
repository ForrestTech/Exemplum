namespace Application.Persistence
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationDbContext : IEventHandlerDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}