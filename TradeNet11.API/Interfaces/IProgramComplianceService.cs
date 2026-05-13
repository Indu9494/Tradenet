using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IProgramComplianceService
    {
        Task<IEnumerable<ProgramCompliance>> GetAllAsync();
        Task<ProgramCompliance?> GetByIdAsync(int id);
        Task UpdateEligibilityAsync(int id, string status, bool misuseFlag, string? remarks);
    }
}
