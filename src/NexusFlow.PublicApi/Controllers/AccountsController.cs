using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NexusFlow.PublicApi.Data.Repositories;
using NexusFlow.PublicApi.Models;
using System.Security.Principal;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository _repository;

        public AccountsController(AccountRepository accountRepository)
        {
            _repository = accountRepository;
        }


        // GET: api/Accounts/{personCode}/{accountCode?}
        [HttpGet("{personCode=-1}/{accountCode=-1}")]
        public async Task<IActionResult> Get(int personCode=-1, int accountCode = -1)
        {
            var accounts = await _repository.GetAccounts(personCode, accountCode);
            if (!accounts.Any())
            {
                return NotFound($"No accounts found for PersonCode {personCode} and AccountCode {accountCode}.");
            }

            return Ok(accounts);
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] Account newAccount)
        {
            if (newAccount == null || string.IsNullOrWhiteSpace(newAccount.AccountNumber))
            {
                return BadRequest(new { Message = "Invalid account data provided." });
            }

            try
            {
                var result = await _repository.CreateAccountAsync(newAccount);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        // PUT: api/Accounts/{code}
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateAccount(int code, [FromBody] Account updatedAccount)
        {
            if (updatedAccount == null || string.IsNullOrWhiteSpace(updatedAccount.AccountNumber))
            {
                return BadRequest(new { Message = "Invalid account data provided." });
            }

            var result = await _repository.GetAccounts(-1, code);
            var existingAccount = result.FirstOrDefault();
            if (existingAccount == null || existingAccount.Code != code)
            {
                return NotFound(new { Message = $"Account with code {code} not found." });
            }

            updatedAccount.Code = code; // Ensure the code remains the same during the update

            await _repository.UpdateAccountDetailsAsync(updatedAccount);
            return Ok(updatedAccount);
        }

        // DELETE: api/Accounts/{code}
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteAccount(int code)
        {
            try
            {
                await _repository.DeleteAccountAsync(code);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex) 
            {
                return Conflict(new { Message = ex.Message });
            }
           
        }
    }
}

