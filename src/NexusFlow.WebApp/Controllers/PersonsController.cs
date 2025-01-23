using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private readonly HttpClient _httpClient;
    public PersonsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    private static List<PersonViewModel> _persons = new()
    {
        new PersonViewModel { Code = 1, IdNumber = "123456789", Name = "John", Surname = "Doe" },
        new PersonViewModel { Code = 2, IdNumber = "987654321", Name = "Jane", Surname = "Smith", Accounts = [new() { Code = 1, AccountNumber = "asdasd" }] }
    };

    [HttpGet]
    public async Task<IActionResult> Index(string searchTerm, string searchType)
    {
        var response = await _httpClient.GetAsync("https://localhost:7253/api/Persons/GetAll");
        var jsonData = await response.Content.ReadAsStringAsync();
        var persons = System.Text.Json.JsonSerializer.Deserialize<List<PersonModel>>(jsonData);

        var filteredPersons = FilterBySearch(searchTerm, searchType, _persons);
        return View(filteredPersons);
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

    [HttpGet("Edit")]
    public IActionResult Edit(int id = 0)
    {
        if (id == 0)
        {
            return View(new PersonViewModel());
        }

        var person = _persons.FirstOrDefault(p => p.Code == id);
        return View(person);
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

    //Utility function
    private List<PersonViewModel> FilterBySearch(string searchTerm, string searchType, List<PersonViewModel> personsList)
    {
        var persons = personsList.AsQueryable();
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

        return persons.ToList();
    }
}

public class PersonModel
{
    public int Code { get; set; }
    public string Name { get; set; }
}

