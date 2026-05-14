using Goverment.Models;

namespace Goverment.Interfaces
{
    public interface IComplianceRecordRepository
    {
        Task<IEnumerable<ComplianceRecord>> GetAllComplianceRecordsAsync();
        Task<ComplianceRecord?> GetComplianceRecordByIdAsync(int complianceId);
        Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByTypeAsync(string type);
        Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByEntityIdAsync(int entityId);
        Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByResultAsync(string result);
        Task<IEnumerable<ComplianceRecord>> GetComplianceRecordsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task CreateComplianceRecordAsync(ComplianceRecord record);
        Task UpdateComplianceRecordAsync(ComplianceRecord record);
        Task DeleteComplianceRecordAsync(int complianceId);
    }
}
