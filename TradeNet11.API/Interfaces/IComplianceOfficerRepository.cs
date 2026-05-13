using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceOfficerRepository
    {
        Task<ComplianceOfficer?> GetByUsernameAsync(string username);
        Task<ComplianceOfficer?> GetByIdAsync(int id);
    }
}
