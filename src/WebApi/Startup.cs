namespace Exemplum.WebApi
{
    using Application;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using Infrastructure;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.IO;
    using System.Reflection;
    
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string DefaultCorsPolicy = "Default";
        private const string WebAppDefaultUri = "https://localhost:6001";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(_configuration);
            services.AddInfrastructure(_configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, builder =>
                {
                    builder.WithOrigins(_configuration.GetServiceUri("webapp")?.ToString() ??  WebAppDefaultUri)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = _configuration["Auth0:Authority"];
                options.Audience = _configuration["Auth0:ApiIdentifier"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "https://schemas.dev-ememplum.com/roles"
                };
            });
            
            var hcBuilder = services.AddHealthChecks();
            
            if(!_configuration.UseInMemoryStorage())
            {
                hcBuilder.AddSqlServer(_configuration.GetDefaultConnection());
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
            
            app.UseAuthentication();
            app.UseAuthorization();

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
                
                 
                if (env.IsDevelopment())
                {
                    // auth can be a real pain when testing locally this turns off auth for local comment out if you want to test auth
                    endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
                    //endpoints.MapControllers();
                }
                else
                {
                    endpoints.MapControllers();
                }
            });
        }
    }
}