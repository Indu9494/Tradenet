using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradeProgramApiController : ControllerBase
    {
        private readonly ITradeProgramRepository _repo;

        public TradeProgramApiController(ITradeProgramRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeProgram>>> Get()
        {
            var items = await _repo.GetAllTradeProgramsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeProgram>> Get(int id)
        {
            var item = await _repo.GetTradeProgramByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TradeProgram>>> GetByStatus(string status)
        {
            var items = await _repo.GetTradeProgramsByStatusAsync(status);
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TradeProgram>>> GetActive()
        {
            var items = await _repo.GetActiveTradeProgramsAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<TradeProgram>> Post([FromBody] TradeProgram model)
        {
            var created = await _repo.CreateTradeProgramAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.ProgramID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TradeProgram model)
        {
            if (id != model.ProgramID) return BadRequest();
            await _repo.UpdateTradeProgramAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteTradeProgramAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}

