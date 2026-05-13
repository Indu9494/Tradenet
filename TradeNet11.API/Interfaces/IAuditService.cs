using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IAuditService
    {
        Task<IEnumerable<Audit>> GetAllAuditsAsync();
        Task<Audit?> GetAuditDetailAsync(int id);
        Task StartAuditAsync(int id);
        Task CompleteAuditAsync(int id, string findings);
        Task CreateAuditAsync(Audit audit);
    }
}
