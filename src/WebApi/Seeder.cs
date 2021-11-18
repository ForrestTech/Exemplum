namespace Exemplum.WebApi;

using Microsoft.Extensions.Hosting;

public static class Seeder
{
    public static async Task SeedDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            ApplicationDbContextSeed.SeedDatabase(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating or seeding the database");

            throw;
        }
    }    
}