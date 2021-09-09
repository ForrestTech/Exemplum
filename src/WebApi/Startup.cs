using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebApi
{
    using Application;
    using FluentValidation.AspNetCore;
    using Infrastructure;
    using Microsoft.AspNetCore.Routing;
    
    // Todo add command handler unit tests with mocks
    // Todo remove todo and do all actions via todolist
    // Todo global EF configuration to ignore domain events
    // Todo Add full database setup
    // Todo persistence migrations and configurations
    // Todo Add support for injectable dates
    // Todo add domain function examples like mark as done set priority (add unit tests for smart enum stuff or migrate to using the package) 
    // Todo add more write update and delete handlers and their domain events
    // Todo add handling for mapping database errors to real errors will need unique index and FKey exceptions
    // Todo Add a basic calendar view entity that is a read model over todos 
    // Todo demonstrate how multi aggregate updates work within a transaction for domains events
    // Todo add httpclient (call to 3rd party service)
    // Todo add docker support for sql/redis ??
    // Todo add redis for caching
    // Todo add poly
    // Todo add health checks
    // Todo add some custom health checks
    // Todo add github code coverage
    // Todo add way to seed data without domains event emission
    // Todo Authentications of users (ideally we would not looks to add a super specific solution)
    // Todo add authorization including adding roles and resolving polices
    // Todo Add full user support for Auditable items
    // Todo Add integration event emission using rabbit MQ
    // Todo add blazor client
    // Todo .net 6 simple endpoints
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}