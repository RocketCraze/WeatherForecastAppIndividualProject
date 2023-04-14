namespace WeatherForecastProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;

    using FluentValidation;
    using FluentValidation.AspNetCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using WeatherForecastProject.Models;

    [Route("api/[controller]")]
    public class WeatherForecastWebAPIController : Controller
    {
        private readonly IValidator<WeatherForecastModel> validator;
        private readonly IValidator<DailyForecast> dailyValidator;
        private readonly IHttpClientFactory clientFactory;
        private readonly IMemoryCache memoryCache;

        public WeatherForecastWebAPIController(
            IValidator<WeatherForecastModel> validator,
            IValidator<DailyForecast> dailyValidator,
            IHttpClientFactory clientFactory,
            IMemoryCache memoryCache)
        {
            this.validator = validator;
            this.dailyValidator = dailyValidator;
            this.clientFactory = clientFactory;
            this.memoryCache = memoryCache;
        }

        string errorString;

        [HttpGet("/LoadWeatherForecast")]
        public async Task<object> LoadWeatherForecast(DataSourceLoadOptions loadOptions)
        {
            var forecast = new WeatherForecastModel();

            var client = clientFactory.CreateClient();

            forecast = memoryCache.Get<WeatherForecastModel>("weather");

            if (forecast == null)
            {
                try
                {
                    forecast = await client.GetFromJsonAsync<WeatherForecastModel>("https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Port%20Louis?unitGroup=metric&include=days&key=5UDR5STWCM6G5PS6LQ9QSBBLT&contentType=json");
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

                }
                catch (Exception ex) 
                {
                    errorString = $"There was an error getting the forecast: { ex.Message }";
                    return BadRequest(errorString);
                }
                
                memoryCache.Set("weather", forecast, TimeSpan.FromMinutes(1));
            }

            object result = await Task.Run(() => DataSourceLoader.Load(forecast.Days, loadOptions));

            return result;

            //var request = new HttpRequestMessage(HttpMethod.Get,
            //    "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Port%20Louis?unitGroup=metric&include=days&key=5UDR5STWCM6G5PS6LQ9QSBBLT&contentType=json");



            //HttpResponseMessage response = await client.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //    forecast = await response.Content.ReadFromJsonAsync<WeatherForecastModel>();

            //    errorString = null;

            //    foreach (var day in forecast.Days)
            //    {
            //        var dailyValidatorResult = dailyValidator.Validate(day);
            //        if (!dailyValidatorResult.IsValid)
            //        {
            //            dailyValidatorResult.AddToModelState(ModelState);
            //            return BadRequest(ModelState.Values.SelectMany(_ => _.Errors.Select(error => error.ErrorMessage)));
            //        }
            //    }                

            //    var validatorResult = validator.Validate(forecast);
            //    if (!validatorResult.IsValid)
            //    {
            //        validatorResult.AddToModelState(ModelState);
            //        return BadRequest(ModelState.Values.SelectMany(_ => _.Errors.Select(error => error.ErrorMessage)));
            //    }
        }
    }
}
