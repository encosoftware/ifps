using ENCO.DDD;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        #region Extensions-FileTypes Dictionary
        private readonly Dictionary<string, FileExtensionTypeEnum> ExtensionFileType = new Dictionary<string, FileExtensionTypeEnum>()
        {
            {".jpeg", FileExtensionTypeEnum.Picture},
            {".jpg", FileExtensionTypeEnum.Picture },
            {".png", FileExtensionTypeEnum.Picture },
            {".xls", FileExtensionTypeEnum.Spreadsheet },
            {".xlsx", FileExtensionTypeEnum.Spreadsheet },
            {".ods", FileExtensionTypeEnum.Spreadsheet },
            {".doc", FileExtensionTypeEnum.Word },
            {".docx", FileExtensionTypeEnum.Word },
            {".odt", FileExtensionTypeEnum.Word },
            {".pdf", FileExtensionTypeEnum.Pdf },
        };
        #endregion

        private readonly IDocumentRepository documentRepository;
        private readonly IFileHandlerService fileHandlerService;
        private readonly IOrderRepository orderRepository;
        private readonly IDocumentStateRepository documentStateRepository;
        public DocumentService(IDocumentRepository documentRepository,
            IFileHandlerService fileHandlerService,
            IOrderRepository orderRepository,
            IDocumentStateRepository documentStateRepository)
        {
            this.documentRepository = documentRepository;
            this.fileHandlerService = fileHandlerService;
            this.orderRepository = orderRepository;
            this.documentStateRepository = documentStateRepository;
        }
        public Task<Document> CreateDocumentAsync(string containerName, string fileName,  DocumentType documentType, User uploadedBy)
        {
            try
            {
                if (!fileHandlerService.IsFileExist(containerName, fileName))
                {
                    throw new IFPSDomainException($"Document ({containerName},{fileName}) not exists!");
                }
            }
            catch(Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException)
                {
                    throw new IFPSDomainException(ex.Message,ex);
                }
                else
                {
                    throw ex;
                }
            }
            
            var document =  new Document(fileName.ToLower(), Path.GetExtension(fileName).ToLower(), containerName,fileName, GetFileExtensionType(Path.GetExtension(fileName)), documentType, uploadedBy);
            return documentRepository.InsertAsync(document);
        }

        private FileExtensionTypeEnum GetFileExtensionType(string extension)
        {
            return ExtensionFileType.ContainsKey(extension.ToLower()) ? ExtensionFileType[extension.ToLower()] : FileExtensionTypeEnum.None;
        }

        public async Task<DocumentGroupVersion> AddDocumentsToExistingVersionAsync(DocumentGroup documentGroup, int documentGroupVersionId, List<Document> documents, DocumentType documentType)
        {
            var version = await orderRepository.GetDocumentGroupVersionWithIncludesAsync(documentGroup.OrderId, documentGroupVersionId);

            await AddDocumentsToVersionAsync(version, documents, documentType);

            return version;
        }

        public async Task<DocumentGroupVersion> AddDocumentsToNewVersionAsync(DocumentGroup documentGroup, List<Document> documents, DocumentType documentType)
        {
            if (!documentGroup.IsHistorized)
            {
                throw new IFPSDomainException("Can not add version to a non historized document group!");
            }

            var version = new DocumentGroupVersion(documentGroup, await documentStateRepository.SingleAsync(x => x.State == DocumentStateEnum.Empty));
            documentGroup.AddNewVersion(version);

            await AddDocumentsToVersionAsync(version, documents, documentType);

            return version;
        }

        private async Task AddDocumentsToVersionAsync(DocumentGroupVersion version, List<Document> documents, DocumentType documentType)
        {
            Ensure.NotNull(version.Core);
            Ensure.NotNull(version.Core.Order);

            if (!version.CanModifyVersion())
            {
                throw new IFPSDomainException("Can not modify the current version!");
            }
            if (!version.Core.DocumentFolder.CanAddDocument(documentType))
            {
                throw new IFPSDomainException($"Can not add this type ({documentType.Type}) of document in this folder (id: {version.Core.DocumentFolder.Id})!");
            }

            string prefix = version.Core.Order.WorkingNumber.Replace("/", "-") + '_' + documentType.Type.ToString() + '_';
            int count = await documentRepository.GetCountOfSameTypeOfDocumentAsync(version.Core.OrderId, documentType.Id);
            foreach (var doc in documents.Where(x => x.DocumentTypeId == documentType.Id))
            {
                doc.DisplayName = prefix + ++count + doc.Extension;
                version.AddDocument(doc);
                version.LastModificationTime = Clock.Now;
            }

            if (version.Core.IsHistorized)
            {
                version.State = await documentStateRepository.SingleAsync(x => x.State == DocumentStateEnum.WaitingForApproval);
            }
            else
            {
                version.State = await documentStateRepository.SingleAsync(x => x.State == DocumentStateEnum.Uploaded);
            }
        }

    }
}
