using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QandA.Data;
using Serilog;

namespace QandA
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.AddFeatureFolders();

			services.AddMediatR(Assembly.GetAssembly(typeof(Startup)));

			services.AddDbContext<QandAContext>
				(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Exemplum.QandA;Trusted_Connection=True;ConnectRetryCount=0"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSerilogRequestLogging();

			app.UseMvc();
		}
	}
}
