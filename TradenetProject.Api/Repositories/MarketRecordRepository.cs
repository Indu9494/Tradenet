using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Repositories
{
    public class MarketRecordRepository : IMarketRecordRepository
    {
        private readonly AppDbContext _context;

        public MarketRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MarketRecord> GetAll() => _context.MarketRecords.ToList();

        public MarketRecord GetById(int id) =>
            _context.MarketRecords.FirstOrDefault(m => m.RecordID == id);

        public void Add(MarketRecord entity)
        {
            _context.MarketRecords.Add(entity);
            _context.SaveChanges();
        }

        public void Update(MarketRecord entity)
        {
            _context.MarketRecords.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var record = GetById(id);
            if (record != null)
            {
                _context.MarketRecords.Remove(record);
                _context.SaveChanges();
            }
        }
    }
}
