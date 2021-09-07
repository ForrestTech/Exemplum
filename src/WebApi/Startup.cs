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

    // Todo Add mediatR for handlers 
    // Todo Add a simple request to get an item
    // Todo Add pagination
    // Todo Add controller endpoint to execute the query
    // Todo add Automapper for mapping to view model (include projection)
    // Todo Add ef core persistence for the items 
    // Todo Add query object support for central query logic (should query logic live in the domain or application)
    // Todo Add command for creation
    // Todo Add fluent validation
    // Todo Add application logic example test
    // Todo add transaction behavior so all handlers share a transaction
    // Todo Add support for Auditable items
    // Todo add support for Domain events
    // Todo add more write update and delete handlers and their domain events 
    // Todo persistence migrations and configurations
    // Todo Add a basic calendar view entity that is a read model over todos 
    // Todo demonstrate how multi aggregate updates work within a transaction for domains events
    // Todo add health checks
    // Todo add some custom health checks
    // Todo Authentications of users (ideally we would not looks to add a super specific solution)
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
