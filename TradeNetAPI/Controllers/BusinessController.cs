using Microsoft.AspNetCore.Mvc;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IBusinessDocumentRepository _businessDocumentRepository;

        public BusinessController(
            IBusinessRepository businessRepository,
            IBusinessDocumentRepository businessDocumentRepository)
        {
            _businessRepository = businessRepository;
            _businessDocumentRepository = businessDocumentRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            var business = await _businessRepository.GetByIdAsync(id);
            if (business == null)
                return NotFound();
            return Ok(business);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Business>> GetBusinessByUserId(int userId)
        {
            var businesses = await _businessRepository.GetAllAsync();
            var business = businesses.FirstOrDefault(b => b.UserID == userId);
            if (business == null)
                return NotFound();
            return Ok(business);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetAllBusinesses()
        {
            var businesses = await _businessRepository.GetAllAsync();
            return Ok(businesses);
        }

        [HttpPost]
        public async Task<ActionResult<Business>> CreateBusiness([FromBody] Business business)
        {
            // Ensure BusinessID is not explicitly set - let the database generate it
            business.BusinessID = 0;
            await _businessRepository.AddAsync(business);
            await _businessRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBusiness), new { id = business.BusinessID }, business);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(int id, [FromBody] Business business)
        {
            if (id != business.BusinessID)
                return BadRequest();

            await _businessRepository.UpdateAsync(business);
            await _businessRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var business = await _businessRepository.GetByIdAsync(id);
            if (business == null)
                return NotFound();

            await _businessRepository.DeleteAsync(business);
            await _businessRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{businessId}/documents")]
        public async Task<ActionResult<IEnumerable<BusinessDocument>>> GetBusinessDocuments(int businessId)
        {
            var documents = await _businessDocumentRepository.GetAllAsync();
            var businessDocs = documents.Where(d => d.BusinessID == businessId);
            return Ok(businessDocs);
        }
    }
}
