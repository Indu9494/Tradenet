using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IProgramComplianceRepository
    {
        Task<IEnumerable<ProgramCompliance>> GetAllAsync();
        Task<ProgramCompliance?> GetByIdAsync(int id);
        Task AddAsync(ProgramCompliance programCompliance);
        Task UpdateAsync(ProgramCompliance programCompliance);
        Task DeleteAsync(int id);
    }
}
