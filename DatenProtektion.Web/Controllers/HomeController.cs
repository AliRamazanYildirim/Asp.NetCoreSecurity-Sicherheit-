using DatenProtektion.Web.Filter;
using DatenProtektion.Web.Modelle;
using DatenProtektion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Encodings.Web;

namespace DatenProtektion.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly JavaScriptEncoder _scriptEncoder;
        private readonly UrlEncoder _urlEncoder;

        public HomeController(ILogger<HomeController> logger, HtmlEncoder htmlEncoder, JavaScriptEncoder scriptEncoder, UrlEncoder urlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
            _scriptEncoder = scriptEncoder;
            _urlEncoder = urlEncoder;
        }

        public IActionResult Index()
        {
            HttpContext.Response.Cookies.Append("e-mail", "admin@gmail.com");
            HttpContext.Response.Cookies.Append("passwort", "admin123");

            if(System.IO.File.Exists("info.txt"))
            {
                ViewBag.namen = System.IO.File.ReadAllLines("info.txt");
            }

            return View();
        }

        public IActionResult Anmelden(string? returnUrl="/")
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Anmelden(string email, string passwort)
        {
            #region Ohne Redirect-Check 
            //string returnUrl = TempData["returnUrl"]?.ToString() ?? "/";
            //return Redirect(returnUrl);
            #endregion

            #region Mit Redirect-Check 
            string returnUrl = TempData["returnUrl"]?.ToString() ?? "/";

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/");
            } 
            #endregion

        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult Index(string name, int nummer)
        {
            string encodedHtml = _htmlEncoder.Encode(name);

            string encodedScript = _scriptEncoder.Encode(name);

            string encodedUrl = _urlEncoder.Encode(name);


            ViewBag.name = name;
            ViewBag.nummer = nummer;
            System.IO.File.AppendAllText("info.txt", $"{name}-{nummer}\n");

            return RedirectToAction("Index");
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