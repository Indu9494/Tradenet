using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionApiController : ControllerBase
    {
        private readonly ITransactionRepository _repo;

        public TransactionApiController(ITransactionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            var items = await _repo.GetAllTransactionsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> Get(int id)
        {
            var item = await _repo.GetTransactionByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("business/{businessId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetByBusiness(int businessId)
        {
            var items = await _repo.GetTransactionsByBusinessIdAsync(businessId);
            return Ok(items);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetByType(string type)
        {
            var items = await _repo.GetTransactionsByTypeAsync(type);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Post([FromBody] Transaction model)
        {
            var created = await _repo.CreateTransactionAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.TransactionID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Transaction model)
        {
            if (id != model.TransactionID) return BadRequest();
            await _repo.UpdateTransactionAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteTransactionAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}

