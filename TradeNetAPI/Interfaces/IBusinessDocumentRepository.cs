using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IBusinessDocumentRepository : IRepository<BusinessDocument>
{
    Task<IEnumerable<BusinessDocument>> GetDocumentsByBusinessIdAsync(int businessId);
}