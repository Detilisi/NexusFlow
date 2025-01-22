using Microsoft.AspNetCore.Mvc;
using NexusFlow.WebApp.Models;

namespace NexusFlow.WebApp.Controllers
{
    [Route("Login/Persons/[controller]")]
    public class AccountsController : Controller
    {
        private AccountViewModel _currentAccount;

        public AccountsController()
        {
            _currentAccount = new() { PersonCode = 1, AccountNumber = "Acc45454", 
                Transactions = [new() { AccountCode=1, Amount=12, Description = "Hello world"} ] };
        }


        [HttpGet("Edit")]
        public IActionResult Edit(int id = 0, int personCode = 1)
        {
            _currentAccount.Code = id;
            _currentAccount.PersonCode = personCode;

            return View(_currentAccount);
        }

        [HttpPost("SubmitSave")]
        public IActionResult SubmitSave(AccountViewModel account)
        {
            if (account.Code == 0)
            {
                //Add
                
            }
            else
            {
                //Update
            }

            return RedirectToAction("Edit", "Persons", new{ id = _currentAccount.PersonCode });
        }
    }
}
