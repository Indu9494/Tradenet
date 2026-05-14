using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface ITradeProgramRepository
    {
        Task<IEnumerable<TradeProgram>> GetAllTradeProgramsAsync();
        Task<TradeProgram?> GetTradeProgramByIdAsync(int programId);
        Task<IEnumerable<TradeProgram>> GetTradeProgramsByStatusAsync(string status);
        Task<IEnumerable<TradeProgram>> GetActiveTradeProgramsAsync();
        Task<IEnumerable<TradeProgram>> GetTradeProgramsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalProgramBudgetAsync();
        Task<decimal> GetProgramBudgetByIdAsync(int programId);
        Task<TradeProgram> CreateTradeProgramAsync(TradeProgram program);
        Task<TradeProgram> UpdateTradeProgramAsync(TradeProgram program);
        Task<bool> DeleteTradeProgramAsync(int programId);
    }
}

