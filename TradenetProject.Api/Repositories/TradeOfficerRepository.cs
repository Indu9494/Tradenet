using Microsoft.EntityFrameworkCore;
using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Repositories
{
    public class TradeOfficerRepository : ITradeOfficerRepository
    {
        private readonly AppDbContext _context;

        public TradeOfficerRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<TradeOfficer> GetAll() => _context.TradeOfficers.ToList();

        public TradeOfficer GetById(int id) =>
            _context.TradeOfficers.FirstOrDefault(o => o.OfficerID == id);

        public TradeOfficer GetCurrentOfficer() =>
            _context.TradeOfficers.First();

        public void Add(TradeOfficer entity)
        {
            _context.TradeOfficers.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TradeOfficer entity)
        {
            _context.TradeOfficers.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var officer = GetById(id);
            if (officer != null)
            {
                _context.TradeOfficers.Remove(officer);
                _context.SaveChanges();
            }
        }
    }
}
