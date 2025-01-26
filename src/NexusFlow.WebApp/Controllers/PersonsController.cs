using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "https://localhost:7253/api/Persons";

    public PersonsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchTerm, string searchType)
    {
        List<PersonViewModel> persons = new();

        try
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch data from API: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            persons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonViewModel>>(jsonData) ?? new List<PersonViewModel>();

            await PopulateListWithAccounts(persons);
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while calling the API: {ex.Message}");
        }

        var filteredPersons = await FilterBySearch(searchTerm, searchType, persons);
        return View(filteredPersons);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to delete person: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while deleting the person: {ex.Message}");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("Edit/{id?}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (!id.HasValue || id == 0)
        {
            // Create new person
            return View(new PersonViewModel());
        }

        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch person details: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var person = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonViewModel>(jsonData);
            if (person == null)
            {
                return View("Error", $"Person with ID {id} not found.");
            } 
            
            await PopulateWithAccounts(person);
            return View(person);
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while fetching person details: {ex.Message}");
        }
    }

    [HttpPost("SubmitSave")]
    public async Task<IActionResult> SubmitSave(PersonViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        try
        {
            HttpResponseMessage response;

            if (model.Code == 0)
            {
                // Add new person
                response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, model);
            }
            else
            {
                // Update existing person
                response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{model.Code}", model);
            }

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to save person: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while saving the person: {ex.Message}");
        }

        return RedirectToAction("Index");
    }

    // Utility function
    private async Task PopulateWithAccounts(PersonViewModel person)
    {
        var accountUrl = _apiBaseUrl.Replace("Persons", "Accounts");
        var response = await _httpClient.GetAsync($"{accountUrl}/{person.Code}/-1");
        if (!response.IsSuccessStatusCode) return;
        
        var jsonData = await response.Content.ReadAsStringAsync();
        var accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountViewModel>>(jsonData) ?? new List<AccountViewModel>();
        person.Accounts = accounts;
    }

    private async Task PopulateListWithAccounts(List<PersonViewModel> personsList)
    {

        foreach (var person in personsList)
        {
            await PopulateWithAccounts(person);
        }
    }

    private async Task<List<PersonViewModel>> FilterBySearch(string searchTerm, string searchType, List<PersonViewModel> personsList)
    {
        if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(searchType)) return personsList;

        var searchUrl = $"{_apiBaseUrl}/{searchTerm}/{searchType}";
        var response = await _httpClient.GetAsync(searchUrl);
        if (!response.IsSuccessStatusCode) return personsList;

        var jsonData = await response.Content.ReadAsStringAsync();
        var persons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonViewModel>>(jsonData) ?? new List<PersonViewModel>();

        return persons.ToList();
    }
}

