using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusinessApiController : ControllerBase
    {
        private readonly IBusinessRepository _repo;

        public BusinessApiController(IBusinessRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> Get()
        {
            var items = await _repo.GetAllBusinessesAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Business>> Post([FromBody] Business model)
        {
            var created = await _repo.AddBusinessAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.BusinessID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Business model)
        {
            if (id != model.BusinessID) return BadRequest();
            await _repo.UpdateBusinessAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteBusinessAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> Get(int id)
        {
            var item = await _repo.GetBusinessByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<Business>>> GetByType(string type)
        {
            var items = await _repo.GetBusinessesByTypeAsync(type);
            return Ok(items);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Business>>> GetByStatus(string status)
        {
            var items = await _repo.GetBusinessesByStatusAsync(status);
            return Ok(items);
        }
    }
}

