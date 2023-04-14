namespace WeatherForecastProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Mvc;

    using WeatherForecastProject.Models;

    [Route("api/[controller]")]
    public class WeatherForecastWebAPIController : Controller
    {
        private readonly IValidator<WeatherForecastModel> validator;
        private readonly IValidator<DailyForecast> dailyValidator;
        private readonly IHttpClientFactory clientFactory;

        public WeatherForecastWebAPIController(
            IValidator<WeatherForecastModel> validator,
            IValidator<DailyForecast> dailyValidator,
            IHttpClientFactory clientFactory)
        {
            this.validator = validator;
            this.dailyValidator = dailyValidator;
            this.clientFactory = clientFactory;
        }

        [HttpGet("/LoadWeatherForecast")]
        public async Task<object> LoadWeatherForecast(DataSourceLoadOptions loadOptions)
        {
            var forecast = new WeatherForecastModel();
            string errorString;

            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Port%20Louis?unitGroup=metric&include=days&key=5UDR5STWCM6G5PS6LQ9QSBBLT&contentType=json");

            var client = clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                forecast = await response.Content.ReadFromJsonAsync<WeatherForecastModel>();
                errorString = null;

                foreach (var day in forecast.Days)
                {
                    var dailyValidatorResult = dailyValidator.Validate(day);
                    if (!dailyValidatorResult.IsValid)
                    {
                        dailyValidatorResult.AddToModelState(ModelState);
                        return BadRequest(ModelState.Values.SelectMany(_ => _.Errors.Select(error => error.ErrorMessage)));
                    }
                }                

                var validatorResult = validator.Validate(forecast);
                if (!validatorResult.IsValid)
                {
                    validatorResult.AddToModelState(ModelState);
                    return BadRequest(ModelState.Values.SelectMany(_ => _.Errors.Select(error => error.ErrorMessage)));
                }

                object result = await Task.Run(() => DataSourceLoader.Load(forecast.Days, loadOptions));

                return result;
            }
            else
            {
                errorString = $"There was an error getting the forecast: {response.ReasonPhrase}";
                return BadRequest(errorString);
            } 
        }
    }
}
