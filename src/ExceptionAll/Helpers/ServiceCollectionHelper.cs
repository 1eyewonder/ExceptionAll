using System.Collections.Immutable;

namespace ExceptionAll;

public static class ServiceCollectionHelper
{
    /// <summary>
    ///     Inject all ExceptionAll related services into the IServiceCollection. Assembly scans for the class implementation
    ///     of IExceptionAllConfiguration
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IExceptionAllBuilder AddExceptionAll<T>(this IServiceCollection services)
        where T : class, IExceptionAllConfiguration
    {
        services.AddSingleton<IExceptionAllConfiguration, T>();
        services.AddSingleton<IContextConfigurationService, ContextConfigurationService>();
        services.AddSingleton<IErrorResponseService, ErrorResponseService>();
        services.AddSingleton<IActionResultService, ActionResultService>();
        services.AddSingleton<IResultService, ResultService>();
        services.AddSingleton<IApiErrorDetailsService, ApiErrorDetailsService>();
        services.AddScoped<ExceptionFilter>();

        return new ExceptionAllBuilder(services);
    }

    /// <summary>
    ///     Removes the default response from being returned on validation error
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IExceptionAllBuilder WithValidationOverride(this IExceptionAllBuilder builder)
    {
        builder.Services.AddOptions<ApiBehaviorOptions>()
               .Configure<IActionResultService, ILoggerFactory, IExceptionAllConfiguration>(
                   (options, ars, loggerFactory, config) =>
                   {
                       var logger = loggerFactory.CreateLogger("Program");
                       options.InvalidModelStateResponseFactory = context =>
                       {
                           var errors = new List<ErrorDetail>();
                           foreach (var (key, value) in context.ModelState)
                               errors.AddRange(
                                   value.Errors.Select(error => new ErrorDetail(key, error.ErrorMessage))
                               );

                           config.ValidationLoggingAction?.Invoke(logger, context);

                           return ars.GetResponse<BadRequestDetails>(
                               context,
                               "Invalid request model",
                               errors.Any() ? errors : null
                           );
                       };
                   }
               );

        return builder;
    }

    /// <summary>
    ///     Adds a global MVC filter to your controllers to exception handle
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IExceptionAllBuilder WithGlobalExceptionFilter(this IExceptionAllBuilder builder)
    {
        builder.Services.AddControllers(x => x.Filters.Add<ExceptionFilter>());

        return builder;
    }

    /// <summary>
    ///     Adds ExceptionAll's out of the box, default Swagger example providers
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <remarks>
    ///     Only MVC currently supports Swashbuckle filter examples. Can update once further
    ///     adjustments are made to .NET and/or Swashbuckle
    /// </remarks>
    public static IExceptionAllBuilder WithExceptionAllSwaggerExamples(this IExceptionAllBuilder builder)
    {
        builder.Services.AddSwaggerExamplesFromAssemblyOf<BadRequestDetailsExample>();

        return builder;
    }
}