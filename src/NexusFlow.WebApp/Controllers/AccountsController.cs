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
            _currentAccount = new() { PersonCode = 1, AccountNumber = "Acc45454" };
        }


        [HttpGet("Details")]
        public IActionResult Details(int? id)
        {
            return View(_currentAccount);
        }

        [HttpGet("Save")]
        public IActionResult Save(int? id, int personCode)
        {
            if (id != null)
            {
            }

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

            return RedirectToAction("Details", "Persons", new{ id = _currentAccount.PersonCode });
        }
    }
}
