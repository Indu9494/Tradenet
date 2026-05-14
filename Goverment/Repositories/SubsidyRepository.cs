using Goverment.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class SubsidyRepository : ISubsidyRepository
    {
        private readonly AppDbContext _context;

        public SubsidyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subsidy>> GetAllSubsidiesAsync()
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .ToListAsync();
        }

        public async Task<Subsidy?> GetSubsidyByIdAsync(int id)
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .FirstOrDefaultAsync(s => s.SubsidyID == id);
        }

        public async Task<IEnumerable<Subsidy>> GetSubsidiesByBusinessIdAsync(int businessId)
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .Where(s => s.BusinessID == businessId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subsidy>> GetSubsidiesByProgramIdAsync(int programId)
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .Where(s => s.ProgramID == programId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subsidy>> GetSubsidiesByStatusAsync(string status)
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subsidy>> GetSubsidiesByTypeAsync(string type)
        {
            return await _context.Subsidies
                .Include(s => s.Business)
                .Include(s => s.TradeProgram)
                .Where(s => s.Type == type)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSubsidyAmountAsync()
        {
            return await _context.Subsidies.SumAsync(s => s.Amount);
        }

        public async Task<decimal> GetDisbursedSubsidyAmountAsync()
        {
            return await _context.Subsidies
                .Where(s => s.Status == "Disbursed")
                .SumAsync(s => s.Amount);
        }

        public async Task CreateSubsidyAsync(Subsidy subsidy)
        {
            _context.Subsidies.Add(subsidy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubsidyAsync(Subsidy subsidy)
        {
            _context.Subsidies.Update(subsidy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubsidyAsync(int id)
        {
            var subsidy = await _context.Subsidies.FindAsync(id);
            if (subsidy != null)
            {
                _context.Subsidies.Remove(subsidy);
                await _context.SaveChangesAsync();
            }
        }
    }
}
