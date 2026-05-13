using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IAuditRepository
    {
        Task<IEnumerable<Audit>> GetAllAsync();
        Task<Audit?> GetByIdAsync(int id);
        Task AddAsync(Audit audit);
        Task UpdateAsync(Audit audit);
        Task DeleteAsync(int id);
    }
}
