using Government.API.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBusinessIdAsync(int businessId)
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .Where(t => t.BusinessID == businessId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(string type)
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .Where(t => t.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByStatusAsync(string status)
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.Business)
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalTransactionAmountAsync()
        {
            return await _context.Transactions.SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalTransactionAmountByBusinessIdAsync(int businessId)
        {
            return await _context.Transactions
                .Where(t => t.BusinessID == businessId)
                .SumAsync(t => t.Amount);
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return false;
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
