using Government.API.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Audit>> GetAllAuditsAsync()
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .ToListAsync();
        }

        public async Task<Audit?> GetAuditByIdAsync(int auditId)
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .FirstOrDefaultAsync(a => a.AuditID == auditId);
        }

        public async Task<IEnumerable<Audit>> GetAuditsByOfficerIdAsync(int officerId)
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .Where(a => a.OfficerID == officerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByStatusAsync(string status)
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByScopeAsync(string scope)
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .Where(a => a.Scope == scope)
                .ToListAsync();
        }

        public async Task<IEnumerable<Audit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Audits
                .Include(a => a.Officer)
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .ToListAsync();
        }

        public async Task<Audit> CreateAuditAsync(Audit audit)
        {
            _context.Audits.Add(audit);
            await _context.SaveChangesAsync();
            return audit;
        }

        public async Task<Audit> UpdateAuditAsync(Audit audit)
        {
            _context.Audits.Update(audit);
            await _context.SaveChangesAsync();
            return audit;
        }

        public async Task<bool> DeleteAuditAsync(int auditId)
        {
            var audit = await _context.Audits.FindAsync(auditId);
            if (audit == null) return false;
            _context.Audits.Remove(audit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
