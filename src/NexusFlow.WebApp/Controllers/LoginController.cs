using Microsoft.AspNetCore.Mvc;

namespace NexusFlow.WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitLogin(string username, string password)
        {
            TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you shortly.";
            return RedirectToAction("Contact");
        }
    }
}
