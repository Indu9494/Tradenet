using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId);
    Task<IEnumerable<Transaction>> GetTransactionsByBusinessIdAsync(int businessId);
}