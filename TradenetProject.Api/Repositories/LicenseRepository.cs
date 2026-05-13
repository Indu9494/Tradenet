using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Repositories
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly AppDbContext _context;

        public LicenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<TradeLicense> GetAll() => _context.TradeLicenses.ToList();

        public TradeLicense GetById(int id) =>
            _context.TradeLicenses.FirstOrDefault(l => l.LicenseID == id);

        public void Add(TradeLicense entity)
        {
            _context.TradeLicenses.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TradeLicense entity)
        {
            _context.TradeLicenses.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var license = GetById(id);
            if (license != null)
            {
                _context.TradeLicenses.Remove(license);
                _context.SaveChanges();
            }
        }
    }
}
