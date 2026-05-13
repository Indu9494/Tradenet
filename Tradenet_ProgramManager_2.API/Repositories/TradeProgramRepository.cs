using Microsoft.EntityFrameworkCore;
using Tradenet_ProgramManager_2.API.Data;
using Tradenet_ProgramManager_2.API.Models;

namespace Tradenet_ProgramManager_2.API.Repositories
{
    public class TradeProgramRepository : ITradeProgramRepository
    {
        private readonly AppDbContext _context;

        public TradeProgramRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all trade programs without tracking for performance optimization.
        /// Read-only operation - no entity modifications expected.
        /// </summary>
        public async Task<IEnumerable<TradeProgram>> GetAll()
        {
            return await _context.TradePrograms
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific trade program by ID with tracking enabled.
        /// Tracking is kept enabled as this entity may be updated or deleted.
        /// </summary>
        public async Task<TradeProgram?> GetById(int id)
        {
            return await _context.TradePrograms.FindAsync(id);
        }

        /// <summary>
        /// Add a new trade program to the database.
        /// </summary>
        public async Task Add(TradeProgram tradeProgram)
        {
            _context.TradePrograms.Add(tradeProgram);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update an existing trade program.
        /// </summary>
        public async Task Update(TradeProgram tradeProgram)
        {
            _context.TradePrograms.Update(tradeProgram);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a trade program by ID.
        /// </summary>
        public async Task Delete(int id)
        {
            var tradeProgram = await _context.TradePrograms.FindAsync(id);
            if (tradeProgram != null)
            {
                _context.TradePrograms.Remove(tradeProgram);
                await _context.SaveChangesAsync();
            }
        }
    }
}
