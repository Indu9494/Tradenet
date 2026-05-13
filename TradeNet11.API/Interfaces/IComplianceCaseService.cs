using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceCaseService
    {
        Task<IEnumerable<ComplianceCase>> GetAllCasesAsync();
        Task<ComplianceCase?> GetCaseDetailAsync(int id);
        Task<IEnumerable<ComplianceRecord>> GetCaseRecordsAsync(int caseId);
        Task UpdateCaseStatusAsync(int caseId, string newStatus, int officerId, string? remarks);
    }
}
