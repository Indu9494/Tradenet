using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class LicenseDocumentRepository : GenericRepository<LicenseDocument>, ILicenseDocumentRepository
    {
        public LicenseDocumentRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LicenseDocument>> GetDocumentsByLicenseIdAsync(int licenseId)
        {
            return await _dbSet.Where(d => d.LicenseID == licenseId).AsNoTracking().ToListAsync();
        }
    }
}
