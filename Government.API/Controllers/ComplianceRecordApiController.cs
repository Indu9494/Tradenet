using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComplianceRecordApiController : ControllerBase
    {
        private readonly IComplianceRecordRepository _repo;

        public ComplianceRecordApiController(IComplianceRecordRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplianceRecord>>> Get()
        {
            var items = await _repo.GetAllComplianceRecordsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplianceRecord>> Get(int id)
        {
            var item = await _repo.GetComplianceRecordByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ComplianceRecord>> Post([FromBody] ComplianceRecord model)
        {
            await _repo.CreateComplianceRecordAsync(model);
            return CreatedAtAction(nameof(Get), new { id = model.ComplianceID }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ComplianceRecord model)
        {
            if (id != model.ComplianceID) return BadRequest();
            await _repo.UpdateComplianceRecordAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteComplianceRecordAsync(id);
            return Ok();
        }
    }
}

