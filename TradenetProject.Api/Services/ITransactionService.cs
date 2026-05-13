using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface ITransactionService
    {
        List<Transaction> GetAllTransactions();
        Transaction GetTransactionById(int id);
        void AddTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(int id);
    }
}
