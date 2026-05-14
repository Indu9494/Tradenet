using Government.API.Data;
using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Government.API.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext _context;

        public ResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _context.Resources
                .Include(r => r.TradeProgram)
                .ToListAsync();
        }

        public async Task<Resource?> GetResourceByIdAsync(int resourceId)
        {
            return await _context.Resources
                .Include(r => r.TradeProgram)
                .FirstOrDefaultAsync(r => r.ResourceID == resourceId);
        }

        public async Task<IEnumerable<Resource>> GetResourcesByProgramIdAsync(int programId)
        {
            return await _context.Resources
                .Include(r => r.TradeProgram)
                .Where(r => r.ProgramID == programId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetResourcesByTypeAsync(string type)
        {
            return await _context.Resources
                .Include(r => r.TradeProgram)
                .Where(r => r.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetResourcesByStatusAsync(string status)
        {
            return await _context.Resources
                .Include(r => r.TradeProgram)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<int> GetTotalResourceQuantityByProgramIdAsync(int programId)
        {
            return await _context.Resources
                .Where(r => r.ProgramID == programId)
                .SumAsync(r => r.Quantity);
        }

        public async Task CreateResourceAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResourceAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResourceAsync(int resourceId)
        {
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
                await _context.SaveChangesAsync();
            }
        }
    }
}

