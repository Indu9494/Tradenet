using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        // GET: Business
        public async Task<IActionResult> Index()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            return View(businesses);
        }

        // GET: Business/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _businessRepository.GetBusinessByIdAsync(id.Value);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // GET: Business/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Business/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessID,Name,Type,Address,ContactInfo,Status")] Business business)
        {
            if (ModelState.IsValid)
            {
                await _businessRepository.AddBusinessAsync(business);
                return RedirectToAction(nameof(Index));
            }
            return View(business);
        }

        // GET: Business/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _businessRepository.GetBusinessByIdAsync(id.Value);
            if (business == null)
            {
                return NotFound();
            }
            return View(business);
        }

        // POST: Business/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessID,Name,Type,Address,ContactInfo,Status")] Business business)
        {
            if (id != business.BusinessID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _businessRepository.UpdateBusinessAsync(business);
                return RedirectToAction(nameof(Index));
            }
            return View(business);
        }

        // GET: Business/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var business = await _businessRepository.GetBusinessByIdAsync(id.Value);
            if (business == null)
            {
                return NotFound();
            }

            return View(business);
        }

        // POST: Business/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _businessRepository.DeleteBusinessAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Business/ByType/Trader
        public async Task<IActionResult> ByType(string type)
        {
            var businesses = await _businessRepository.GetBusinessesByTypeAsync(type);
            return View("Index", businesses);
        }

        // GET: Business/ByStatus/Active
        public async Task<IActionResult> ByStatus(string status)
        {
            var businesses = await _businessRepository.GetBusinessesByStatusAsync(status);
            return View("Index", businesses);
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: Business/Api/GetAll
        [HttpGet]
        [Route("Business/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var businesses = await _businessRepository.GetAllBusinessesAsync();
            return Json(businesses);
        }

        // GET: Business/Api/GetById/5
        [HttpGet]
        [Route("Business/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var business = await _businessRepository.GetBusinessByIdAsync(id);
            if (business == null)
            {
                return Json(new { success = false, message = $"Business with ID {id} not found." });
            }
            return Json(new { success = true, data = business });
        }

        // GET: Business/Api/GetByType/Exporter
        [HttpGet]
        [Route("Business/Api/GetByType/{type}")]
        public async Task<IActionResult> ApiGetByType(string type)
        {
            var businesses = await _businessRepository.GetBusinessesByTypeAsync(type);
            return Json(new { success = true, data = businesses });
        }

        // GET: Business/Api/GetByStatus/Active
        [HttpGet]
        [Route("Business/Api/GetByStatus/{status}")]
        public async Task<IActionResult> ApiGetByStatus(string status)
        {
            var businesses = await _businessRepository.GetBusinessesByStatusAsync(status);
            return Json(new { success = true, data = businesses });
        }
    }
}
