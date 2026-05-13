using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;

namespace Tradenet_ProgramManager_2.API.Services
{
    /// <summary>
    /// Interface for Trade Program Service to enable loose coupling and testability
    /// </summary>
    public interface ITradeProgramService
    {
        /// <summary>
        /// Get all trade programs
        /// </summary>
        Task<IEnumerable<TradeProgram>> GetAllPrograms();

        /// <summary>
        /// Get a specific trade program by ID
        /// </summary>
        Task<TradeProgram?> GetProgramById(int id);

        /// <summary>
        /// Add a new trade program with budget validation
        /// </summary>
        /// <returns>True if successful, false if budget is negative</returns>
        Task<bool> AddProgram(TradeProgram tradeProgram);

        /// <summary>
        /// Update an existing trade program with budget validation
        /// </summary>
        /// <returns>True if successful, false if budget is negative</returns>
        Task<bool> UpdateProgram(TradeProgram tradeProgram);

        /// <summary>
        /// Delete a trade program by ID
        /// </summary>
        Task DeleteProgram(int id);

        /// <summary>
        /// Get dashboard data including total programs, budget, sales, purchases, and net balance
        /// </summary>
        Task<DashboardViewModel> GetDashboardData();

        /// <summary>
        /// Get transaction data including sales and purchase volumes with recent transactions
        /// </summary>
        Task<TransactionViewModel> GetTransactionData();

        /// <summary>
        /// Check if there are any non-compliant trade programs
        /// </summary>
        Task<bool> HasNonCompliantPrograms();
    }
}
