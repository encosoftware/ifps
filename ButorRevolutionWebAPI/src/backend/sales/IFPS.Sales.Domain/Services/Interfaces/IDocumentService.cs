using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> CreateDocumentAsync(string containerName, string fileName,  DocumentType documentType, User uploadedBy);
        Task<DocumentGroupVersion> AddDocumentsToExistingVersionAsync(DocumentGroup documentGroup, int documentGroupVersionId, List<Document> documents, DocumentType documentType);
        Task<DocumentGroupVersion> AddDocumentsToNewVersionAsync(DocumentGroup documentGroup, List<Document> documents, DocumentType documentType);
    }
}
