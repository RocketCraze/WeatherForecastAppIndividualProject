using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastProject.Models;

namespace WeatherForecastProject.Controllers
{
    public class WeatherForecastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
