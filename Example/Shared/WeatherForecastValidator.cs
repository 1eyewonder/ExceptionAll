using FluentValidation;

namespace Example.Shared;

public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
    public WeatherForecastValidator()
    {
        RuleFor(x => x.Summary)
            .NotEmpty()
            .NotNull();
    }
}