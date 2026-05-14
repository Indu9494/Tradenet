using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TradeNetProject.Repositories
{
    public class ComplianceRepository : IComplianceRepository
    {
        private readonly AppDbContext _context;

        public ComplianceRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<ComplianceRecord> GetAll() => _context.ComplianceRecords.ToList();

        public ComplianceRecord GetById(int id) =>
            _context.ComplianceRecords.FirstOrDefault(r => r.ComplianceID == id);

        public void Add(ComplianceRecord entity)
        {
            _context.ComplianceRecords.Add(entity);
            _context.SaveChanges();
        }

        public void Update(ComplianceRecord entity)
        {
            _context.ComplianceRecords.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var record = GetById(id);
            if (record != null)
            {
                _context.ComplianceRecords.Remove(record);
                _context.SaveChanges();
            }
        }
    }
}
