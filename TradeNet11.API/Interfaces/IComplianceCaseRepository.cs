using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceCaseRepository
    {
        Task<IEnumerable<ComplianceCase>> GetAllAsync();
        Task<ComplianceCase?> GetByIdAsync(int id);
        Task AddAsync(ComplianceCase complianceCase);
        Task UpdateAsync(ComplianceCase complianceCase);
        Task DeleteAsync(int id);
    }
}
