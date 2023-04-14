namespace WeatherForecastProject.Models
{
    public class WeatherForecastModel
    {
        public string ResolvedAddress { get; set; }

        public string Timezone { get; set; }

        public DailyForecast[] Days { get; set; }
    }
}