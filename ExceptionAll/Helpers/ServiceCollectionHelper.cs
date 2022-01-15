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
        services.AddSingleton<IErrorResponseService, ErrorResponseService>();
        services.AddSingleton<IActionResultService, ActionResultService>();
        services.AddSingleton<IContextConfigurationService, ContextConfigurationService>();

        services.Scan(
            x => x.FromAssemblyOf<T>()
                  .AddClasses(c => c.AssignableTo<IExceptionAllConfiguration>())
                  .AsImplementedInterfaces()
                  .WithSingletonLifetime());

        var serviceProvider      = services.BuildServiceProvider();
        var actionResultService  = serviceProvider.GetRequiredService<IActionResultService>();
        var contextConfiguration = serviceProvider.GetRequiredService<IContextConfigurationService>();
        var configuration        = serviceProvider.GetRequiredService<IExceptionAllConfiguration>();
        //var errorResponseService = serviceProvider.GetRequiredService<IErrorResponseService>();

        //errorResponseService?.AddErrorResponses(configuration.ErrorResponses.ToList());

        if (configuration.ContextConfiguration is not null)
            contextConfiguration.Configure(configuration.ContextConfiguration);

        // Adds an exception filter to
        services.AddMvc(options => { options.Filters.Add<ExceptionFilter>(); });

        // Removes the default response from being returned on validation error
        services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new List<ErrorDetail>();
                    foreach (var (key, value) in context.ModelState)
                    {
                        errors.AddRange(value.Errors.Select(error => new ErrorDetail(key, error.ErrorMessage)));
                    }

                    return actionResultService.GetResponse<BadRequestDetails>(
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

    /// <summary>
    /// Applies the configuration provided by the user to the error response container
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddExceptionAll(this IServiceProvider services)
    {
        var contextConfiguration = services.GetService<IContextConfigurationService>();
        var errorResponseService = services.GetService<IErrorResponseService>();
        var configuration        = services.GetService<IExceptionAllConfiguration>();

        if (configuration is null || contextConfiguration is null) return;

        errorResponseService?.AddErrorResponses(configuration.ErrorResponses.ToList());

        if (configuration.ContextConfiguration is not null)
            contextConfiguration.Configure(configuration.ContextConfiguration);
    }

    /// <summary>
    /// Add all error responses into the response collection
    /// </summary>
    /// <param name="service"></param>
    /// <param name="errorResponses"></param>
    private static void AddErrorResponses(
        this IErrorResponseService service,
        List<IErrorResponse> errorResponses)
    {
        if (errorResponses == null || !errorResponses.Any())
            throw new ArgumentNullException(nameof(errorResponses));

        foreach (var errorResponse in errorResponses)
        {
            service.AddErrorResponse(errorResponse);
        }
    }
}