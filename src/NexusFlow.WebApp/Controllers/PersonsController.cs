using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers;

[Route("Login/[controller]")]
public class PersonsController : Controller
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "https://localhost:7253/api/Persons";

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
            var response = await _httpClient.GetAsync(ApiBaseUrl);
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch data from API: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            persons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonViewModel>>(jsonData) ?? new List<PersonViewModel>();
        }
        catch (Exception ex)
        {
            return View("Error", $"An error occurred while calling the API: {ex.Message}");
        }

        var filteredPersons = FilterBySearch(searchTerm, searchType, persons);
        return View(filteredPersons);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{id}");
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
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch person details: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var person = Newtonsoft.Json.JsonConvert.DeserializeObject<PersonViewModel>(jsonData);
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
                response = await _httpClient.PostAsJsonAsync(ApiBaseUrl, model);
            }
            else
            {
                // Update existing person
                response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/{model.Code}", model);
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
            }
        }

        return persons.ToList();
    }
}

