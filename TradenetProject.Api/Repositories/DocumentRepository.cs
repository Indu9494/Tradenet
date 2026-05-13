using TradeNetProject.Data;
using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;

        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Document> GetAll() => _context.Documents.ToList();

        public Document GetById(int id) =>
            _context.Documents.FirstOrDefault(d => d.DocumentID == id);

        public void Add(Document entity)
        {
            _context.Documents.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Document entity)
        {
            _context.Documents.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var doc = GetById(id);
            if (doc != null)
            {
                _context.Documents.Remove(doc);
                _context.SaveChanges();
            }
        }
    }
}
