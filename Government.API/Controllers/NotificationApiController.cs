using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationApiController : ControllerBase
    {
        private readonly INotificationRepository _repo;

        public NotificationApiController(INotificationRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> Get()
        {
            var items = await _repo.GetAllNotificationsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> Get(int id)
        {
            var item = await _repo.GetNotificationByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> Post([FromBody] Notification model)
        {
            var created = await _repo.CreateNotificationAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.NotificationID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Notification model)
        {
            if (id != model.NotificationID) return BadRequest();
            await _repo.UpdateNotificationAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteNotificationAsync(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}

