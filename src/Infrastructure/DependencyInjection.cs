namespace Infrastructure
{
    using Application.Common.DateAndTime;
    using Application.Common.DomainEvents;
    using Application.Persistence;
    using DateAndTime;
    using DomainEvents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["UseInMemoryDatabase"]))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CleanArchitectureDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
            services.AddScoped<IEventHandlerDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            services.AddTransient<IDomainEventService, DomainEventService>();

            services.AddTransient<IClock, Clock>();
            
            return services;
        }
    }
}