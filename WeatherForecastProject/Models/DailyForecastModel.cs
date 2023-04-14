namespace WeatherForecastProject.Models
{
    public class DailyForecastModel
    {
        public string DateTime { get; set; }

        public float TempMax { get; set; }

        public float TempMin { get; set; }

        public float PrecipProb { get; set; }

        public float Windspeed { get; set; }

        public float Cloudcover { get; set; }

        public string Sunrise { get; set; }

        public string Sunset { get; set; }
    }
}
