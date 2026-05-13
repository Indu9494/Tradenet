using Microsoft.EntityFrameworkCore;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Repositories
{
    public class ComplianceOfficerRepository : IComplianceOfficerRepository
    {
        private readonly AppDbContext _context;

        public ComplianceOfficerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ComplianceOfficer?> GetByUsernameAsync(string username)
        {
            return await _context.ComplianceOfficers
                .FirstOrDefaultAsync(o => o.Username == username && o.IsActive);
        }

        public async Task<ComplianceOfficer?> GetByIdAsync(int id)
        {
            return await _context.ComplianceOfficers.FindAsync(id);
        }
    }
}
