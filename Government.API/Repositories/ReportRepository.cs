using Government.API.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportID == reportId);
        }

        public async Task<IEnumerable<Report>> GetReportsByScopeAsync(string scope)
        {
            return await _context.Reports
                .Where(r => r.Scope == scope)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reports
                .Where(r => r.GeneratedDate >= startDate && r.GeneratedDate <= endDate)
                .ToListAsync();
        }

        public async Task<Report> CreateReportAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<Report> UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return false;
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
