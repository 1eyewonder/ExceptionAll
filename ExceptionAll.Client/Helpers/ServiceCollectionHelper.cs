namespace ExceptionAll.Client.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection AddExceptionAllClientServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        return services.AddScoped<ISerializerService, SerializerService>();
    }

    public static IHttpClientBuilder AddExceptionAllClient(this IServiceCollection services)
    {
        services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
        return services.AddHttpClient<ExceptionAllClientFactory>();
    }

    public static IHttpClientBuilder AddExceptionAllClient(this IServiceCollection services, Action<HttpClient> configureClient)
    {
        services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
        return services.AddHttpClient<ExceptionAllClientFactory>(configureClient);
    }

    public static IHttpClientBuilder AddExceptionAllClient(this IServiceCollection services, string name, Action<HttpClient> configureClient)
    {
        services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
        return services.AddHttpClient<ExceptionAllClientFactory>(name, configureClient);
    }

    public static IHttpClientBuilder AddExceptionAllClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
        return services.AddHttpClient<ExceptionAllClientFactory>(configureClient);
    }

    public static IHttpClientBuilder AddExceptionAllClient(this IServiceCollection services, string name, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddScoped<IExceptionAllClientFactory, ExceptionAllClientFactory>();
        return services.AddHttpClient<ExceptionAllClientFactory>(name, configureClient);
    }
}