using ExceptionAll.Details;
using ExceptionAll.Dtos;
using ExceptionAll.Filters;
using ExceptionAll.Helpers;
using ExceptionAll.Interfaces;
using ExceptionAll.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

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
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionAll.APIExample", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IErrorResponseService errorResponseService,
            IActionResultService actionResultService)
        {
            errorResponseService.AddErrorResponse(
                ErrorResponse
                    .CreateErrorResponse(actionResultService)
                    .WithTitle("Bad Request - Fluent Validation")
                    .ForException(typeof(FluentValidation.ValidationException))
                    .WithReturnType(typeof(BadRequestDetails))
                    .WithLogAction((x, e) => x.LogError("Something bad happened", e))
            );            

            // Adds CorrelationId to incoming requests for tracking. Optional
            app.UseCorrelationIdMiddleware();

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