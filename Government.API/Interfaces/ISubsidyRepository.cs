using Goverment.Models;

namespace Goverment.Interfaces
{
    public interface ISubsidyRepository
    {
        Task<IEnumerable<Subsidy>> GetAllSubsidiesAsync();
        Task<Subsidy?> GetSubsidyByIdAsync(int id);
        Task<IEnumerable<Subsidy>> GetSubsidiesByBusinessIdAsync(int businessId);
        Task<IEnumerable<Subsidy>> GetSubsidiesByProgramIdAsync(int programId);
        Task<IEnumerable<Subsidy>> GetSubsidiesByStatusAsync(string status);
        Task<IEnumerable<Subsidy>> GetSubsidiesByTypeAsync(string type);
        Task<decimal> GetTotalSubsidyAmountAsync();
        Task<decimal> GetDisbursedSubsidyAmountAsync();
        Task CreateSubsidyAsync(Subsidy subsidy);
        Task UpdateSubsidyAsync(Subsidy subsidy);
        Task DeleteSubsidyAsync(int id);
    }
}
