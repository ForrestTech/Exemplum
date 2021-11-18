try
{
    Activity.DefaultIdFormat = ActivityIdFormat.W3C;

    Log.Logger = LogConfiguration.CreateLogger();

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    Log.Information("Starting API host");

    builder.Services.AddApplication(builder.Configuration, builder.Environment);
    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Default", policyOptions =>
        {
            policyOptions
                .WithOrigins(builder.Configuration.GetServiceUri("webapp")?.ToString() ?? "https://localhost:6001")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
        options.TokenValidationParameters = new TokenValidationParameters {NameClaimType = "name", RoleClaimType = "https://schemas.dev-ememplum.com/roles"};
    });

    var hcBuilder = builder.Services.AddHealthChecks()
        .ForwardToPrometheus();

    if (!builder.Configuration.UseInMemoryStorage())
    {
        hcBuilder.AddSqlServer(builder.Configuration.GetDefaultConnection());
    }

    //This does not seem to work with dotnet 6            
    //builder.Services.AddHealthChecksUI()
    //  .AddInMemoryStorage();

    builder.Services.AddControllers().AddFluentValidation(x => x.AutomaticValidationEnabled = false);

    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    builder.Services.Configure<RouteOptions>(options =>
    {
        options.LowercaseUrls = true;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Exemplum", Version = "v1"});
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        
        c.IncludeXmlComments(xmlPath);
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseApiExceptionHandler(true);    
    }
    else
    {
        app.UseApiExceptionHandler(false);
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseHttpMetrics();

    app.UseCors("Default");

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMetricServer();

    app.UseEndpoints(endpoints =>
    {
        //with a more complex set of health checks we can separate checks by tag
        endpoints.MapHealthChecks("/health/ready",
            new HealthCheckOptions {Predicate = (_) => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse});
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions {Predicate = (_) => false});

        endpoints.MapHealthChecksUI();

        endpoints.MapControllers();

        endpoints.MapMetrics();
    });

    app.MapSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exemplum"));

    //we are using both minimal API and Controllers as examples
    app.MapWeatherForecastEndpoints();

    await SeedDatabase(app);
    
    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


static async Task SeedDatabase(IHost host)
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

        ApplicationDbContextSeed.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating or seeding the database");

        throw;
    }
}

// Make the implicit Program class public so test projects can access it
public partial class Program
{
}