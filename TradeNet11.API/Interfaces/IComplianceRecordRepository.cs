using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceRecordRepository
    {
        Task<IEnumerable<ComplianceRecord>> GetByCaseIdAsync(int caseId);
        Task AddAsync(ComplianceRecord record);
    }
}
