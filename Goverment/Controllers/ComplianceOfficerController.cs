using Goverment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    [Authorize(Roles = "Compliance")]
    public class ComplianceOfficerController : Controller
    {
        private readonly IComplianceRecordRepository _complianceRecordRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly ITradeLicenseRepository _tradeLicenseRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ComplianceOfficerController(
            IComplianceRecordRepository complianceRecordRepository,
            IBusinessRepository businessRepository,
            ITradeLicenseRepository tradeLicenseRepository,
            ITransactionRepository transactionRepository)
        {
            _complianceRecordRepository = complianceRecordRepository;
            _businessRepository = businessRepository;
            _tradeLicenseRepository = tradeLicenseRepository;
            _transactionRepository = transactionRepository;
        }

        // GET: ComplianceOfficer/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var complianceRecords = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();

            ViewBag.TotalRecords = complianceRecords.Count();
            ViewBag.CompliantRecords = complianceRecords.Count(c => c.Result == "Compliant");
            ViewBag.NonCompliantRecords = complianceRecords.Count(c => c.Result == "Non-Compliant");
            ViewBag.UnderReview = complianceRecords.Count(c => c.Result == "Under Review");
            ViewBag.ExpiredLicenses = licenses.Count(l => l.Status == "Expired");
            ViewBag.TotalBusinesses = businesses.Count();

            return View();
        }

        // GET: ComplianceOfficer/CheckCompliance
        public async Task<IActionResult> CheckCompliance()
        {
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            
            ViewBag.ExpiredLicenses = licenses.Where(l => l.ExpiryDate < DateTime.Now).ToList();
            ViewBag.ExpiringLicenses = licenses.Where(l => l.ExpiryDate >= DateTime.Now && l.ExpiryDate <= DateTime.Now.AddDays(30)).ToList();
            ViewBag.InactiveBusinesses = businesses.Where(b => b.Status == "Inactive").ToList();
            ViewBag.PendingTransactions = transactions.Where(t => t.Status == "Pending").ToList();
            
            return View();
        }

        // GET: ComplianceOfficer/CreateRecord
        public async Task<IActionResult> CreateRecord()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            ViewBag.Businesses = businesses;
            return View();
        }

        // POST: ComplianceOfficer/CreateRecord
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecord(int entityId, string type, string result, string notes)
        {
            var record = new Models.ComplianceRecord
            {
                EntityID = entityId,
                Type = type,
                Result = result,
                Date = DateTime.Now,
                Notes = notes
            };

            await _complianceRecordRepository.CreateComplianceRecordAsync(record);
            TempData["Success"] = "Compliance record created successfully!";
            return RedirectToAction("ViewRecords");
        }

        // GET: ComplianceOfficer/ViewRecords
        public async Task<IActionResult> ViewRecords()
        {
            var records = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            return View(records);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: ComplianceOfficer/Api/GetDashboardStats
        [HttpGet]
        [Route("ComplianceOfficer/Api/GetDashboardStats")]
        public async Task<IActionResult> ApiGetDashboardStats()
        {
            var complianceRecords = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();

            var stats = new
            {
                totalRecords = complianceRecords.Count(),
                compliantRecords = complianceRecords.Count(c => c.Result == "Compliant"),
                nonCompliantRecords = complianceRecords.Count(c => c.Result == "Non-Compliant"),
                underReview = complianceRecords.Count(c => c.Result == "Under Review"),
                expiredLicenses = licenses.Count(l => l.Status == "Expired"),
                totalBusinesses = businesses.Count()
            };

            return Json(new { success = true, data = stats });
        }

        // GET: ComplianceOfficer/Api/GetComplianceIssues
        [HttpGet]
        [Route("ComplianceOfficer/Api/GetComplianceIssues")]
        public async Task<IActionResult> ApiGetComplianceIssues()
        {
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            var issues = new
            {
                expiredLicenses = licenses.Where(l => l.ExpiryDate < DateTime.Now).ToList(),
                expiringLicenses = licenses.Where(l => l.ExpiryDate >= DateTime.Now && l.ExpiryDate <= DateTime.Now.AddDays(30)).ToList(),
                inactiveBusinesses = businesses.Where(b => b.Status == "Inactive").ToList(),
                pendingTransactions = transactions.Where(t => t.Status == "Pending").ToList()
            };

            return Json(new { success = true, data = issues });
        }

        // GET: ComplianceOfficer/Api/GetAllRecords
        [HttpGet]
        [Route("ComplianceOfficer/Api/GetAllRecords")]
        public async Task<IActionResult> ApiGetAllRecords()
        {
            var records = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            return Json(new { success = true, data = records });
        }

        // GET: ComplianceOfficer/Api/GetNonCompliantRecords
        [HttpGet]
        [Route("ComplianceOfficer/Api/GetNonCompliantRecords")]
        public async Task<IActionResult> ApiGetNonCompliantRecords()
        {
            var records = await _complianceRecordRepository.GetComplianceRecordsByResultAsync("Non-Compliant");
            return Json(new { success = true, data = records });
        }

        // POST: ComplianceOfficer/Api/CreateComplianceRecord
        [HttpPost]
        [Route("ComplianceOfficer/Api/CreateComplianceRecord")]
        public async Task<IActionResult> ApiCreateComplianceRecord([FromBody] ComplianceRecordRequest request)
        {
            if (request == null || request.EntityId <= 0)
            {
                return Json(new { success = false, message = "Invalid record data" });
            }

            var record = new Models.ComplianceRecord
            {
                EntityID = request.EntityId,
                Type = request.Type ?? "General",
                Result = request.Result ?? "Under Review",
                Date = DateTime.Now,
                Notes = request.Notes ?? string.Empty
            };

            await _complianceRecordRepository.CreateComplianceRecordAsync(record);
            return Json(new { success = true, message = "Compliance record created successfully", data = record });
        }
    }

    // Request model for API
    public class ComplianceRecordRequest
    {
        public int EntityId { get; set; }
        public string? Type { get; set; }
        public string? Result { get; set; }
        public string? Notes { get; set; }
    }
}
