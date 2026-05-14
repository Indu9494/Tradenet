using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface IBusinessRepository
    {
        Task<IEnumerable<Business>> GetAllBusinessesAsync();
        Task<Business?> GetBusinessByIdAsync(int businessId);
        Task<IEnumerable<Business>> GetBusinessesByTypeAsync(string type);
        Task<IEnumerable<Business>> GetBusinessesByStatusAsync(string status);
        Task<Business> AddBusinessAsync(Business business);
        Task<Business> UpdateBusinessAsync(Business business);
        Task<bool> DeleteBusinessAsync(int businessId);
        Task<bool> BusinessExistsAsync(int businessId);
    }
}

