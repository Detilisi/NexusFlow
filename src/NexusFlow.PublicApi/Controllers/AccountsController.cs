using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusFlow.PublicApi.Data.Repositories;
using NexusFlow.PublicApi.Models;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository _accountRepository;

        public AccountsController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        // GET: api/Accounts/{id}
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(int code)
        {
            var account = await _accountRepository.GetAccountByCodeAsync(code);
            if (account == null)
            {
                return NotFound($"Account with Code {code} not found.");
            }
            return Ok(account);
        }

        // GET: api/Accounts/{personCode}/{accountCode?}
        [HttpGet("{personCode}/{accountCode?}")]
        public async Task<IActionResult> Get(int personCode, int? accountCode = null)
        {
            var accounts = await _accountRepository.GetAccountsForPersonAsync(personCode, accountCode);
            if (!accounts.Any())
            {
                return NotFound($"No accounts found for PersonCode {personCode} and AccountCode {accountCode}.");
            }

            return Ok(accounts);
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account newAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rowsAffected = await _accountRepository.CreateAccountAsync(newAccount);
            if (rowsAffected > 0)
            {
                return CreatedAtAction(nameof(Get), new { code = newAccount.Code }, newAccount);
            }

            return BadRequest("Failed to create account.");
        }

        // PUT: api/Accounts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account updatedAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedAccount.Code)
            {
                return BadRequest("Account code mismatch.");
            }

            var rowsAffected = await _accountRepository.UpdateAccountDetailsAsync(updatedAccount);
            if (rowsAffected > 0)
            {
                return NoContent();
            }

            return NotFound($"Account with Code {id} not found.");
        }

        // DELETE: api/Accounts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var rowsAffected = await _accountRepository.DeleteAccountAsync(id);
            if (rowsAffected > 0)
            {
                return NoContent();
            }

            return NotFound($"Account with Code {id} not found.");
        }
    }
}

