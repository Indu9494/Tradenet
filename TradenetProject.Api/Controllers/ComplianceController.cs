using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradenetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceService _service;

        public ComplianceController(IComplianceService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ComplianceRecord>> Get()
        {
            return Ok(_service.GetAllRecords());
        }

        [HttpGet("{id}")]
        public ActionResult<ComplianceRecord> Get(int id)
        {
            var rec = _service.GetRecordById(id);
            if (rec == null) return NotFound();
            return Ok(rec);
        }

        [HttpPost]
        public IActionResult Create(ComplianceRecord record)
        {
            _service.AddRecord(record);
            return CreatedAtAction(nameof(Get), new { id = record.ComplianceID }, record);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ComplianceRecord record)
        {
            if (id != record.ComplianceID) return BadRequest();
            _service.UpdateRecord(record);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteRecord(id);
            return NoContent();
        }
    }
}
