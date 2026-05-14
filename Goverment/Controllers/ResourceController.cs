using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goverment.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceController(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        // GET: Resource
        public async Task<IActionResult> Index()
        {
            var resources = await _resourceRepository.GetAllResourcesAsync();
            return View(resources);
        }

        // GET: Resource/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _resourceRepository.GetResourceByIdAsync(id.Value);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // GET: Resource/ByProgram/5
        public async Task<IActionResult> ByProgram(int programId)
        {
            var resources = await _resourceRepository.GetResourcesByProgramIdAsync(programId);
            return View("Index", resources);
        }

        // GET: Resource/ByType/Funds
        public async Task<IActionResult> ByType(string type)
        {
            var resources = await _resourceRepository.GetResourcesByTypeAsync(type);
            return View("Index", resources);
        }

        // GET: Resource/ByStatus/Available
        public async Task<IActionResult> ByStatus(string status)
        {
            var resources = await _resourceRepository.GetResourcesByStatusAsync(status);
            return View("Index", resources);
        }

        // GET: Resource/TotalQuantityByProgram/5
        public async Task<IActionResult> TotalQuantityByProgram(int programId)
        {
            var totalQuantity = await _resourceRepository.GetTotalResourceQuantityByProgramIdAsync(programId);
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.ProgramId = programId;
            return View();
        }
    }
}
