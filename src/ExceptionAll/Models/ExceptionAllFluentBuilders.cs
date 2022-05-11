namespace ExceptionAll;

public class ExceptionAllBuilder : IExceptionAllBuilder
{
    public IServiceCollection Services { get; }

    public ExceptionAllBuilder(IServiceCollection services)
    {
        Services = services;
    }
}