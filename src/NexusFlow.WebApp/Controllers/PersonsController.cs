using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Code = 1, IdNumber = "123456789", Name = "John", Surname = "Doe" },
        new PersonViewModel { Code = 2, IdNumber = "987654321", Name = "Jane", Surname = "Smith", Accounts = [new() { Code = 1, AccountNumber = "asdasd" }] }
    };

    [HttpGet]
    public IActionResult Index(string searchTerm, string searchType)
    {
        var persons = _persons.AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchType))
        {
            switch (searchType)
            {
                case "ID Number":
                    persons = persons.Where(p => p.IdNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Surname":
                    persons = persons.Where(p => p.Surname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                    break;
                case "Account Number":
                    persons = persons.Where(p => p.Accounts.FirstOrDefault(a => a.AccountNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) != null);
                    break;
            }
        }

        return View(persons.ToList());
    }

    //CRUD
    [HttpGet("Delete")]
    public IActionResult Delete(int id)
    {
        var person = _persons.FirstOrDefault(p => p.Code == id);
        if (person == null) return NotFound();
        _persons.Remove(person);

        return RedirectToAction("Index");
    }

    [HttpGet("Details")]
    public IActionResult Details(int id = 0)
    {
        if (id == 0)
        {
            return View(new PersonViewModel());
        }

        var person = _persons.FirstOrDefault(p => p.Code == id);
        return View(person);
    }

    [HttpGet("Save")]
    public IActionResult Save(int? id)
    {
        if (id != null)
        {
            var person = _persons.FirstOrDefault(p => p.Code == id);
            return View(person);
        }
        return View();
    }

    [HttpPost("SubmitSave")]
    public IActionResult SubmitSave(PersonViewModel model)
    {
        if(model.Code == 0)
        {
            //Add
            _persons.Add(model);
        }
        else
        {
            //Update
            var person = _persons.FirstOrDefault(p => p.Code == model.Code);
            person.IdNumber = model.IdNumber;
            person.Name = model.Name;
            person.Surname = model.Surname;
        }

        return RedirectToAction("Index");
    }
}
