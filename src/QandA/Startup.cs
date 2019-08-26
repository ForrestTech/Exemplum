using System;
using System.Net.Http;
using System.Reflection;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using QandA.Data;
using Serilog;

namespace QandA
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IHostingEnvironment HostingEnvironment { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment environment)
		{
			Configuration = configuration;
			HostingEnvironment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddProblemDetails(ConfigureProblemDetails);

			services.AddMvc()
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				}).AddFluentValidation(options =>
				{
					options.RegisterValidatorsFromAssemblyContaining<Startup>();
				});


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Instance = context.HttpContext.Request.Path,
						Status = StatusCodes.Status400BadRequest,
						Type = "/swagger",
						Detail = "Please refer to the errors property for additional details."
					};
					return new BadRequestObjectResult(problemDetails)
					{
						ContentTypes = { "application/problem+json", "application/problem+xml" }
					};
				};
			});

			services.AddMediatR(Assembly.GetAssembly(typeof(Startup)));

			services.AddDbContext<DatabaseContext>
				(options => options.UseSqlServer(Configuration.GetConnectionString("QandADatabase")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSerilogRequestLogging();

			app.UseProblemDetails();

			app.UseMvc();
		}

		private void ConfigureProblemDetails(ProblemDetailsOptions options)
		{
			// This is the default behavior; only include exception details in a development environment.
			options.IncludeExceptionDetails = ctx => HostingEnvironment.IsDevelopment();

			// This will map NotImplementedException to the 501 Not Implemented status code.
			options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));

			// This will map HttpRequestException to the 503 Service Unavailable status code.
			options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));

			// Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
			// If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
			options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
		}
	}
}
