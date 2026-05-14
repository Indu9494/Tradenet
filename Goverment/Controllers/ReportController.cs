using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // GET: Report
        public async Task<IActionResult> Index()
        {
            var Report = await _reportRepository.GetAllReportsAsync();
            return View(Report);
        }

        // GET: Report/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _reportRepository.GetReportByIdAsync(id.Value);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Report/ByScope/License
        public async Task<IActionResult> ByScope(string scope)
        {
            var reports = await _reportRepository.GetReportsByScopeAsync(scope);
            return View("Index", reports);
        }

        // GET: Report/ByDateRange?startDate=2024-01-01&endDate=2024-12-31
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var reports = await _reportRepository.GetReportsByDateRangeAsync(startDate, endDate);
            return View("Index", reports);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: Report/Api/GetAll
        [HttpGet]
        [Route("Report/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var reports = await _reportRepository.GetAllReportsAsync();
            return Json(reports);
        }

        // GET: Report/Api/GetById/5
        [HttpGet]
        [Route("Report/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var report = await _reportRepository.GetReportByIdAsync(id);
            if (report == null)
            {
                return Json(new { success = false, message = $"Report with ID {id} not found." });
            }
            return Json(new { success = true, data = report });
        }

        // GET: Report/Api/GetByScope/License
        [HttpGet]
        [Route("Report/Api/GetByScope/{scope}")]
        public async Task<IActionResult> ApiGetByScope(string scope)
        {
            var reports = await _reportRepository.GetReportsByScopeAsync(scope);
            return Json(new { success = true, data = reports });
        }

        // GET: Report/Api/GetByDateRange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        [Route("Report/Api/GetByDateRange")]
        public async Task<IActionResult> ApiGetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var reports = await _reportRepository.GetReportsByDateRangeAsync(startDate, endDate);
            return Json(new { success = true, data = reports });
        }
    }
}
