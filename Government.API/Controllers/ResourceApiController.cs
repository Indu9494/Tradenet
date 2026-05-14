using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResourceApiController : ControllerBase
    {
        private readonly IResourceRepository _repo;

        public ResourceApiController(IResourceRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> Get()
        {
            var items = await _repo.GetAllResourcesAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(int id)
        {
            var item = await _repo.GetResourceByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Resource>> Post([FromBody] Resource model)
        {
            await _repo.CreateResourceAsync(model);
            return CreatedAtAction(nameof(Get), new { id = model.ResourceID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Resource model)
        {
            if (id != model.ResourceID) return BadRequest();
            await _repo.UpdateResourceAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteResourceAsync(id);
            return Ok();
        }
    }
}

