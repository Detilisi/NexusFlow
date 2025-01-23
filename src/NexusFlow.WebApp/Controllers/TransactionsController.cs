using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NexusFlow.WebApp.Models;
using System.Text;

namespace NexusFlow.WebApp.Controllers
{
    [Route("Login/Persons/Accounts/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7253/api/Transactions";

        public TransactionsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id = 0, int accountCode = 1)
        {
            if (id == 0)
            {
                // Return a blank transaction for creating a new one
                var newTransaction = new TransactionViewModel
                {
                    AccountCode = accountCode
                };
                return View(newTransaction);
            }

            // Fetch the transaction details from the API
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch transaction: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<TransactionViewModel>(jsonData);
            return View(transaction);
        }

        [HttpPost("SubmitSave")]
        public async Task<IActionResult> SubmitSave(TransactionViewModel transaction)
        {
            HttpResponseMessage response;

            if (transaction.Code == 0)
            {
                // Add a new transaction
                var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(_apiBaseUrl, content);
            }
            else
            {
                // Update an existing transaction
                var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");
                response = await _httpClient.PutAsync($"{_apiBaseUrl}/{transaction.Code}", content);
            }

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to save transaction: {response.ReasonPhrase}");
            }

            return RedirectToAction("Edit", "Accounts", new { id = transaction.AccountCode });
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(int accountCode)
        {
            // Fetch all transactions for the given account from the API
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/ByAccount/{accountCode}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error", $"Failed to fetch transactions: {response.ReasonPhrase}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<List<TransactionViewModel>>(jsonData);
            return View(transactions);
        }
    }
}
