using IFPS.Factory.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> CreateDocumentAsync(Document document);
    }
}
