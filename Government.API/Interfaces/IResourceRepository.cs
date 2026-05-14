using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource?> GetResourceByIdAsync(int resourceId);
        Task<IEnumerable<Resource>> GetResourcesByProgramIdAsync(int programId);
        Task<IEnumerable<Resource>> GetResourcesByTypeAsync(string type);
        Task<IEnumerable<Resource>> GetResourcesByStatusAsync(string status);
        Task<int> GetTotalResourceQuantityByProgramIdAsync(int programId);
        Task CreateResourceAsync(Resource resource);
        Task UpdateResourceAsync(Resource resource);
        Task DeleteResourceAsync(int resourceId);
    }
}

