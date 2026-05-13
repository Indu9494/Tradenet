using Microsoft.EntityFrameworkCore;
using Tradenet_ProgramManager_2.API.Data;
using Tradenet_ProgramManager_2.API.Models;

namespace Tradenet_ProgramManager_2.API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all transactions without tracking for performance optimization.
        /// Includes related Program data and orders by most recent first.
        /// Read-only operation - no entity modifications expected.
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Program)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Get transactions by program ID without tracking.
        /// Read-only operation optimized for performance.
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetByProgramId(int programId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.ProgramId == programId)
                .Include(t => t.Program)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Get transactions by type (Sale/Purchase) without tracking.
        /// Read-only operation optimized for performance.
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetByType(string type)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.Type == type)
                .Include(t => t.Program)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Get a specific transaction by ID with tracking enabled.
        /// Tracking is kept enabled as this entity may be updated or deleted.
        /// </summary>
        public async Task<Transaction?> GetById(int id)
        {
            return await _context.Transactions
                .Include(t => t.Program)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Add a new transaction to the database.
        /// </summary>
        public async Task Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get the total amount of transactions by type without tracking.
        /// Aggregation operation - read-only and optimized.
        /// </summary>
        public async Task<decimal> GetTotalByType(string type)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.Type == type)
                .SumAsync(t => t.Amount);
        }
    }
}
