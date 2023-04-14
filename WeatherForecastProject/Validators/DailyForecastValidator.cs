namespace WeatherForecastProject.Validators
{
    using FluentValidation;

    using WeatherForecastProject.Models;

    public class DailyForecastValidator : AbstractValidator<DailyForecast>
    {
        public DailyForecastValidator()
        {
            RuleFor(_ => _.DateTime)
                .MinimumLength(1)
                .NotNull();

            RuleFor(_ => _.TempMax)
                .NotNull();

            RuleFor(_ => _.TempMin)
                .NotNull();

            RuleFor(_ => _.PrecipProb)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .NotNull();

            RuleFor(_ => _.Humidity)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .NotNull();

            RuleFor(_ => _.Windspeed)
                .GreaterThanOrEqualTo(0)
                .NotNull();

            RuleFor(_ => _.Cloudcover)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .NotNull();

            RuleFor(_ => _.Sunrise)
                .MinimumLength(1)
                .NotNull();

            RuleFor(_ => _.Sunset)
                .MinimumLength(1)
                .NotNull();
        }
    }
}
