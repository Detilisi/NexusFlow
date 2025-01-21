using Microsoft.AspNetCore.Mvc;

namespace NexusFlow.WebApp.Controllers
{
    [Route("Login/Persons/[controller]")]
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
