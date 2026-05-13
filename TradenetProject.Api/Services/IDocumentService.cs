using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface IDocumentService
    {
        List<Document> GetAllDocuments();
        Document GetDocumentById(int id);
        void AddDocument(Document document);
        void UpdateDocument(Document document);
        void DeleteDocument(int id);
        void ApproveDocument(int id);
        void RejectDocument(int id);
    }
}
