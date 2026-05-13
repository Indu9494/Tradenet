using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IMarketRecordRepository : IRepository<MarketRecord>
{
    Task<IEnumerable<MarketRecord>> GetMarketRecordsAsync();
}