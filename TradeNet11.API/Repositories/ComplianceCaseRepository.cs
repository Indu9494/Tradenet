using Microsoft.EntityFrameworkCore;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Repositories
{
    public class ComplianceCaseRepository : IComplianceCaseRepository
    {
        private readonly AppDbContext _context;

        public ComplianceCaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplianceCase>> GetAllAsync()
        {
            return await _context.ComplianceCases
                .Include(c => c.AssignedOfficer)
                .OrderByDescending(c => c.ReportedAt)
                .ToListAsync();
        }

        public async Task<ComplianceCase?> GetByIdAsync(int id)
        {
            return await _context.ComplianceCases
                .Include(c => c.AssignedOfficer)
                .Include(c => c.Records)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(ComplianceCase complianceCase)
        {
            _context.ComplianceCases.Add(complianceCase);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ComplianceCase complianceCase)
        {
            _context.ComplianceCases.Update(complianceCase);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ComplianceCases.FindAsync(id);
            if (entity is not null)
            {
                _context.ComplianceCases.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
