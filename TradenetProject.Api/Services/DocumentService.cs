using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repository;

        public DocumentService(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public List<Document> GetAllDocuments() => _repository.GetAll();

        public Document GetDocumentById(int id) => _repository.GetById(id);

        public void AddDocument(Document document) => _repository.Add(document);

        public void UpdateDocument(Document document) => _repository.Update(document);

        public void DeleteDocument(int id) => _repository.Delete(id);

        public void ApproveDocument(int id)
        {
            var doc = _repository.GetById(id);
            if (doc != null)
            {
                doc.Status = "Verified";
                doc.VerifiedBy = "Trade Officer";
                _repository.Update(doc);
            }
        }

        public void RejectDocument(int id)
        {
            var doc = _repository.GetById(id);
            if (doc != null)
            {
                doc.Status = "Rejected";
                doc.VerifiedBy = "Trade Officer";
                _repository.Update(doc);
            }
        }
    }
}
