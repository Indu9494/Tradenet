using Microsoft.EntityFrameworkCore;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Repositories
{
    public class ProgramComplianceRepository : IProgramComplianceRepository
    {
        private readonly AppDbContext _context;

        public ProgramComplianceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramCompliance>> GetAllAsync()
        {
            return await _context.ProgramCompliances
                .Include(p => p.ReviewedByOfficer)
                .OrderByDescending(p => p.ReviewedAt)
                .ToListAsync();
        }

        public async Task<ProgramCompliance?> GetByIdAsync(int id)
        {
            return await _context.ProgramCompliances
                .Include(p => p.ReviewedByOfficer)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(ProgramCompliance programCompliance)
        {
            _context.ProgramCompliances.Add(programCompliance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProgramCompliance programCompliance)
        {
            _context.ProgramCompliances.Update(programCompliance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ProgramCompliances.FindAsync(id);
            if (entity is not null)
            {
                _context.ProgramCompliances.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
