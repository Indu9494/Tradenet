using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradeNetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeOfficerController : ControllerBase
    {
        private readonly ITradeOfficerService _service;

        public TradeOfficerController(ITradeOfficerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TradeOfficer>> Get()
        {
            // For simplicity using a single current officer retrieval
            var officer = _service.GetCurrentOfficer();
            return Ok(new[] { officer });
        }

        [HttpGet("{id}")]
        public ActionResult<TradeOfficer> Get(int id)
        {
            var item = _service.GetOfficerById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
