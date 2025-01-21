﻿using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Code = 1, IdNumber = "123456789", Name = "John", Surname = "Doe" },
        new PersonViewModel { Code = 2, IdNumber = "987654321", Name = "Jane", Surname = "Smith" }
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
        var person = _persons.FirstOrDefault(p => p.Code == id);
        if (person == null) return NotFound();
        _persons.Remove(person);

        return RedirectToAction("Index");
    }

    [HttpGet("Detials")]
    public IActionResult Detials(int id)
    {
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

    // Handle Save Submission
    [HttpPost("Save")]
    public IActionResult Save(PersonViewModel model)
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
