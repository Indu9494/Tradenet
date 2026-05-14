using Goverment.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class TradeProgramRepository : ITradeProgramRepository
    {
        private readonly AppDbContext _context;

        public TradeProgramRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TradeProgram>> GetAllTradeProgramsAsync()
        {
            return await _context.TradePrograms
                .Include(tp => tp.Resources)
                .ToListAsync();
        }

        public async Task<TradeProgram?> GetTradeProgramByIdAsync(int programId)
        {
            return await _context.TradePrograms
                .Include(tp => tp.Resources)
                .FirstOrDefaultAsync(tp => tp.ProgramID == programId);
        }

        public async Task<IEnumerable<TradeProgram>> GetTradeProgramsByStatusAsync(string status)
        {
            return await _context.TradePrograms
                .Include(tp => tp.Resources)
                .Where(tp => tp.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeProgram>> GetActiveTradeProgramsAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.TradePrograms
                .Include(tp => tp.Resources)
                .Where(tp => tp.StartDate <= currentDate && tp.EndDate >= currentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeProgram>> GetTradeProgramsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.TradePrograms
                .Include(tp => tp.Resources)
                .Where(tp => tp.StartDate >= startDate && tp.EndDate <= endDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalProgramBudgetAsync()
        {
            return await _context.TradePrograms.SumAsync(tp => tp.Budget);
        }

        public async Task<decimal> GetProgramBudgetByIdAsync(int programId)
        {
            var program = await _context.TradePrograms
                .FirstOrDefaultAsync(tp => tp.ProgramID == programId);
            return program?.Budget ?? 0m;
        }

        public async Task<TradeProgram> CreateTradeProgramAsync(TradeProgram program)
        {
            _context.TradePrograms.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<TradeProgram> UpdateTradeProgramAsync(TradeProgram program)
        {
            _context.TradePrograms.Update(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> DeleteTradeProgramAsync(int programId)
        {
            var program = await _context.TradePrograms.FindAsync(programId);
            if (program == null) return false;
            _context.TradePrograms.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
