using Goverment.Models;

namespace Goverment.Interfaces
{
    public interface IAuditRepository
    {
        Task<IEnumerable<Audit>> GetAllAuditsAsync();
        Task<Audit?> GetAuditByIdAsync(int auditId);
        Task<IEnumerable<Audit>> GetAuditsByOfficerIdAsync(int officerId);
        Task<IEnumerable<Audit>> GetAuditsByStatusAsync(string status);
        Task<IEnumerable<Audit>> GetAuditsByScopeAsync(string scope);
        Task<IEnumerable<Audit>> GetAuditsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Audit> CreateAuditAsync(Audit audit);
        Task<Audit> UpdateAuditAsync(Audit audit);
        Task<bool> DeleteAuditAsync(int auditId);
    }
}
