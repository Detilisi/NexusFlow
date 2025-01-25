using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusFlow.PublicApi.Models;

namespace NexusFlow.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private static List<Transaction> _transactions = new List<Transaction>
        {
            new Transaction
            {
                Code = 1,
                AccountCode = 1,
                Amount = 500.00m,
                Description = "Initial Deposit",
                //CaptureDate = DateTime.Now,
                //TransactionDate = DateTime.Now,
                //Type = TransactionType.Credit
            },
            new Transaction
            {
                Code = 2,
                AccountCode = 2,
                Amount = 200.00m,
                Description = "Bill Payment",
                //CaptureDate = DateTime.Now,
                //TransactionDate = DateTime.Now,
                //Type = TransactionType.Debit
            }
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_transactions);
        }

        [HttpGet("{code}")]
        public IActionResult Get(int code)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Code == code);
            if (transaction == null)
                return NotFound($"Transaction with Code {code} not found.");

            return Ok(transaction);
        }

        [HttpGet("{accountCode}/{transactionCode=-1}")]
        public IActionResult Get(int accountCode, int transactionCode = -1)
        {
            IEnumerable<Transaction> result;

            if (transactionCode == -1)
            {
                result = _transactions.Where(a => a.AccountCode == accountCode);
            }
            else
            {
                // Fetch a specific account by accountCode and personCode
                result = _transactions.Where(a => a.Code == transactionCode && a.AccountCode == accountCode);
            }

            if (!result.Any())
            {
                return NotFound("No transactions found");
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Transaction newTransaction)
        {
            if (newTransaction == null)
                return BadRequest("Transaction data is null.");

            newTransaction.Code = _transactions.Any() ? _transactions.Max(t => t.Code) + 1 : 1;
            _transactions.Add(newTransaction);

            return CreatedAtAction(nameof(Get), new { code = newTransaction.Code }, newTransaction);
        }

        [HttpPut("{code}")]
        public IActionResult Update(int code, [FromBody] Transaction updatedTransaction)
        {
            var existingTransaction = _transactions.FirstOrDefault(t => t.Code == code);
            if (existingTransaction == null)
                return NotFound($"Transaction with Code {code} not found.");

            existingTransaction.AccountCode = updatedTransaction.AccountCode;
            existingTransaction.Amount = updatedTransaction.Amount;
            existingTransaction.Description = updatedTransaction.Description;
            existingTransaction.CaptureDate = updatedTransaction.CaptureDate;
            existingTransaction.TransactionDate = updatedTransaction.TransactionDate;

            return NoContent();
        }

        [HttpDelete("{code}")]
        public IActionResult Delete(int code)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Code == code);
            if (transaction == null)
                return NotFound($"Transaction with Code {code} not found.");

            _transactions.Remove(transaction);
            return NoContent();
        }
    }
}
