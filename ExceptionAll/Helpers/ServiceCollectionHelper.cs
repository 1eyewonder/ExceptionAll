namespace ExceptionAll.Helpers;

public static class ServiceCollectionHelper
{
    /// <summary>
    /// Inject all ExceptionAll related services into the IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddExceptionAll<T>(this IServiceCollection services) where T : class, IExceptionAllConfiguration
    {
        services.AddSingleton<IActionResultService, ActionResultService>();

        services.Scan(
            x => x.FromAssemblyOf<T>()
                  .AddClasses(c => c.AssignableTo<IExceptionAllConfiguration>())
                  .AsImplementedInterfaces()
                  .WithSingletonLifetime());

        services.AddSingleton<IContextConfigurationService, ContextConfigurationService>();
        services.AddSingleton<IErrorResponseService, ErrorResponseService>();

        // Adds an exception filter to
        services.AddMvc(options => { options.Filters.Add<ExceptionFilter>(); });

        // Removes the default response from being returned on validation error
        services.AddOptions<ApiBehaviorOptions>()
            .Configure<IActionResultService>((options, ars) =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new List<ErrorDetail>();
                    foreach (var (key, value) in context.ModelState)
                    {
                        errors.AddRange(value.Errors.Select(error => new ErrorDetail(key, error.ErrorMessage)));
                    }

                    return ars.GetResponse<BadRequestDetails>(
                        context,
                        "Invalid request model",
                        errors.Any() ? errors : null);
                };
            });

        return services;
    }

    /// <summary>
    /// Assembly scans for the class implementation of IExceptionAllConfiguration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection WithConfigurationInAssemblyOf<T>(this IServiceCollection services) where T : class
    {
        return services.Scan(
            x => x.FromAssemblyOf<T>()
                  .AddClasses(c => c.AssignableTo<IExceptionAllConfiguration>())
                  .AsImplementedInterfaces()
                  .WithSingletonLifetime());
    }

    /// <summary>
    /// Adds ExceptionAll's out of the box, default Swagger example providers
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection WithExceptionAllSwaggerExamples(this IServiceCollection services)
    {
        return services.AddSwaggerExamplesFromAssemblyOf<BadRequestDetailsExample>();
    }
}