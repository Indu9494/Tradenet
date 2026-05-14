using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class AuditController : Controller
    {
        private readonly IAuditRepository _auditRepository;

        public AuditController(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        // GET: Audit
        public async Task<IActionResult> Index()
        {
            var audits = await _auditRepository.GetAllAuditsAsync();
            return View(audits);
        }

        // GET: Audit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audit = await _auditRepository.GetAuditByIdAsync(id.Value);
            if (audit == null)
            {
                return NotFound();
            }

            return View(audit);
        }

        // GET: Audit/ByOfficer/5
        public async Task<IActionResult> ByOfficer(int officerId)
        {
            var audits = await _auditRepository.GetAuditsByOfficerIdAsync(officerId);
            return View("Index", audits);
        }

        // GET: Audit/ByStatus/Completed
        public async Task<IActionResult> ByStatus(string status)
        {
            var audits = await _auditRepository.GetAuditsByStatusAsync(status);
            return View("Index", audits);
        }

        // GET: Audit/ByScope/License
        public async Task<IActionResult> ByScope(string scope)
        {
            var audits = await _auditRepository.GetAuditsByScopeAsync(scope);
            return View("Index", audits);
        }

        // GET: Audit/ByDateRange?startDate=2024-01-01&endDate=2024-12-31
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var audits = await _auditRepository.GetAuditsByDateRangeAsync(startDate, endDate);
            return View("Index", audits);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: Audit/Api/GetAll
        [HttpGet]
        [Route("Audit/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var audits = await _auditRepository.GetAllAuditsAsync();
            return Json(audits);
        }

        // GET: Audit/Api/GetById/5
        [HttpGet]
        [Route("Audit/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var audit = await _auditRepository.GetAuditByIdAsync(id);
            if (audit == null)
            {
                return Json(new { success = false, message = $"Audit with ID {id} not found." });
            }
            return Json(new { success = true, data = audit });
        }

        // GET: Audit/Api/GetByOfficer/5
        [HttpGet]
        [Route("Audit/Api/GetByOfficer/{officerId}")]
        public async Task<IActionResult> ApiGetByOfficer(int officerId)
        {
            var audits = await _auditRepository.GetAuditsByOfficerIdAsync(officerId);
            return Json(new { success = true, data = audits });
        }

        // GET: Audit/Api/GetByStatus/Completed
        [HttpGet]
        [Route("Audit/Api/GetByStatus/{status}")]
        public async Task<IActionResult> ApiGetByStatus(string status)
        {
            var audits = await _auditRepository.GetAuditsByStatusAsync(status);
            return Json(new { success = true, data = audits });
        }

        // GET: Audit/Api/GetByScope/License
        [HttpGet]
        [Route("Audit/Api/GetByScope/{scope}")]
        public async Task<IActionResult> ApiGetByScope(string scope)
        {
            var audits = await _auditRepository.GetAuditsByScopeAsync(scope);
            return Json(new { success = true, data = audits });
        }

        // GET: Audit/Api/GetByDateRange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        [Route("Audit/Api/GetByDateRange")]
        public async Task<IActionResult> ApiGetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var audits = await _auditRepository.GetAuditsByDateRangeAsync(startDate, endDate);
            return Json(new { success = true, data = audits });
        }
    }
}
