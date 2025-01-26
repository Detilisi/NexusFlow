using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace NexusFlow.WebApp.Controllers;

public class LoginController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "https://localhost:7253/api/Users";

    public LoginController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> SubmitLogin(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Username and password are required.");
            return View("Index");
        }

        // Encode credentials in Base64
        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        // Set up the Authorization header
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/validate-user");

            if (response.IsSuccessStatusCode)
            {
                // Login successful
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Persons"); // Redirect to PersonsController
            }
            else
            {
                // Login failed
                ModelState.AddModelError("", "Invalid username or password.");
                return View("Index");
            }
        }
        catch (Exception ex)
        {
            // Handle errors
            ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            return View("Index");
        }
    }
}
