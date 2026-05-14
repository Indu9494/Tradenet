using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface ITradeLicenseRepository
    {
        Task<IEnumerable<TradeLicense>> GetAllTradeLicensesAsync();
        Task<TradeLicense?> GetTradeLicenseByIdAsync(int licenseId);
        Task<IEnumerable<TradeLicense>> GetTradeLicensesByBusinessIdAsync(int businessId);
        Task<IEnumerable<TradeLicense>> GetTradeLicensesByTypeAsync(string type);
        Task<IEnumerable<TradeLicense>> GetTradeLicensesByStatusAsync(string status);
        Task<IEnumerable<TradeLicense>> GetExpiredTradeLicensesAsync();
        Task<IEnumerable<TradeLicense>> GetExpiringTradeLicensesAsync(int daysThreshold);
        Task CreateTradeLicenseAsync(TradeLicense license);
        Task UpdateTradeLicenseAsync(TradeLicense license);
        Task DeleteTradeLicenseAsync(int licenseId);
    }
}

