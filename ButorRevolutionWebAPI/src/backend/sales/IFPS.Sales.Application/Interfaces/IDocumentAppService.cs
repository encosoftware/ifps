using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IDocumentAppService
    {
        Task<DocumentDetailsDto> GetDocumentAsync(Guid id);
        Task<List<DocumentTypeDto>> GetDocumentTypesAsync();
        Task<List<DocumentFolderTypeDto>> GetDocumentFolderTypesAsync();
        Task<List<DocumentDetailsDto>> GetAllDocumentsAsync(Guid orderId);
    }
}
