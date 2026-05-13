using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IBusinessRepository : IRepository<Business>
{
    Task<Business?> GetBusinessByUserIdAsync(int userId);
    Task<IEnumerable<Business>> GetBusinessesByUserIdAsync(int userId);
}