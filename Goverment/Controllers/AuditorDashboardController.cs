using Goverment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    [Authorize]
    public class AuditorDashboardController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IComplianceRecordRepository _complianceRecordRepository;
        private readonly ITradeProgramRepository _tradeProgramRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ISubsidyRepository _subsidyRepository;
        private readonly ITradeLicenseRepository _tradeLicenseRepository;

        public AuditorDashboardController(
            IReportRepository reportRepository,
            ITransactionRepository transactionRepository,
            IComplianceRecordRepository complianceRecordRepository,
            ITradeProgramRepository tradeProgramRepository,
            IBusinessRepository businessRepository,
            IAuditRepository auditRepository,
            ISubsidyRepository subsidyRepository,
            ITradeLicenseRepository tradeLicenseRepository)
        {
            _reportRepository = reportRepository;
            _transactionRepository = transactionRepository;
            _complianceRecordRepository = complianceRecordRepository;
            _tradeProgramRepository = tradeProgramRepository;
            _businessRepository = businessRepository;
            _auditRepository = auditRepository;
            _subsidyRepository = subsidyRepository;
            _tradeLicenseRepository = tradeLicenseRepository;
        }

        // GET: AuditorDashboard/Index - Main Audit Dashboard
        public async Task<IActionResult> Index()
        {
            // Get summary data for the dashboard
            var totalTransactions = await _transactionRepository.GetTotalTransactionAmountAsync();
            var complianceRecords = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            var recentReports = await _reportRepository.GetAllReportsAsync();
            var activePrograms = await _tradeProgramRepository.GetActiveTradeProgramsAsync();
            var totalBudget = await _tradeProgramRepository.GetTotalProgramBudgetAsync();

            ViewBag.TotalTransactionAmount = totalTransactions;
            ViewBag.TotalComplianceRecords = complianceRecords.Count();
            ViewBag.ComplianceViolations = complianceRecords.Count(c => c.Result == "Non-Compliant" || c.Result == "Failed");
            ViewBag.ActiveProgramsCount = activePrograms.Count();
            ViewBag.TotalProgramBudget = totalBudget;
            ViewBag.RecentReportsCount = recentReports.Count();

            return View();
        }

        // GET: AuditorDashboard/InvestigationHub - Investigation Hub
        public async Task<IActionResult> InvestigationHub()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            return View(businesses);
        }

        // POST: AuditorDashboard/SearchInvestigation
        [HttpPost]
        public async Task<IActionResult> SearchInvestigation(string searchType, string searchValue)
        {
            ViewBag.SearchType = searchType;
            ViewBag.SearchValue = searchValue;
            ViewBag.SearchPerformed = true;

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                TempData["Error"] = "Please enter a search value.";
                return RedirectToAction(nameof(InvestigationHub));
            }

            if (searchType == "BusinessID" && int.TryParse(searchValue, out int businessId))
            {
                var business = await _businessRepository.GetBusinessByIdAsync(businessId);
                if (business != null)
                {
                    var transactions = await _transactionRepository.GetTransactionsByBusinessIdAsync(businessId);
                    var licenses = await _tradeLicenseRepository.GetTradeLicensesByBusinessIdAsync(businessId);
                    var audits = await _auditRepository.GetAllAuditsAsync();

                    ViewBag.Business = business;
                    ViewBag.Transactions = transactions;
                    ViewBag.Licenses = licenses;
                    ViewBag.Audits = audits;
                }
                else
                {
                    ViewBag.NotFound = $"Business with ID {businessId} not found.";
                }
            }
            else if (searchType == "LicenseID" && int.TryParse(searchValue, out int licenseId))
            {
                var license = await _tradeLicenseRepository.GetTradeLicenseByIdAsync(licenseId);
                if (license != null)
                {
                    var business = await _businessRepository.GetBusinessByIdAsync(license.BusinessID);
                    var audits = await _auditRepository.GetAllAuditsAsync();

                    ViewBag.License = license;
                    ViewBag.Business = business;
                    ViewBag.Audits = audits;
                }
                else
                {
                    ViewBag.NotFound = $"License with ID {licenseId} not found.";
                }
            }
            else if (searchType == "TransactionID" && int.TryParse(searchValue, out int transactionId))
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
                if (transaction != null)
                {
                    var business = await _businessRepository.GetBusinessByIdAsync(transaction.BusinessID);
                    var audits = await _auditRepository.GetAllAuditsAsync();

                    ViewBag.Transaction = transaction;
                    ViewBag.Business = business;
                    ViewBag.Audits = audits;
                }
                else
                {
                    ViewBag.NotFound = $"Transaction with ID {transactionId} not found.";
                }
            }
            else
            {
                ViewBag.NotFound = "Invalid search value. Please enter a valid numeric ID.";
            }

            return View("InvestigationResults");
        }

        // GET: AuditorDashboard/ComplianceReview - Compliance Review
        public async Task<IActionResult> ComplianceReview()
        {
            var complianceRecords = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            var audits = await _auditRepository.GetAllAuditsAsync();

            ViewBag.ComplianceRecords = complianceRecords;
            ViewBag.Audits = audits;

            return View();
        }

        // GET: AuditorDashboard/SubsidyTracking - Subsidy and Program Tracking
        public async Task<IActionResult> SubsidyTracking()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            var totalBudget = await _tradeProgramRepository.GetTotalProgramBudgetAsync();
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();

            ViewBag.Programs = programs;
            ViewBag.TotalBudget = totalBudget;
            ViewBag.Subsidies = subsidies;

            return View();
        }
    }
}
