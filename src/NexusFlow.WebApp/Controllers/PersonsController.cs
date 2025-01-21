using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Id = 1, IdNumber = "123456789", FirstName = "John", LastName = "Doe", AccountNumber = "AC12345" },
        new PersonViewModel { Id = 2, IdNumber = "987654321", FirstName = "Jane", LastName = "Smith", AccountNumber = "AC54321" }
    };

    [HttpGet]
    public IActionResult Index()
    {
        return View(_persons);
    }

    //CRUD
    [HttpGet("Delete")]
    public IActionResult Delete(int id)
    {
        var person = _persons.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();
        _persons.Remove(person);

        return RedirectToAction("Index");
    }

    [HttpGet("Detials")]
    public IActionResult Detials(int id)
    {
        var person = _persons.FirstOrDefault(p => p.Id == id);
        return View(person);
    }

    [HttpGet("Save")]
    public IActionResult Save(int? id)
    {
        if (id != null)
        {
            var person = _persons.FirstOrDefault(p => p.Id == id);
            return View(person);
        }
        return View();
    }

    // Handle Save Submission
    [HttpPost("Save")]
    public IActionResult Save(PersonViewModel model)
    {
        if(model.Id == 0)
        {
            //Add
            _persons.Add(model);
        }
        else
        {
            //Update
            var person = _persons.FirstOrDefault(p => p.Id == model.Id);
            person.IdNumber = model.IdNumber;
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.AccountNumber = model.AccountNumber;
        }

        return RedirectToAction("Index");
    }
}
