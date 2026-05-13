using Microsoft.AspNetCore.Mvc;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return Ok(transactions);
        }

        [HttpGet("business/{businessId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByBusiness(int businessId)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var businessTransactions = transactions.Where(t => t.BusinessID == businessId);
            return Ok(businessTransactions);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            // Ensure TransactionID is not explicitly set - let the database generate it
            transaction.TransactionID = 0;
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionID }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.TransactionID)
                return BadRequest();

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return NotFound();

            await _transactionRepository.DeleteAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
