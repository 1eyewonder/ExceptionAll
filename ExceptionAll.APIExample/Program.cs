using ExceptionAll.APIExample;
using ExceptionAll.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddExceptionAll<ExceptionAllConfiguration>()
       .WithExceptionAllSwaggerExamples();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionAll.APIExample", Version = "v1" });
        c.EnableAnnotations();
        c.ExampleFilters();

        if (File.Exists("/ExceptionAll-swagger.xml"))
        {
            c.IncludeXmlComments("/ExceptionAll-swagger.xml");
        }
    });

var app = builder.Build();

app.Services.AddExceptionAll();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(
    c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExceptionAll.APIExample v1");
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
    });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();