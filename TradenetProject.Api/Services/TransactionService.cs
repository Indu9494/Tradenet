using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public List<Transaction> GetAllTransactions() => _repository.GetAll();

        public Transaction GetTransactionById(int id) => _repository.GetById(id);

        public void AddTransaction(Transaction transaction) => _repository.Add(transaction);

        public void UpdateTransaction(Transaction transaction) => _repository.Update(transaction);

        public void DeleteTransaction(int id) => _repository.Delete(id);
    }
}
