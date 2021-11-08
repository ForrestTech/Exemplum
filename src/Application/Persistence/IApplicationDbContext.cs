namespace Exemplum.Application.Persistence;

public interface IApplicationDbContext : IEventHandlerDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}