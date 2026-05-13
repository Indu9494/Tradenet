using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface ILicenseDocumentRepository : IRepository<LicenseDocument>
{
    Task<IEnumerable<LicenseDocument>> GetDocumentsByLicenseIdAsync(int licenseId);
}