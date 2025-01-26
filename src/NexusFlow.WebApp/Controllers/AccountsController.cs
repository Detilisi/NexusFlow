using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NexusFlow.WebApp.Models;
using System.Text;

namespace NexusFlow.WebApp.Controllers
{
    [Route("Login/Persons/[controller]")]
    public class AccountsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7253/api/Accounts";

        public AccountsController(HttpClient httpClient) => _httpClient = httpClient;

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id = 0, int personCode = 1)
        {
            AccountViewModel account;

            if (id == 0)
            {
                // Create a new account
                account = new AccountViewModel
                {
                    PersonCode = personCode
                };
            }
            else
            {
                // Fetch the account from the API
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error", $"Failed to fetch account data from API: {response.ReasonPhrase}");
                }

                var jsonData = await response.Content.ReadAsStringAsync();
                var accounts = JsonConvert.DeserializeObject<List<AccountViewModel>>(jsonData);
                if (accounts == null)
                {
                    return View("Error", $"Failed to deserialize account data from API: {response.ReasonPhrase}");
                }

                account = accounts.FirstOrDefault();
                await PopulateWithTransactions(account);
            }

            return View(account);
        }

        [HttpPost("SubmitSave")]
        public async Task<IActionResult> SubmitSave(AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", account);
            }

            if (account.Code == 0)
            {
                // Add new account via API
                var jsonContent = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiBaseUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error", $"Failed to create account: {response.ReasonPhrase}");
                }
            }
            else
            {
                // Update existing account via API
                var jsonContent = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{account.Code}", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error", $"Failed to update account: {response.ReasonPhrase}");
                }
            }

            return RedirectToAction("Edit", "Persons", new { id = account.PersonCode });
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id, int personCode)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to delete account: {response.ReasonPhrase}");
            }

            return RedirectToAction("Edit", "Persons", new { id = personCode });
        }

        // Utility function
        private async Task PopulateWithTransactions(AccountViewModel account)
        {
            var transactionsApiUrl = _apiBaseUrl.Replace("Accounts", "Transactions");
            var response = await _httpClient.GetAsync($"{transactionsApiUrl}/{account.Code}/-1");
            if (!response.IsSuccessStatusCode) return;

            var jsonData = await response.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<List<TransactionViewModel>>(jsonData) ?? new List<TransactionViewModel>();
            account.Transactions = transactions;
        }
    }
}
