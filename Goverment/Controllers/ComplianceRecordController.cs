using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class ComplianceRecordController : Controller
    {
        private readonly IComplianceRecordRepository _complianceRecordRepository;

        public ComplianceRecordController(IComplianceRecordRepository complianceRecordRepository)
        {
            _complianceRecordRepository = complianceRecordRepository;
        }

        // GET: ComplianceRecord
        public async Task<IActionResult> Index()
        {
            var complianceRecords = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            return View(complianceRecords);
        }

        // GET: ComplianceRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complianceRecord = await _complianceRecordRepository.GetComplianceRecordByIdAsync(id.Value);
            if (complianceRecord == null)
            {
                return NotFound();
            }

            return View(complianceRecord);
        }

        // GET: ComplianceRecord/ByType/License
        public async Task<IActionResult> ByType(string type)
        {
            var complianceRecords = await _complianceRecordRepository.GetComplianceRecordsByTypeAsync(type);
            return View("Index", complianceRecords);
        }

        // GET: ComplianceRecord/ByEntity/5
        public async Task<IActionResult> ByEntity(int entityId)
        {
            var complianceRecords = await _complianceRecordRepository.GetComplianceRecordsByEntityIdAsync(entityId);
            return View("Index", complianceRecords);
        }

        // GET: ComplianceRecord/ByResult/Compliant
        public async Task<IActionResult> ByResult(string result)
        {
            var complianceRecords = await _complianceRecordRepository.GetComplianceRecordsByResultAsync(result);
            return View("Index", complianceRecords);
        }

        // GET: ComplianceRecord/ByDateRange?startDate=2024-01-01&endDate=2024-12-31
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var complianceRecords = await _complianceRecordRepository.GetComplianceRecordsByDateRangeAsync(startDate, endDate);
            return View("Index", complianceRecords);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: ComplianceRecord/Api/GetAll
        [HttpGet]
        [Route("ComplianceRecord/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var records = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            return Json(records);
        }

        // GET: ComplianceRecord/Api/GetById/5
        [HttpGet]
        [Route("ComplianceRecord/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var record = await _complianceRecordRepository.GetComplianceRecordByIdAsync(id);
            if (record == null)
            {
                return Json(new { success = false, message = $"Compliance record with ID {id} not found." });
            }
            return Json(new { success = true, data = record });
        }

        // GET: ComplianceRecord/Api/GetByType/License
        [HttpGet]
        [Route("ComplianceRecord/Api/GetByType/{type}")]
        public async Task<IActionResult> ApiGetByType(string type)
        {
            var records = await _complianceRecordRepository.GetComplianceRecordsByTypeAsync(type);
            return Json(new { success = true, data = records });
        }

        // GET: ComplianceRecord/Api/GetByEntity/5
        [HttpGet]
        [Route("ComplianceRecord/Api/GetByEntity/{entityId}")]
        public async Task<IActionResult> ApiGetByEntity(int entityId)
        {
            var records = await _complianceRecordRepository.GetComplianceRecordsByEntityIdAsync(entityId);
            return Json(new { success = true, data = records });
        }

        // GET: ComplianceRecord/Api/GetByResult/Compliant
        [HttpGet]
        [Route("ComplianceRecord/Api/GetByResult/{result}")]
        public async Task<IActionResult> ApiGetByResult(string result)
        {
            var records = await _complianceRecordRepository.GetComplianceRecordsByResultAsync(result);
            return Json(new { success = true, data = records });
        }

        // GET: ComplianceRecord/Api/GetByDateRange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        [Route("ComplianceRecord/Api/GetByDateRange")]
        public async Task<IActionResult> ApiGetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var records = await _complianceRecordRepository.GetComplianceRecordsByDateRangeAsync(startDate, endDate);
            return Json(new { success = true, data = records });
        }
    }
}
