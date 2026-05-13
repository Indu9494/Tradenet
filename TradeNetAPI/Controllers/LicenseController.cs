using Microsoft.AspNetCore.Mvc;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ITradeLicenseRepository _licenseRepository;
        private readonly ILicenseDocumentRepository _licenseDocumentRepository;

        public LicenseController(
            ITradeLicenseRepository licenseRepository,
            ILicenseDocumentRepository licenseDocumentRepository)
        {
            _licenseRepository = licenseRepository;
            _licenseDocumentRepository = licenseDocumentRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeLicense>> GetLicense(int id)
        {
            var license = await _licenseRepository.GetByIdAsync(id);
            if (license == null)
                return NotFound();
            return Ok(license);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeLicense>>> GetAllLicenses()
        {
            var licenses = await _licenseRepository.GetAllAsync();
            return Ok(licenses);
        }

        [HttpGet("business/{businessId}")]
        public async Task<ActionResult<IEnumerable<TradeLicense>>> GetLicensesByBusiness(int businessId)
        {
            var licenses = await _licenseRepository.GetAllAsync();
            var businessLicenses = licenses.Where(l => l.BusinessID == businessId);
            return Ok(businessLicenses);
        }

        [HttpPost]
        public async Task<ActionResult<TradeLicense>> CreateLicense([FromBody] TradeLicense license)
        {
            // Ensure LicenseID is not explicitly set - let the database generate it
            license.LicenseID = 0;
            await _licenseRepository.AddAsync(license);
            await _licenseRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLicense), new { id = license.LicenseID }, license);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(int id, [FromBody] TradeLicense license)
        {
            if (id != license.LicenseID)
                return BadRequest();

            await _licenseRepository.UpdateAsync(license);
            await _licenseRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            var license = await _licenseRepository.GetByIdAsync(id);
            if (license == null)
                return NotFound();

            await _licenseRepository.DeleteAsync(license);
            await _licenseRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{licenseId}/documents")]
        public async Task<ActionResult<IEnumerable<LicenseDocument>>> GetLicenseDocuments(int licenseId)
        {
            var documents = await _licenseDocumentRepository.GetAllAsync();
            var licenseDocs = documents.Where(d => d.LicenseID == licenseId);
            return Ok(licenseDocs);
        }
    }
}
