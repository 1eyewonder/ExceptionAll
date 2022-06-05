using Example.Shared;
using ExceptionAll;
using ExceptionAll.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalApiExample.Api;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExceptionAll.MinimalApiExample", Version = "v1" });
        c.EnableAnnotations();
        //c.ExampleFilters(); filters not currently supported outside of mvc pipeline in .NET 6
    }
);

builder.Services.AddExceptionAll<ExceptionAllConfiguration>();

builder.Services.AddValidatorsFromAssemblyContaining<WeatherForecastValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(new SwaggerOptions());
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExceptionAll.MinimalApiExample v1");
            c.DisplayRequestDuration();
            c.EnableTryItOutByDefault();
        }
    );
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(
       "api/success", () =>
       {
           return Enumerable.Range(1, 5)
                            .Select(
                                index => new WeatherForecast
                                {
                                    Date         = DateTime.Now.AddDays(index),
                                    TemperatureC = Random.Shared.Next(-20, 55),
                                    Summary      = summaries[Random.Shared.Next(summaries.Length)]
                                }
                            )
                            .ToList();
       }
   )
   .Produces<InternalServerErrorDetails>(500);

app.MapGet(
       "api/", () =>
       {
           var rng = new Random();
           var result = Enumerable.Range(1, 5)
                                  .Select(
                                      index => new WeatherForecast
                                      {
                                          Date         = DateTime.Now.AddDays(index),
                                          TemperatureC = rng.Next(-20, 55),
                                          Summary      = summaries[rng.Next(summaries.Length)]
                                      }
                                  )
                                  .ToList();

           throw new Exception("This is simulating an uncaught exception");

           return result;
       }
   )
   .Produces<ApiErrorDetails>(500);

app.MapGet(
    "api/GetNullRefError", ([FromQuery] string param, [FromQuery] string otherParam) =>
    {
        param = null;

        throw new ArgumentNullException(nameof(param));

        return Results.Ok(param);
    }
);

app.MapGet(
    "api/GetWithoutExceptionAllError", () =>
    {
        try { throw new Exception("Some exception"); }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return Results.BadRequest(e.Message);
        }
    }
);

app.MapGet(
       "api/GetSomething",
       (HttpContext context, [FromServices] IResultService resultService, [FromQuery] string test) =>
       {
           var errors = new List<ErrorDetail>
           {
               new("Error #1", "Something wrong happened here"),
               new("Error #2", "Something wrong happened there")
           };

           return resultService.GetResponse<NotFoundDetails>(
               context,
               $"No item exists with name of {test}",
               errors
           );
       }
   )
   .Produces<BadRequestDetails>(400)
   .Produces<NotFoundDetails>(404)
   .Produces<NotFoundDetails>(500);

app.MapPost(
       "api/AddForecast", async (HttpContext context,
                                 [FromBody] WeatherForecast forecast,
                                 [FromServices] IValidator<WeatherForecast> validator,
                                 [FromServices] IResultService resultService) =>
       {
           var validationResult = validator.Validate(forecast);

           if (validationResult.IsValid) return Results.Created(string.Empty, forecast);
           var errors = validationResult.Errors.Select(x => new ErrorDetail($"Property name: '{x.PropertyName}'", $"Reason for failure: {x.ErrorMessage}"));

           return resultService.GetResponse<BadRequestDetails>(context, "Model provided is invalid", errors.ToList());
       }
   )
   .Produces<BadRequestDetails>(400)
   .Produces<NotFoundDetails>(500);

app.UseMiddleware<ExceptionAllMiddleware>();
app.UseHttpsRedirection();

app.Run();