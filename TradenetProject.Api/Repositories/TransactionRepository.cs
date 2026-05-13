using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Transaction> GetAll() => _context.Transactions.ToList();

        public Transaction GetById(int id) =>
            _context.Transactions.FirstOrDefault(t => t.TransactionID == id);

        public void Add(Transaction entity)
        {
            _context.Transactions.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Transaction entity)
        {
            _context.Transactions.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var transaction = GetById(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }
}
