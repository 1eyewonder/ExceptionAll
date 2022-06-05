using Example.Server;
using Example.Shared;
using ExceptionAll;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddExceptionAll<ExceptionAllConfiguration>()
       .WithValidationOverride()
       .WithExceptionAllSwaggerExamples();

builder.Services.AddControllers()
       .AddFluentValidation(
           x => { x.DisableDataAnnotationsValidation = true; }
       );

builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<WeatherForecastValidator>());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionAll.APIExample", Version = "v1" });
        c.EnableAnnotations();
        c.ExampleFilters();

        if (File.Exists("/ExceptionAll-swagger.xml")) c.IncludeXmlComments("/ExceptionAll-swagger.xml");
    }
);

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(
    c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExceptionAll.APIExample v1");
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
    }
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { app.UseWebAssemblyDebugging(); }
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();