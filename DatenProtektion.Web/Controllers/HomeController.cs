using DatenProtektion.Web.Filter;
using DatenProtektion.Web.Modelle;
using DatenProtektion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DatenProtektion.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Response.Cookies.Append("e-mail", "admin@gmail.com");
            HttpContext.Response.Cookies.Append("passwort", "admin123");

            return View();
        }

        [HttpPost]
        public IActionResult Index(string name, int nummer)
        {
            ViewBag.name = name;
            ViewBag.nummer = nummer;

            return View();
        }

        [ServiceFilter(typeof(CheckWhiteList))]

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}