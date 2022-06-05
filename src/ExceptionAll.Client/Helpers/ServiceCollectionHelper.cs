namespace ExceptionAll.Client;

public static class ServiceCollectionHelper
{
    public static IServiceCollection AddExceptionAllClientServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        return services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
    }
}