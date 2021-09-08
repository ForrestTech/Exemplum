using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebApi
{
    using Application;
    using Infrastructure;

    // Todo Add command for creation
    // Todo Add fluent validation (add problem details)
    // Todo add domain function examples like mark as done
    // Todo Add full database setup
    // Todo add transaction behavior so all handlers share a transaction
    // Todo Add support for Auditable items
    // Todo add support for Domain events
    // Todo Add support for injectable dates
    // Todo add more write update and delete handlers and their domain events 
    // Todo persistence migrations and configurations
    // Todo add handling for mapping database errors to real errors 
    // Todo Add a basic calendar view entity that is a read model over todos 
    // Todo demonstrate how multi aggregate updates work within a transaction for domains events
    // Todo add health checks
    // Todo add some custom health checks
    // Todo add github builds
    // Todo add github tests
    // Todo add github dependency checks, code coverage and quality analysis, check for any hint paths
    // Todo Authentications of users (ideally we would not looks to add a super specific solution)
    // Todo add authorization including adding roles and resolving polices
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
            
            services.AddControllers();
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
