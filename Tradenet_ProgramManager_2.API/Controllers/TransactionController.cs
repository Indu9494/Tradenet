using Microsoft.AspNetCore.Mvc;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Repositories;

namespace Tradenet_ProgramManager_2.API.Controllers
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

        /// <summary>
        /// Get all transactions ordered by most recent first.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAll();
            return Ok(transactions);
        }

        /// <summary>
        /// Get a single transaction by ID.
        /// Returns 404 if transaction not found.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var transaction = await _transactionRepository.GetById(id);
            if (transaction == null)
            {
                return NotFound(new { message = "Transaction not found" });
            }
            return Ok(transaction);
        }

        /// <summary>
        /// Get all transactions for a specific program.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("program/{programId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByProgramId(int programId)
        {
            var transactions = await _transactionRepository.GetByProgramId(programId);
            return Ok(transactions);
        }

        /// <summary>
        /// Get all transactions by type (Sale or Purchase).
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByType(string type)
        {
            var transactions = await _transactionRepository.GetByType(type);
            return Ok(transactions);
        }

        /// <summary>
        /// Create a new transaction.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _transactionRepository.Add(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        /// <summary>
        /// Get the total amount of transactions by type (Sale or Purchase).
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("total/{type}")]
        public async Task<ActionResult<decimal>> GetTotalByType(string type)
        {
            var total = await _transactionRepository.GetTotalByType(type);
            return Ok(new { type = type, total = total });
        }
    }
}
