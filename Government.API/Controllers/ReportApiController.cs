using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportApiController : ControllerBase
    {
        private readonly IReportRepository _repo;

        public ReportApiController(IReportRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> Get()
        {
            var items = await _repo.GetAllReportsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> Get(int id)
        {
            var item = await _repo.GetReportByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Report>> Post([FromBody] Report model)
        {
            var created = await _repo.CreateReportAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.ReportID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Report model)
        {
            if (id != model.ReportID) return BadRequest();
            await _repo.UpdateReportAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteReportAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
