using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradeNetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _service;

        public LicenseController(ILicenseService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TradeLicense>> Get()
        {
            return Ok(_service.GetAllLicenses());
        }

        [HttpGet("{id}")]
        public ActionResult<TradeLicense> Get(int id)
        {
            var item = _service.GetLicenseById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(TradeLicense license)
        {
            _service.AddLicense(license);
            return CreatedAtAction(nameof(Get), new { id = license.LicenseID }, license);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TradeLicense license)
        {
            if (id != license.LicenseID) return BadRequest();
            _service.UpdateLicense(license);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteLicense(id);
            return NoContent();
        }
    }
}
