var builder = WebApplication.CreateBuilder(args);

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Log.Logger = LogConfiguration.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policyOptions =>
    {
        policyOptions
            .WithOrigins(builder.Configuration.GetValue<string>("WebAppUrl") ?? "https://localhost:6001")
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

var hcBuilder = builder.Services.AddHealthChecks();

if (!builder.Configuration.UseInMemoryStorage())
{
    hcBuilder.AddSqlServer(builder.Configuration.GetDefaultConnection());
}

builder.Services.AddControllers();

builder.Services.AddFluentValidationClientsideAdapters();

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

builder.Services.AddMassTransit(x =>
{
    if (builder.Configuration.UseInMemoryStorage())
    {
        x.UsingInMemory();
    }
    else
    {
        x.UsingRabbitMq((context,cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        });    
    }
});

builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddMemoryCache();
}

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

app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.MapSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exemplum"));

//we are using both minimal API and Controllers as examples
app.MapWeatherForecastEndpoints();
app.MapElementsEndpoinst();

await Seeder.SeedDatabase(app.Services);

await app.RunAsync();

return 0;

// Make the implicit Program class public so test projects can access it
public partial class Program
{
}