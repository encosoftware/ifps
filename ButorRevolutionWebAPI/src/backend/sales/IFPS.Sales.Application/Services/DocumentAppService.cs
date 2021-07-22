using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IDocumentFolderRepository documentFolderRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IOrderRepository orderRepository;

        public DocumentAppService(IDocumentRepository documentRepository,
            IDocumentFolderRepository documentFolderRepository,
            IDocumentTypeRepository documentTypeRepository,
            IApplicationServiceDependencyAggregate aggregate,
            IOrderRepository orderRepository
            ) : base(aggregate)
        {
            this.documentRepository = documentRepository;
            this.documentFolderRepository = documentFolderRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<DocumentDetailsDto> GetDocumentAsync(Guid id)
        {
            return DocumentDetailsDto.FromModel(await documentRepository.SingleAsync(ent => ent.Id == id));
        }

        public Task<List<DocumentFolderTypeDto>> GetDocumentFolderTypesAsync()
        {
            return documentFolderRepository.GetAllListAsync(x => true, DocumentFolderTypeDto.Projection);
        }

        public Task<List<DocumentTypeDto>> GetDocumentTypesAsync()
        {
            return documentTypeRepository.GetAllListAsync(x => true, DocumentTypeDto.Projection);
        }

        public async Task<List<DocumentDetailsDto>> GetAllDocumentsAsync(Guid orderId)
        {
            await orderRepository.SingleAsync(ent => ent.Id == orderId);
            return (await documentRepository.GetAllListAsync(ent => ent.DocumentGroupVersion.Core.OrderId == orderId))
                .Select(x => DocumentDetailsDto.FromModel(x)).ToList();
        }
    }
}
