namespace Exemplum.WebApi
{
    using Application;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.IO;
    using System.Reflection;
    
    // TODO Caching decorator for specific handler
    // TODO add docker support using project tye
    // TODO add seq to docker/tye (configure health checks)
    // TODO add docker support for sql/redis ??
    // TODO add Authentications of users (ideally we would not looks to add a super specific solution)
    // TODO add authorization including adding roles and resolving polices
    // TODO add full user support for Auditable items
    // TODO add support for metric pushing to grafana
    // TODO create nuget template package
    // TODO add github release drafting system
    // TODO add github code coverage
    // TODO add distributed tracing support
    // TODO add integration event emission using rabbit MQ
    // TODO add rabbit to docker
    // TODO .net 6 simple endpoints
    public class Startup
    {
        private const string DefaultCorsPolicy = "Default";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, builder =>
                {
                    builder.WithOrigins("https://localhost:6001")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            var hcBuilder = services.AddHealthChecks();
            
            if(!Configuration.UseInMemoryStorage())
            {
                hcBuilder.AddSqlServer(Configuration.GetDefaultConnection());
            }

            services.AddHealthChecksUI()
                .AddInMemoryStorage();
            
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exemplum", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.CustomOperationIds(apiDesc =>
                    apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exemplum"));
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(DefaultCorsPolicy);

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                //with a more complex set of health checks we can separate checks by tag
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (_) => false
                });
                
                endpoints.MapHealthChecksUI();
                endpoints.MapControllers();
            });
        }
    }
}