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
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsRepository _repository;

        public TransactionsController(TransactionsRepository accountRepository)
        {
            _repository = accountRepository;
        }

        // GET: api/Transactions/{personCode}/{accountCode?}
        [HttpGet("{accountCode=-1}/{transactionCode=-1}")]
        public async Task<IActionResult> Get(int accountCode = -1, int transactionCode = -1)
        {
            var results = await _repository.GetTransactions(accountCode, transactionCode);
            if (!results.Any())
            {
                return NotFound("No Transactions found.");
            }

            return Ok(results);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction newTransaction)
        {
            if (newTransaction == null || string.IsNullOrWhiteSpace(newTransaction.Description))
            {
                return BadRequest(new { Message = "Invalid transaction data provided." });
            }

            try
            {
                var result = await _repository.CreateTransactionAsync(newTransaction);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(int code, [FromBody] Transaction updatedTransaction)
        {
            var result = await _repository.GetTransactions(-1, code);
            var existingTransaction = result.FirstOrDefault();
            
            if (existingTransaction == null || existingTransaction.Code != code)
            {
                return NotFound($"Transaction with Code {code} not found.");
            }

            updatedTransaction.Code = code;

            await _repository.UpdateTransactionAsync(updatedTransaction);
            return Ok(updatedTransaction);
        }
    }
}
