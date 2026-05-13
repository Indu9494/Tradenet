using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface ITradeLicenseRepository : IRepository<TradeLicense>
{
    Task<IEnumerable<TradeLicense>> GetAvailableLicensesAsync();
    Task<TradeLicense?> GetLicenseByBusinessIdAsync(int businessId);
}