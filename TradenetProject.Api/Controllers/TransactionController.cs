using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradeNetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> Get()
        {
            return Ok(_service.GetAllTransactions());
        }

        [HttpGet("{id}")]
        public ActionResult<Transaction> Get(int id)
        {
            var item = _service.GetTransactionById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(Transaction transaction)
        {
            _service.AddTransaction(transaction);
            return CreatedAtAction(nameof(Get), new { id = transaction.TransactionID }, transaction);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Transaction transaction)
        {
            if (id != transaction.TransactionID) return BadRequest();
            _service.UpdateTransaction(transaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteTransaction(id);
            return NoContent();
        }
    }
}
