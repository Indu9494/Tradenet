using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class TradeLicenseController : Controller
    {
        private readonly ITradeLicenseRepository _tradeLicenseRepository;

        public TradeLicenseController(ITradeLicenseRepository tradeLicenseRepository)
        {
            _tradeLicenseRepository = tradeLicenseRepository;
        }

        // GET: TradeLicense
        public async Task<IActionResult> Index()
        {
            var tradeLicense = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            return View(tradeLicense);
        }

        // GET: TradeLicense/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeLicense = await _tradeLicenseRepository.GetTradeLicenseByIdAsync(id.Value);
            if (tradeLicense == null)
            {
                return NotFound();
            }

            return View(tradeLicense);
        }

        // GET: TradeLicense/ByBusiness/5
        public async Task<IActionResult> ByBusiness(int businessId)
        {
            var tradeLicenses = await _tradeLicenseRepository.GetTradeLicensesByBusinessIdAsync(businessId);
            return View("Index", tradeLicenses);
        }

        // GET: TradeLicense/ByType/Import
        public async Task<IActionResult> ByType(string type)
        {
            var tradeLicenses = await _tradeLicenseRepository.GetTradeLicensesByTypeAsync(type);
            return View("Index", tradeLicenses);
        }

        // GET: TradeLicense/ByStatus/Active
        public async Task<IActionResult> ByStatus(string status)
        {
            var tradeLicenses = await _tradeLicenseRepository.GetTradeLicensesByStatusAsync(status);
            return View("Index", tradeLicenses);
        }

        // GET: TradeLicense/Expired
        public async Task<IActionResult> Expired()
        {
            var tradeLicenses = await _tradeLicenseRepository.GetExpiredTradeLicensesAsync();
            return View("Index", tradeLicenses);
        }

        // GET: TradeLicense/Expiring?days=30
        public async Task<IActionResult> Expiring(int days = 30)
        {
            var tradeLicenses = await _tradeLicenseRepository.GetExpiringTradeLicensesAsync(days);
            ViewBag.DaysThreshold = days;
            return View("Index", tradeLicenses);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: TradeLicense/Api/GetAll
        [HttpGet]
        [Route("TradeLicense/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var licenses = await _tradeLicenseRepository.GetAllTradeLicensesAsync();
            return Json(licenses);
        }

        // GET: TradeLicense/Api/GetById/5
        [HttpGet]
        [Route("TradeLicense/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var license = await _tradeLicenseRepository.GetTradeLicenseByIdAsync(id);
            if (license == null)
            {
                return Json(new { success = false, message = $"License with ID {id} not found." });
            }
            return Json(new { success = true, data = license });
        }

        // GET: TradeLicense/Api/GetByBusiness/5
        [HttpGet]
        [Route("TradeLicense/Api/GetByBusiness/{businessId}")]
        public async Task<IActionResult> ApiGetByBusiness(int businessId)
        {
            var licenses = await _tradeLicenseRepository.GetTradeLicensesByBusinessIdAsync(businessId);
            return Json(new { success = true, data = licenses });
        }

        // GET: TradeLicense/Api/GetByType/Export
        [HttpGet]
        [Route("TradeLicense/Api/GetByType/{type}")]
        public async Task<IActionResult> ApiGetByType(string type)
        {
            var licenses = await _tradeLicenseRepository.GetTradeLicensesByTypeAsync(type);
            return Json(new { success = true, data = licenses });
        }

        // GET: TradeLicense/Api/GetByStatus/Active
        [HttpGet]
        [Route("TradeLicense/Api/GetByStatus/{status}")]
        public async Task<IActionResult> ApiGetByStatus(string status)
        {
            var licenses = await _tradeLicenseRepository.GetTradeLicensesByStatusAsync(status);
            return Json(new { success = true, data = licenses });
        }

        // GET: TradeLicense/Api/GetExpired
        [HttpGet]
        [Route("TradeLicense/Api/GetExpired")]
        public async Task<IActionResult> ApiGetExpired()
        {
            var licenses = await _tradeLicenseRepository.GetExpiredTradeLicensesAsync();
            return Json(new { success = true, data = licenses });
        }

        // GET: TradeLicense/Api/GetExpiring?days=30
        [HttpGet]
        [Route("TradeLicense/Api/GetExpiring")]
        public async Task<IActionResult> ApiGetExpiring([FromQuery] int days = 30)
        {
            var licenses = await _tradeLicenseRepository.GetExpiringTradeLicensesAsync(days);
            return Json(new { success = true, data = licenses, daysThreshold = days });
        }
    }
}
