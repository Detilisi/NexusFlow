using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*[HttpPost]
        public ActionResult SendMessage(string name, string email, string message)
        {
            // Logic to handle the message (e.g., save to database, send email, etc.)

            // Optionally, you can add a success message or redirect
            TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you shortly.";
            return RedirectToAction("Contact");
        }*/
    }
}
