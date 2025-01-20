using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers
{
    public class LoginController : Controller
    {
        //Auth
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitLogin(string username, string password)
        {
            TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you shortly.";
            return RedirectToAction("Persons");
        }

        //Persons
        private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Id = 1, IdNumber = "123456789", FirstName = "John", LastName = "Doe", AccountNumber = "AC12345" },
        new PersonViewModel { Id = 2, IdNumber = "987654321", FirstName = "Jane", LastName = "Smith", AccountNumber = "AC54321" }
    };
        public IActionResult Persons()
        {
            return View(_persons);
        }
    }
}
