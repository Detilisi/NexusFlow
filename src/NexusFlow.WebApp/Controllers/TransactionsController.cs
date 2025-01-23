using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers
{
    [Route("Login/Persons/Accounts/[controller]")]
    public class TransactionsController : Controller
    {
        private TransactionViewModel _currentTransaction = new();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int id = 0, int accountCode = 1)
        {
            _currentTransaction.Code = id;
            _currentTransaction.AccountCode = accountCode;

            return View(_currentTransaction);
        }

        [HttpPost("SubmitSave")]
        public IActionResult SubmitSave(TransactionViewModel transaction)
        {
            if (transaction.Code == 0)
            {
                //Add

            }
            else
            {
                //Update
            }

            return RedirectToAction("Edit", "Accounts", new { id = _currentTransaction.AccountCode });
        }
    }
}
