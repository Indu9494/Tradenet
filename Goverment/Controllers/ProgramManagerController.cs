using Goverment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ProgramManagerController : Controller
    {
        private readonly ITradeProgramRepository _tradeProgramRepository;
        private readonly ISubsidyRepository _subsidyRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IBusinessRepository _businessRepository;

        public ProgramManagerController(
            ITradeProgramRepository tradeProgramRepository,
            ISubsidyRepository subsidyRepository,
            IResourceRepository resourceRepository,
            IBusinessRepository businessRepository)
        {
            _tradeProgramRepository = tradeProgramRepository;
            _subsidyRepository = subsidyRepository;
            _resourceRepository = resourceRepository;
            _businessRepository = businessRepository;
        }

        // GET: ProgramManager/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            var resources = await _resourceRepository.GetAllResourcesAsync();

            ViewBag.TotalPrograms = programs.Count();
            ViewBag.ActivePrograms = programs.Count(p => p.Status == "Active");
            ViewBag.TotalBudget = programs.Sum(p => p.Budget);
            ViewBag.PendingSubsidies = subsidies.Count(s => s.Status == "Pending");
            ViewBag.ApprovedSubsidies = subsidies.Count(s => s.Status == "Approved");
            ViewBag.DisbursedAmount = subsidies.Where(s => s.Status == "Disbursed").Sum(s => s.Amount);
            ViewBag.TotalResources = resources.Count();

            return View();
        }

        // GET: ProgramManager/ManagePrograms
        public async Task<IActionResult> ManagePrograms()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            return View(programs);
        }

        // GET: ProgramManager/ReviewSubsidies
        public async Task<IActionResult> ReviewSubsidies()
        {
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            return View(subsidies);
        }

        // POST: ProgramManager/ApproveSubsidy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSubsidy(int id)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy != null)
            {
                subsidy.Status = "Approved";
                subsidy.ApprovalDate = DateTime.Now;
                await _subsidyRepository.UpdateSubsidyAsync(subsidy);
                TempData["Success"] = $"Subsidy #{id} approved successfully!";
            }
            return RedirectToAction("ReviewSubsidies");
        }

        // POST: ProgramManager/RejectSubsidy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSubsidy(int id, string reason)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy != null)
            {
                subsidy.Status = "Rejected";
                subsidy.RejectionReason = reason;
                await _subsidyRepository.UpdateSubsidyAsync(subsidy);
                TempData["Success"] = $"Subsidy #{id} rejected!";
            }
            return RedirectToAction("ReviewSubsidies");
        }

        // POST: ProgramManager/DisburseSubsidy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisburseSubsidy(int id)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy != null && subsidy.Status == "Approved")
            {
                subsidy.Status = "Disbursed";
                subsidy.DisbursementDate = DateTime.Now;
                await _subsidyRepository.UpdateSubsidyAsync(subsidy);
                TempData["Success"] = $"Subsidy #{id} disbursed successfully!";
            }
            return RedirectToAction("ReviewSubsidies");
        }

        // GET: ProgramManager/AllocateResources
        public async Task<IActionResult> AllocateResources()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            var resources = await _resourceRepository.GetAllResourcesAsync();

            ViewBag.Programs = programs;
            ViewBag.Resources = resources;

            return View();
        }

        // =============================================
        // API Endpoints
        // =============================================

        // GET: ProgramManager/Api/GetDashboardStats
        [HttpGet]
        [Route("ProgramManager/Api/GetDashboardStats")]
        public async Task<IActionResult> ApiGetDashboardStats()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            var resources = await _resourceRepository.GetAllResourcesAsync();

            var stats = new
            {
                totalPrograms = programs.Count(),
                activePrograms = programs.Count(p => p.Status == "Active"),
                totalBudget = programs.Sum(p => p.Budget),
                pendingSubsidies = subsidies.Count(s => s.Status == "Pending"),
                approvedSubsidies = subsidies.Count(s => s.Status == "Approved"),
                disbursedAmount = subsidies.Where(s => s.Status == "Disbursed").Sum(s => s.Amount),
                totalResources = resources.Count()
            };

            return Json(new { success = true, data = stats });
        }

        // GET: ProgramManager/Api/GetAllPrograms
        [HttpGet]
        [Route("ProgramManager/Api/GetAllPrograms")]
        public async Task<IActionResult> ApiGetAllPrograms()
        {
            var programs = await _tradeProgramRepository.GetAllTradeProgramsAsync();
            return Json(new { success = true, data = programs });
        }

        // GET: ProgramManager/Api/GetPendingSubsidies
        [HttpGet]
        [Route("ProgramManager/Api/GetPendingSubsidies")]
        public async Task<IActionResult> ApiGetPendingSubsidies()
        {
            var subsidies = await _subsidyRepository.GetSubsidiesByStatusAsync("Pending");
            return Json(new { success = true, data = subsidies });
        }

        // GET: ProgramManager/Api/GetAllSubsidies
        [HttpGet]
        [Route("ProgramManager/Api/GetAllSubsidies")]
        public async Task<IActionResult> ApiGetAllSubsidies()
        {
            var subsidies = await _subsidyRepository.GetAllSubsidiesAsync();
            return Json(new { success = true, data = subsidies });
        }

        // POST: ProgramManager/Api/ApproveSubsidy/{id}
        [HttpPost]
        [Route("ProgramManager/Api/ApproveSubsidy/{id}")]
        public async Task<IActionResult> ApiApproveSubsidy(int id)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy == null)
            {
                return Json(new { success = false, message = $"Subsidy with ID {id} not found." });
            }

            subsidy.Status = "Approved";
            subsidy.ApprovalDate = DateTime.Now;
            await _subsidyRepository.UpdateSubsidyAsync(subsidy);

            return Json(new { success = true, message = $"Subsidy #{id} approved successfully!", data = subsidy });
        }

        // POST: ProgramManager/Api/RejectSubsidy/{id}
        [HttpPost]
        [Route("ProgramManager/Api/RejectSubsidy/{id}")]
        public async Task<IActionResult> ApiRejectSubsidy(int id, [FromBody] RejectRequest request)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy == null)
            {
                return Json(new { success = false, message = $"Subsidy with ID {id} not found." });
            }

            subsidy.Status = "Rejected";
            subsidy.RejectionReason = request?.Reason ?? "Not specified";
            await _subsidyRepository.UpdateSubsidyAsync(subsidy);

            return Json(new { success = true, message = $"Subsidy #{id} rejected!", data = subsidy });
        }

        // POST: ProgramManager/Api/DisburseSubsidy/{id}
        [HttpPost]
        [Route("ProgramManager/Api/DisburseSubsidy/{id}")]
        public async Task<IActionResult> ApiDisburseSubsidy(int id)
        {
            var subsidy = await _subsidyRepository.GetSubsidyByIdAsync(id);
            if (subsidy == null)
            {
                return Json(new { success = false, message = $"Subsidy with ID {id} not found." });
            }

            if (subsidy.Status != "Approved")
            {
                return Json(new { success = false, message = "Only approved subsidies can be disbursed." });
            }

            subsidy.Status = "Disbursed";
            subsidy.DisbursementDate = DateTime.Now;
            await _subsidyRepository.UpdateSubsidyAsync(subsidy);

            return Json(new { success = true, message = $"Subsidy #{id} disbursed successfully!", data = subsidy });
        }

        // GET: ProgramManager/Api/GetAllResources
        [HttpGet]
        [Route("ProgramManager/Api/GetAllResources")]
        public async Task<IActionResult> ApiGetAllResources()
        {
            var resources = await _resourceRepository.GetAllResourcesAsync();
            return Json(new { success = true, data = resources });
        }
    }

    // Request models for API
    public class RejectRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
