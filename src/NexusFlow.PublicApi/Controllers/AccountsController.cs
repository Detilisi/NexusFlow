using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusFlow.PublicApi.Models;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // Simulated in-memory data store
        private static List<Account> _accounts = new List<Account>
        {
            new Account { Code = 1, PersonCode = 1, AccountNumber = "AC12345", OutStandingBalance = 100.50m, Status = AccountStatus.Open },
            new Account { Code = 2, PersonCode = 2, AccountNumber = "AC54321", OutStandingBalance = 250.75m, Status = AccountStatus.Open }
        };

        // GET: api/Accounts
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            return Ok(_accounts);
        }

        // GET: api/Accounts/{id}
        [HttpGet("{id}")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Code == id);
            if (account == null)
            {
                return NotFound($"Account with Code {id} not found.");
            }
            return Ok(account);
        }

        // POST: api/Accounts
        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account newAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            newAccount.Code = _accounts.Max(a => a.Code) + 1; // Auto-increment Code
            _accounts.Add(newAccount);

            return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.Code }, newAccount);
        }

        // PUT: api/Accounts/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] Account updatedAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = _accounts.FirstOrDefault(a => a.Code == id);
            if (account == null)
            {
                return NotFound($"Account with Code {id} not found.");
            }

            account.PersonCode = updatedAccount.PersonCode;
            account.AccountNumber = updatedAccount.AccountNumber;
            account.OutStandingBalance = updatedAccount.OutStandingBalance;
            account.Status = updatedAccount.Status;

            return NoContent();
        }

        // DELETE: api/Accounts/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Code == id);
            if (account == null)
            {
                return NotFound($"Account with Code {id} not found.");
            }

            _accounts.Remove(account);
            return NoContent();
        }
    }
}

