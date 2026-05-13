using Microsoft.AspNetCore.Mvc;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;
using Tradenet_ProgramManager_2.API.Repositories;
using Tradenet_ProgramManager_2.API.Services;

namespace Tradenet_ProgramManager_2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeProgramController : ControllerBase
    {
        private readonly ITradeProgramRepository _tradeProgramRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITradeProgramService _tradeProgramService;

        public TradeProgramController(
            ITradeProgramRepository tradeProgramRepository,
            ITransactionRepository transactionRepository,
            ITradeProgramService tradeProgramService)
        {
            _tradeProgramRepository = tradeProgramRepository;
            _transactionRepository = transactionRepository;
            _tradeProgramService = tradeProgramService;
        }

        /// <summary>
        /// Get all trade programs.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeProgram>>> GetAllPrograms()
        {
            var programs = await _tradeProgramRepository.GetAll();
            return Ok(programs);
        }

        /// <summary>
        /// Get a single trade program by ID.
        /// Returns 404 if program not found.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeProgram>> GetProgramById(int id)
        {
            var program = await _tradeProgramRepository.GetById(id);
            if (program == null)
            {
                return NotFound(new { message = "Program not found" });
            }
            return Ok(program);
        }

        /// <summary>
        /// Create a new trade program.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TradeProgram>> CreateProgram([FromBody] TradeProgram tradeProgram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeProgramRepository.Add(tradeProgram);
            return CreatedAtAction(nameof(GetProgramById), new { id = tradeProgram.Id }, tradeProgram);
        }

        /// <summary>
        /// Update an existing trade program.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromBody] TradeProgram tradeProgram)
        {
            if (id != tradeProgram.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeProgramRepository.Update(tradeProgram);
            return Ok(new { message = "Program updated successfully" });
        }

        /// <summary>
        /// Delete a trade program by ID.
        /// Returns 404 if program not found.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var program = await _tradeProgramRepository.GetById(id);
            if (program == null)
            {
                return NotFound(new { message = "Program not found" });
            }

            await _tradeProgramRepository.Delete(id);
            return Ok(new { message = "Program deleted successfully" });
        }

        /// <summary>
        /// Get dashboard data including total programs, budget used, sales, purchases, and net balance.
        /// Exception handling is delegated to global exception handler middleware.
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardViewModel>> GetDashboardData()
        {
            var programs = await _tradeProgramRepository.GetAll();
            var transactions = await _transactionRepository.GetAll();

            var dashboard = new DashboardViewModel
            {
                TotalPrograms = programs.Count(),
                BudgetUsed = programs.Sum(p => p.Budget),
                MarketHealth = "Good",
                TotalSales = transactions.Where(t => t.Type == "Sale").Sum(t => t.Amount),
                TotalPurchases = transactions.Where(t => t.Type == "Purchase").Sum(t => t.Amount),
                NetBalance = transactions.Where(t => t.Type == "Sale").Sum(t => t.Amount) - 
                            transactions.Where(t => t.Type == "Purchase").Sum(t => t.Amount)
            };

            return Ok(dashboard);
        }
    }
}
