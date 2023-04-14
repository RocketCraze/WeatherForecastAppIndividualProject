namespace WeatherForecastProject.Models
{
    public class WeatherForecastModel
    {
        public string ResolvedAddress { get; set; }

        public string Timezone { get; set; }

        public DailyForecastModel[] Days { get; set; }
    }
}
