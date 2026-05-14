using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradeLicenseApiController : ControllerBase
    {
        private readonly ITradeLicenseRepository _repo;

        public TradeLicenseApiController(ITradeLicenseRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeLicense>>> Get()
        {
            var items = await _repo.GetAllTradeLicensesAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeLicense>> Get(int id)
        {
            var item = await _repo.GetTradeLicenseByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<TradeLicense>> Post([FromBody] TradeLicense model)
        {
            await _repo.CreateTradeLicenseAsync(model);
            return CreatedAtAction(nameof(Get), new { id = model.LicenseID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TradeLicense model)
        {
            if (id != model.LicenseID) return BadRequest();
            await _repo.UpdateTradeLicenseAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteTradeLicenseAsync(id);
            return Ok();
        }
    }
}

