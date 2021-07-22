using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IDocumentAppService
    {
        Task<List<DocumentListDto>> GetDocumentsAsync(DocumentFilterDto filter);
        Task<int> CreateDocumentAsync(DocumentCreateDto documentDto);
        Task<DocumentDetailsDto> GetDocumentDetailsAsync(int id);
        Task<List<DocumentTypeDto>> GetDocumentTypesAsync();
        Task<List<DocumentFolderTypeDto>> GetDocumentFolderTypesAsync();
        Task<DocumentDetailsDto> GetDocumentAsync(Guid id);
    }
}

