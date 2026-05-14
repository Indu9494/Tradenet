using Government.API.Data;
using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Government.API.Repositories
{
    public class ComplianceRecordRepository : IComplianceRecordRepository
    {
        private readonly AppDbContext _context;

        public ComplianceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplianceRecord>> GetAllComplianceRecordsAsync()
        {
            return await _context.ComplianceRecords.ToListAsync();
        }

        public async Task<ComplianceRecord?> GetComplianceRecordByIdAsync(int complianceId)
        {
            return await _context.ComplianceRecords
                .FirstOrDefaultAsync(c => c.ComplianceID == complianceId);
        }

        public async Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByTypeAsync(string type)
        {
            return await _context.ComplianceRecords
                .Where(c => c.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByEntityIdAsync(int entityId)
        {
            return await _context.ComplianceRecords
                .Where(c => c.EntityID == entityId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByResultAsync(string result)
        {
            return await _context.ComplianceRecords
                .Where(c => c.Result == result)
                .ToListAsync();
        }

        public async Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ComplianceRecords
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .ToListAsync();
        }

        public async Task CreateComplianceRecordAsync(ComplianceRecord record)
        {
            _context.ComplianceRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComplianceRecordAsync(ComplianceRecord record)
        {
            _context.ComplianceRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComplianceRecordAsync(int complianceId)
        {
            var record = await _context.ComplianceRecords.FindAsync(complianceId);
            if (record != null)
            {
                _context.ComplianceRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}

