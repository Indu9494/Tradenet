using Microsoft.AspNetCore.Mvc;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly ITradeProgramRepository _programRepository;

        public ProgramController(ITradeProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeProgram>> GetProgram(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                return NotFound();
            return Ok(program);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeProgram>>> GetAllPrograms()
        {
            var programs = await _programRepository.GetAllAsync();
            return Ok(programs);
        }

        [HttpPost]
        public async Task<ActionResult<TradeProgram>> CreateProgram([FromBody] TradeProgram program)
        {
            // Ensure ProgramID is not explicitly set - let the database generate it
            program.ProgramID = 0;
            await _programRepository.AddAsync(program);
            await _programRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProgram), new { id = program.ProgramID }, program);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromBody] TradeProgram program)
        {
            if (id != program.ProgramID)
                return BadRequest();

            await _programRepository.UpdateAsync(program);
            await _programRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                return NotFound();

            await _programRepository.DeleteAsync(program);
            await _programRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
