using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByBusinessIdAsync(int businessId);
        Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(string type);
        Task<IEnumerable<Transaction>> GetTransactionsByStatusAsync(string status);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalTransactionAmountAsync();
        Task<decimal> GetTotalTransactionAmountByBusinessIdAsync(int businessId);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}

