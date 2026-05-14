using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditApiController : ControllerBase
    {
        private readonly IAuditRepository _repo;

        public AuditApiController(IAuditRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Audit>>> Get()
        {
            var items = await _repo.GetAllAuditsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Audit>> Get(int id)
        {
            var item = await _repo.GetAuditByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("officer/{officerId}")]
        public async Task<ActionResult<IEnumerable<Audit>>> GetByOfficer(int officerId)
        {
            var items = await _repo.GetAuditsByOfficerIdAsync(officerId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Audit>> Post([FromBody] Audit model)
        {
            var created = await _repo.CreateAuditAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.AuditID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Audit model)
        {
            if (id != model.AuditID) return BadRequest();
            await _repo.UpdateAuditAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAuditAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}

