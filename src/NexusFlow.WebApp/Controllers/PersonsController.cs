using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private readonly HttpClient _httpClient;
    private List<PersonViewModel> _persons = new();
    public PersonsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchTerm, string searchType)
    {
        var apiUrl = "https://localhost:7253/api/Persons/GetAll";
        try
        {
            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch data from API: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            _persons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonViewModel>>(jsonData) ?? [];
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while calling the API: {ex.Message}");
        }


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

public class Person
{
    public int Code { get; set; } = 0;
    public string IdNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}

