using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IDocumentFolderRepository documentFolderRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;

        public DocumentAppService(IDocumentRepository documentRepository,
            IDocumentFolderRepository documentFolderRepository,
            IDocumentTypeRepository documentTypeRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.documentRepository = documentRepository;
            this.documentFolderRepository = documentFolderRepository;
            this.documentTypeRepository = documentTypeRepository;
        }

        public Task<int> CreateDocumentAsync(DocumentCreateDto documentDto)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentDetailsDto> GetDocumentDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentFolderTypeDto>> GetDocumentFolderTypesAsync()
        {
            return documentFolderRepository.GetAllListAsync(x => true, DocumentFolderTypeDto.Projection);
        }

        public Task<List<DocumentListDto>> GetDocumentsAsync(DocumentFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<DocumentTypeDto>> GetDocumentTypesAsync()
        {
            return documentTypeRepository.GetAllListAsync(x => true, DocumentTypeDto.Projection);
        }

        public async Task<DocumentDetailsDto> GetDocumentAsync(Guid id)
        {
            return DocumentDetailsDto.FromModel(await documentRepository.SingleAsync(ent => ent.Id == id));
        }

    }
}
