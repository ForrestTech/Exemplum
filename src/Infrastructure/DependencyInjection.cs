namespace Infrastructure
{
    using Application.Common.DomainEvents;
    using Application.Persistence;
    using DomainEvents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("CleanArchitectureDb"));
            
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
            services.AddScoped<IEventHandlerDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            services.AddTransient<IDomainEventService, DomainEventService>();
            
            return services;
        }
    }
}