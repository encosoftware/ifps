using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }
        public Task<Document> CreateDocumentAsync(Document document)
        {
            return documentRepository.InsertAsync(document);
        }
    }
}
