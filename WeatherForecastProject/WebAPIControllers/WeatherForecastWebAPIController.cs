namespace WeatherForecastProject.WebAPIControllers
{
    using DevExtreme.AspNet.Data;
    using DevExtreme.AspNet.Mvc;
    using FluentValidation;

    using Microsoft.AspNetCore.Mvc;

    using WeatherForecastProject.Models;

    [Route("api/[controller]")]
    public class WeatherForecastWebAPIController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public WeatherForecastWebAPIController(
            IHttpClientFactory clientFactory)
        {
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
