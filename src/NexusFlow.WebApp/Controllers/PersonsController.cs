using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Home/[controller]")]
public class PersonsController : Controller
{
    private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Id = 1, IdNumber = "123456789", FirstName = "John", LastName = "Doe", AccountNumber = "AC12345" },
        new PersonViewModel { Id = 2, IdNumber = "987654321", FirstName = "Jane", LastName = "Smith", AccountNumber = "AC54321" }
    };

    public IActionResult Index()
    {
        return View(_persons);
    }
}
