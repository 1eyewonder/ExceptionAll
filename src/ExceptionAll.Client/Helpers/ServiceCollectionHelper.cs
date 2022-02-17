namespace ExceptionAll.Client.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection AddExceptionAllClientServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ISerializerService, SerializerService>();
        return services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
    }
}