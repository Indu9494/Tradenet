using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // GET: Transaction
        public async Task<IActionResult> Index() // Added 'async' and 'Task<>'
        {
            // RIGHT: 'await' unwraps the Task so 'transactions' becomes IEnumerable<Transaction>
            var Transaction = await _transactionRepository.GetAllTransactionsAsync();
            return View(Transaction);
        }

        // GET: Transaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionRepository.GetTransactionByIdAsync(id.Value);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/ByBusiness/5
        public async Task<IActionResult> ByBusiness(int businessId)
        {
            var transactions = await _transactionRepository.GetTransactionsByBusinessIdAsync(businessId);
            return View("Index", transactions);
        }

        // GET: Transaction/ByType/Sale
        public async Task<IActionResult> ByType(string type)
        {
            var transactions = await _transactionRepository.GetTransactionsByTypeAsync(type);
            return View("Index", transactions);
        }

        // GET: Transaction/ByStatus/Completed
        public async Task<IActionResult> ByStatus(string status)
        {
            var transactions = await _transactionRepository.GetTransactionsByStatusAsync(status);
            return View("Index", transactions);
        }

        // GET: Transaction/ByDateRange?startDate=2024-01-01&endDate=2024-12-31
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(startDate, endDate);
            return View("Index", transactions);
        }

        // GET: Transaction/TotalAmount
        public async Task<IActionResult> TotalAmount()
        {
            var totalAmount = await _transactionRepository.GetTotalTransactionAmountAsync();
            ViewBag.TotalAmount = totalAmount;
            return View();
        }

        // GET: Transaction/TotalByBusiness/5
        public async Task<IActionResult> TotalByBusiness(int businessId)
        {
            var totalAmount = await _transactionRepository.GetTotalTransactionAmountByBusinessIdAsync(businessId);
            ViewBag.TotalAmount = totalAmount;
            ViewBag.BusinessId = businessId;
            return View();
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: Transaction/Api/GetAll
        [HttpGet]
        [Route("Transaction/Api/GetAll")]
        public async Task<IActionResult> ApiGetAll()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return Json(transactions);
        }

        // GET: Transaction/Api/GetById/5
        [HttpGet]
        [Route("Transaction/Api/GetById/{id}")]
        public async Task<IActionResult> ApiGetById(int id)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return Json(new { success = false, message = $"Transaction with ID {id} not found." });
            }
            return Json(new { success = true, data = transaction });
        }

        // GET: Transaction/Api/GetByBusiness/5
        [HttpGet]
        [Route("Transaction/Api/GetByBusiness/{businessId}")]
        public async Task<IActionResult> ApiGetByBusiness(int businessId)
        {
            var transactions = await _transactionRepository.GetTransactionsByBusinessIdAsync(businessId);
            return Json(new { success = true, data = transactions });
        }

        // GET: Transaction/Api/GetByType/Sale
        [HttpGet]
        [Route("Transaction/Api/GetByType/{type}")]
        public async Task<IActionResult> ApiGetByType(string type)
        {
            var transactions = await _transactionRepository.GetTransactionsByTypeAsync(type);
            return Json(new { success = true, data = transactions });
        }

        // GET: Transaction/Api/GetByStatus/Completed
        [HttpGet]
        [Route("Transaction/Api/GetByStatus/{status}")]
        public async Task<IActionResult> ApiGetByStatus(string status)
        {
            var transactions = await _transactionRepository.GetTransactionsByStatusAsync(status);
            return Json(new { success = true, data = transactions });
        }

        // GET: Transaction/Api/GetByDateRange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet]
        [Route("Transaction/Api/GetByDateRange")]
        public async Task<IActionResult> ApiGetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(startDate, endDate);
            return Json(new { success = true, data = transactions });
        }

        // GET: Transaction/Api/GetTotalAmount
        [HttpGet]
        [Route("Transaction/Api/GetTotalAmount")]
        public async Task<IActionResult> ApiGetTotalAmount()
        {
            var totalAmount = await _transactionRepository.GetTotalTransactionAmountAsync();
            return Json(new { success = true, totalAmount });
        }

        // GET: Transaction/Api/GetTotalByBusiness/5
        [HttpGet]
        [Route("Transaction/Api/GetTotalByBusiness/{businessId}")]
        public async Task<IActionResult> ApiGetTotalByBusiness(int businessId)
        {
            var totalAmount = await _transactionRepository.GetTotalTransactionAmountByBusinessIdAsync(businessId);
            return Json(new { success = true, businessId, totalAmount });
        }
    }
}
