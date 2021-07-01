using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Filters;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace ExceptionAll.APIExample
{
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
            services.AddSingleton<IErrorResponseService, ErrorResponseService>();
            services.AddSingleton<IActionResultService, ActionResultService>();

            services.AddControllers(x =>
            {
                x.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionAll.APIExample", Version = "v1" });            
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IErrorResponseService errorResponseService, IActionResultService actionResultService)
        {
            errorResponseService.AddErrorResponse(new ErrorResponse
            {
                StatusCode = 400,
                ErrorTitle = "Bad Request",
                ExceptionType = typeof(ValidationException),
                DetailsType= typeof(BadRequestDetails),
                LogAction = (e) => actionResultService
                    .Logger
                    .LogDebug(e, e.Message)
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExceptionAll.APIExample v1"));

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