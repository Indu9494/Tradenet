using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class BusinessDocumentRepository : GenericRepository<BusinessDocument>, IBusinessDocumentRepository
    {
        public BusinessDocumentRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BusinessDocument>> GetDocumentsByBusinessIdAsync(int businessId)
        {
            return await _dbSet.Where(d => d.BusinessID == businessId).AsNoTracking().ToListAsync();
        }
    }
}
