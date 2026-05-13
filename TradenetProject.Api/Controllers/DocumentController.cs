using Microsoft.AspNetCore.Mvc;
using TradeNetProject.Models;
using TradeNetProject.Services;

namespace TradeNetProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Document>> Get()
        {
            return Ok(_service.GetAllDocuments());
        }

        [HttpGet("{id}")]
        public ActionResult<Document> Get(int id)
        {
            var item = _service.GetDocumentById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(Document document)
        {
            _service.AddDocument(document);
            return CreatedAtAction(nameof(Get), new { id = document.DocumentID }, document);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Document document)
        {
            if (id != document.DocumentID) return BadRequest();
            _service.UpdateDocument(document);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteDocument(id);
            return NoContent();
        }
    }
}
