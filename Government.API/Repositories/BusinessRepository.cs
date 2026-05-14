using Government.API.Data;
using Government.API.Interfaces;
using Government.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Government.API.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _context;

        public BusinessRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Business>> GetAllBusinessesAsync()
        {
            return await _context.Businesses.ToListAsync();
        }

        public async Task<Business?> GetBusinessByIdAsync(int businessId)
        {
            return await _context.Businesses
                .FirstOrDefaultAsync(b => b.BusinessID == businessId);
        }

        public async Task<IEnumerable<Business>> GetBusinessesByTypeAsync(string type)
        {
            return await _context.Businesses
                .Where(b => b.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Business>> GetBusinessesByStatusAsync(string status)
        {
            return await _context.Businesses
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<Business> AddBusinessAsync(Business business)
        {
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateBusinessAsync(Business business)
        {
            _context.Entry(business).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<bool> DeleteBusinessAsync(int businessId)
        {
            var business = await _context.Businesses.FindAsync(businessId);
            if (business == null)
            {
                return false;
            }

            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BusinessExistsAsync(int businessId)
        {
            return await _context.Businesses.AnyAsync(e => e.BusinessID == businessId);
        }
    }
}

