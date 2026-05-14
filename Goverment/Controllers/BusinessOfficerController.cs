using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    [Authorize(Roles = "Business")]
    public class BusinessOfficerController : Controller
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly ITradeLicenseRepository _tradeLicenseRepository;
        private readonly ISubsidyRepository _subsidyRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITradeProgramRepository _tradeProgramRepository;

        public BusinessOfficerController(
            IBusinessRepository businessRepository,
            ITradeLicenseRepository tradeLicenseRepository,
            ISubsidyRepository subsidyRepository,
            ITransactionRepository transactionRepository,
            ITradeProgramRepository tradeProgramRepository)
        {
            _businessRepository = businessRepository;
            _tradeLicenseRepository = tradeLicenseRepository;
            _subsidyRepository = subsidyRepository;
            _transactionRepository = transactionRepository;
            _tradeProgramRepository = tradeProgramRepository;
        }

        // =============================================
        // MVC View Actions
        // =============================================

        // GET: BusinessOfficer/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            ViewBag.TotalBusinesses = businesses.Count();
            ViewBag.TotalLicenses = licenses.Count();
            ViewBag.ActiveLicenses = licenses.Count(l => l.Status == "Active");
            ViewBag.PendingSubsidies = subsidies.Count(s => s.Status == "Pending");
            ViewBag.ApprovedSubsidies = subsidies.Count(s => s.Status == "Approved");
            ViewBag.TotalTransactions = transactions.Sum(t => t.Amount);

            return View();
        }

        // GET: BusinessOfficer/MyLicenses
        public async Task<IActionResult> MyLicenses()
        {
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            return View(licenses);
        }

        // GET: BusinessOfficer/ApplyLicense
        public async Task<IActionResult> ApplyLicense()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            ViewBag.Businesses = businesses;
            return View();
        }

        // POST: BusinessOfficer/ApplyLicense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLicense(int businessId, string type)
        {
            if (businessId <= 0 || string.IsNullOrWhiteSpace(type))
            {
                TempData["Error"] = "Please provide valid business and license type.";
                return RedirectToAction(nameof(ApplyLicense));
            }

            var license = new TradeLicense
            {
                BusinessID = businessId,
                Type = type,
                IssuedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                Status = "Pending"
            };

            await _tradeLicenseRepository.CreateTradeLicenseAsync(license);
            TempData["Success"] = "License application submitted successfully! Awaiting Trade Officer approval.";
            return RedirectToAction(nameof(MyLicenses));
        }

        // GET: BusinessOfficer/MySubsidies
        public async Task<IActionResult> MySubsidies()
        {
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            return View(subsidies);
        }

        // GET: BusinessOfficer/ApplySubsidy
        public async Task<IActionResult> ApplySubsidy()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();

            ViewBag.Businesses = businesses;
            ViewBag.Programs = programs;

            return View();
        }

        // POST: BusinessOfficer/ApplySubsidy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplySubsidy(int businessId, int programId, string type, decimal amount, string notes)
        {
            if (businessId <= 0 || programId <= 0 || amount <= 0)
            {
                TempData["Error"] = "Please provide valid application details.";
                return RedirectToAction(nameof(ApplySubsidy));
            }

            var subsidy = new Subsidy
            {
                BusinessID = businessId,
                ProgramID = programId,
                Type = type,
                Amount = amount,
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                Notes = notes ?? string.Empty
            };

            await _subsidyRepository.CreateSubsidyAsync(subsidy);
            TempData["Success"] = "Subsidy application submitted successfully! Awaiting Program Manager approval.";
            return RedirectToAction(nameof(MySubsidies));
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: BusinessOfficer/Api/GetDashboardStats
        [HttpGet]
        [Route("BusinessOfficer/Api/GetDashboardStats")]
        public async Task<IActionResult> ApiGetDashboardStats()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            var stats = new
            {
                totalBusinesses = businesses.Count(),
                totalLicenses = licenses.Count(),
                activeLicenses = licenses.Count(l => l.Status == "Active"),
                pendingSubsidies = subsidies.Count(s => s.Status == "Pending"),
                approvedSubsidies = subsidies.Count(s => s.Status == "Approved"),
                totalTransactions = transactions.Sum(t => t.Amount)
            };

            return Json(new { success = true, data = stats });
        }

        // GET: BusinessOfficer/Api/GetMyLicenses
        [HttpGet]
        [Route("BusinessOfficer/Api/GetMyLicenses")]
        public async Task<IActionResult> ApiGetMyLicenses()
        {
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            return Json(new { success = true, data = licenses });
        }

        // GET: BusinessOfficer/Api/GetMyLicensesByBusiness/{businessId}
        [HttpGet]
        [Route("BusinessOfficer/Api/GetMyLicensesByBusiness/{businessId}")]
        public async Task<IActionResult> ApiGetMyLicensesByBusiness(int businessId)
        {
            var licenses = await _tradeLicenseRepository.GetTradeLicensesByBusinessIdAsync(businessId);
            return Json(new { success = true, data = licenses });
        }

        // GET: BusinessOfficer/Api/GetMySubsidies
        [HttpGet]
        [Route("BusinessOfficer/Api/GetMySubsidies")]
        public async Task<IActionResult> ApiGetMySubsidies()
        {
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            return Json(new { success = true, data = subsidies });
        }

        // GET: BusinessOfficer/Api/GetMySubsidiesByBusiness/{businessId}
        [HttpGet]
        [Route("BusinessOfficer/Api/GetMySubsidiesByBusiness/{businessId}")]
        public async Task<IActionResult> ApiGetMySubsidiesByBusiness(int businessId)
        {
            var subsidies = await _subsidyRepository.GetSubsidiesByBusinessIdAsync(businessId);
            return Json(new { success = true, data = subsidies });
        }

        // GET: BusinessOfficer/Api/GetAvailablePrograms
        [HttpGet]
        [Route("BusinessOfficer/Api/GetAvailablePrograms")]
        public async Task<IActionResult> ApiGetAvailablePrograms()
        {
            var programs = await _tradeProgramRepository.GetActiveTradeProgramsAsync();
            return Json(new { success = true, data = programs });
        }

        // POST: BusinessOfficer/Api/SubmitLicenseApplication
        [HttpPost]
        [Route("BusinessOfficer/Api/SubmitLicenseApplication")]
        public async Task<IActionResult> ApiSubmitLicenseApplication([FromBody] LicenseApplicationRequest request)
        {
            if (request == null || request.BusinessId <= 0 || string.IsNullOrWhiteSpace(request.Type))
            {
                return Json(new { success = false, message = "Invalid application data" });
            }

            var license = new TradeLicense
            {
                BusinessID = request.BusinessId,
                Type = request.Type,
                IssuedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(1),
                Status = "Pending"
            };

            await _tradeLicenseRepository.CreateTradeLicenseAsync(license);
            return Json(new { success = true, message = "License application submitted successfully", data = license });
        }

        // POST: BusinessOfficer/Api/SubmitSubsidyApplication
        [HttpPost]
        [Route("BusinessOfficer/Api/SubmitSubsidyApplication")]
        public async Task<IActionResult> ApiSubmitSubsidyApplication([FromBody] SubsidyApplicationRequest request)
        {
            if (request == null || request.BusinessId <= 0 || request.ProgramId <= 0 || request.Amount <= 0)
            {
                return Json(new { success = false, message = "Invalid application data" });
            }

            var subsidy = new Subsidy
            {
                BusinessID = request.BusinessId,
                ProgramID = request.ProgramId,
                Type = request.Type ?? "Grant",
                Amount = request.Amount,
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                Notes = request.Notes ?? string.Empty
            };

            await _subsidyRepository.CreateSubsidyAsync(subsidy);
            return Json(new { success = true, message = "Subsidy application submitted successfully", data = subsidy });
        }
    }

    // Request models for API
    public class LicenseApplicationRequest
    {
        public int BusinessId { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public class SubsidyApplicationRequest
    {
        public int BusinessId { get; set; }
        public int ProgramId { get; set; }
        public string? Type { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
