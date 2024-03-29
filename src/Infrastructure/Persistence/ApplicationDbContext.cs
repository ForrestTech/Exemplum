﻿namespace Exemplum.Infrastructure.Persistence;

using Application.Common.DomainEvents;
using Application.Common.Identity;
using Application.Persistence;
using Domain.Audit;
using Domain.Common;
using Domain.Common.DateAndTime;
using Domain.Todo;
using ExceptionHandling;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IHandleDbExceptions _idbExceptions;
    private readonly IPublishDomainEvents _publishDomainEvents;
    private readonly IClock _clock;
    private readonly ICurrentUser _currentUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IHandleDbExceptions idbExceptions,
        IPublishDomainEvents publishDomainEvents,
        IClock clock,
        ICurrentUser currentUser) : base(options)
    {
        _idbExceptions = idbExceptions;
        _publishDomainEvents = publishDomainEvents;
        _clock = clock;
        _currentUser = currentUser;
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<AuditItem> AuditItems => Set<AuditItem>();

    /// <summary>
    /// A helper function that allows saving changes without publishing domain events.  This function is generally not used but can be useful to infrastructure code like data seeding that dont want to publish events.
    /// </summary>
    public int SaveChangesWithoutPublishing()
    {
        HandleAuditableEntities();

        try
        {
            var result = base.SaveChanges();
            return result;
        }
        catch (Exception e)
        {
            _idbExceptions.HandleException(e);
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        {
            HandleAuditableEntities();

            HandleAuditableEntities();

            await DispatchEvents();

            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                _idbExceptions.HandleException(e);
                throw;
            }
        }
    }

    private void HandleAuditableEntities()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUser.UserId ?? string.Empty;
                    entry.Entity.Created = _clock.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUser.UserId ?? string.Empty;
                    entry.Entity.LastModified = _clock.Now;
                    break;
                default:
                    // we dont need to handle the other entity state for auditability
                    break;
            }
        }
    }

    private async Task DispatchEvents()
    {
        var domainEvents = ChangeTracker
            .Entries<IHaveDomainEvents>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publishDomainEvents.Publish(domainEvent);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        IgnoreDomainEvents(builder);

        ConfigureSoftDeleteQueryFilter(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Dont map domains events to storage
    /// </summary>
    private static void IgnoreDomainEvents(ModelBuilder builder)
    {
        var propertyNames = typeof(IHaveDomainEvents).GetProperties()
            .Select(p => p.Name)
            .ToList();

        var entityTypes = builder.Model.GetEntityTypes()
            .Where(t => typeof(IHaveDomainEvents)
                .IsAssignableFrom(t.ClrType));

        foreach (var entityType in entityTypes)
        {
            var entityTypeBuilder = builder.Entity(entityType.ClrType);
            foreach (var propertyName in propertyNames)
            {
                entityTypeBuilder.Ignore(propertyName);
            }
        }
    }

    private static void ConfigureSoftDeleteQueryFilter(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }
        }
    }
}