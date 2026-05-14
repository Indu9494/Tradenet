using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class TradeProgramController : Controller
    {
        private readonly ITradeProgramRepository _tradeProgramRepository;

        public TradeProgramController(ITradeProgramRepository tradeProgramRepository)
        {
            _tradeProgramRepository = tradeProgramRepository;
        }

        // GET: TradeProgram
        public async Task<IActionResult> Index()
        {
            var tradePrograms = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            return View(tradePrograms);
        }

        // GET: TradeProgram/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeProgram = await _tradeProgramRepository.GetTradeProgramByIdAsync(id.Value);
            if (tradeProgram == null)
            {
                return NotFound();
            }

            return View(tradeProgram);
        }

        // GET: TradeProgram/Active
        public async Task<IActionResult> Active()
        {
            var tradePrograms = await _tradeProgramRepository.GetActiveTradeProgramsAsync();
            return View("Index", tradePrograms);
        }

        // GET: TradeProgram/ByStatus/Active
        public async Task<IActionResult> ByStatus(string status)
        {
            var tradePrograms = await _tradeProgramRepository.GetTradeProgramsByStatusAsync(status);
            return View("Index", tradePrograms);
        }

        // GET: TradeProgram/ByDateRange?startDate=2024-01-01&endDate=2024-12-31
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var tradePrograms = await _tradeProgramRepository.GetTradeProgramsByDateRangeAsync(startDate, endDate);
            return View("Index", tradePrograms);
        }

        // GET: TradeProgram/TotalBudget
        public async Task<IActionResult> TotalBudget()
        {
            var totalBudget = await _tradeProgramRepository.GetTotalProgramBudgetAsync();
            ViewBag.TotalBudget = totalBudget;
            return View();
        }

        // GET: TradeProgram/Budget/5
        public async Task<IActionResult> Budget(int id)
        {
            var budget = await _tradeProgramRepository.GetProgramBudgetByIdAsync(id);
            ViewBag.Budget = budget;
            ViewBag.ProgramId = id;
            return View();
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: TradeProgram/Api/GetAll
        [HttpGet]
        [Route("TradeProgram/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            return Json(programs);
        }

        // GET: TradeProgram/Api/GetById/5
        [HttpGet]
        [Route("TradeProgram/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var program = await _tradeProgramRepository.GetTradeProgramByIdAsync(id);
            if (program == null)
            {
                return Json(new { success = false, message = $"Program with ID {id} not found." });
            }
            return Json(new { success = true, data = program });
        }

        // GET: TradeProgram/Api/GetActive
        [HttpGet]
        [Route("TradeProgram/Api/GetActive")]
        public async Task<IActionResult> ApiGetActive()
        {
            var programs = await _tradeProgramRepository.GetActiveTradeProgramsAsync();
            return Json(new { success = true, data = programs });
        }

        // GET: TradeProgram/Api/GetByStatus/Active
        [HttpGet]
        [Route("TradeProgram/Api/GetByStatus/{status}")]
        public async Task<IActionResult> ApiGetByStatus(string status)
        {
            var programs = await _tradeProgramRepository.GetTradeProgramsByStatusAsync(status);
            return Json(new { success = true, data = programs });
        }

        // GET: TradeProgram/Api/GetByDateRange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        [Route("TradeProgram/Api/GetByDateRange")]
        public async Task<IActionResult> ApiGetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var programs = await _tradeProgramRepository.GetTradeProgramsByDateRangeAsync(startDate, endDate);
            return Json(new { success = true, data = programs });
        }

        // GET: TradeProgram/Api/GetTotalBudget
        [HttpGet]
        [Route("TradeProgram/Api/GetTotalBudget")]
        public async Task<IActionResult> ApiGetTotalBudget()
        {
            var totalBudget = await _tradeProgramRepository.GetTotalProgramBudgetAsync();
            return Json(new { success = true, totalBudget });
        }

        // GET: TradeProgram/Api/GetBudget/5
        [HttpGet]
        [Route("TradeProgram/Api/GetBudget/{id}")]
        public async Task<IActionResult> ApiGetBudget(int id)
        {
            var budget = await _tradeProgramRepository.GetProgramBudgetByIdAsync(id);
            return Json(new { success = true, programId = id, budget });
        }
    }
}
