using Microsoft.EntityFrameworkCore;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Repositories
{
    public class ComplianceRecordRepository : IComplianceRecordRepository
    {
        private readonly AppDbContext _context;

        public ComplianceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplianceRecord>> GetByCaseIdAsync(int caseId)
        {
            return await _context.ComplianceRecords
                .Where(r => r.ComplianceCaseId == caseId)
                .Include(r => r.Officer)
                .OrderByDescending(r => r.Timestamp)
                .ToListAsync();
        }

        public async Task AddAsync(ComplianceRecord record)
        {
            _context.ComplianceRecords.Add(record);
            await _context.SaveChangesAsync();
        }
    }
}
