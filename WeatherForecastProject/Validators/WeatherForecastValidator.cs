namespace WeatherForecastProject.Validators
{
    using FluentValidation;

    using WeatherForecastProject.Models;

    public class WeatherForecastValidator : AbstractValidator<WeatherForecastModel>
    {
        public WeatherForecastValidator() 
        {
            RuleFor(_ => _.ResolvedAddress)
                .MinimumLength(1)
                .NotNull();

            RuleFor(_ => _.Timezone)
                .MinimumLength(1)
                .NotNull();
        }
    }
}
