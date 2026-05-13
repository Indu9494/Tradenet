using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradeNetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketRecordController : ControllerBase
    {
        private readonly IMarketRecordService _service;

        public MarketRecordController(IMarketRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MarketRecord>> Get()
        {
            return Ok(_service.GetAllRecords());
        }

        [HttpGet("{id}")]
        public ActionResult<MarketRecord> Get(int id)
        {
            var item = _service.GetRecordById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(MarketRecord record)
        {
            _service.AddRecord(record);
            return CreatedAtAction(nameof(Get), new { id = record.RecordID }, record);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, MarketRecord record)
        {
            if (id != record.RecordID) return BadRequest();
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
