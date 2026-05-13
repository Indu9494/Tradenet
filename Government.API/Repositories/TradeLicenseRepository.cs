using Government.API.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class TradeLicenseRepository : ITradeLicenseRepository
    {
        private readonly AppDbContext _context;

        public TradeLicenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TradeLicense>> GetAllTradeLicensesAsync()
        {
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .ToListAsync();
        }

        public async Task<TradeLicense?> GetTradeLicenseByIdAsync(int licenseId)
        {
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .FirstOrDefaultAsync(tl => tl.LicenseID == licenseId);
        }

        public async Task<IEnumerable<TradeLicense>> GetTradeLicensesByBusinessIdAsync(int businessId)
        {
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .Where(tl => tl.BusinessID == businessId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeLicense>> GetTradeLicensesByTypeAsync(string type)
        {
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .Where(tl => tl.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeLicense>> GetTradeLicensesByStatusAsync(string status)
        {
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .Where(tl => tl.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeLicense>> GetExpiredTradeLicensesAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .Where(tl => tl.ExpiryDate < currentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TradeLicense>> GetExpiringTradeLicensesAsync(int daysThreshold)
        {
            var currentDate = DateTime.Now;
            var thresholdDate = currentDate.AddDays(daysThreshold);
            return await _context.TradeLicenses
                .Include(tl => tl.Business)
                .Where(tl => tl.ExpiryDate >= currentDate && tl.ExpiryDate <= thresholdDate)
                .ToListAsync();
        }

        public async Task CreateTradeLicenseAsync(TradeLicense license)
        {
            _context.TradeLicenses.Add(license);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTradeLicenseAsync(TradeLicense license)
        {
            _context.TradeLicenses.Update(license);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTradeLicenseAsync(int licenseId)
        {
            var license = await _context.TradeLicenses.FindAsync(licenseId);
            if (license != null)
            {
                _context.TradeLicenses.Remove(license);
                await _context.SaveChangesAsync();
            }
        }
    }
}
