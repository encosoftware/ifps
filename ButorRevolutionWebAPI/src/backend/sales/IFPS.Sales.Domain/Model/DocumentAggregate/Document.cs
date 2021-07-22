using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class Document : File
    {
        /// <summary>
        /// Type of the file, based on file extension. Like: Picture, Pdf, Excel, etc.
        /// </summary>
        public FileExtensionTypeEnum FileExtensionType { get; set; }

        /// <summary>
        /// Userfriendly name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Type of the document
        /// </summary>
        public DocumentType DocumentType { get; set; }
        public int? DocumentTypeId { get; set; }

        public DocumentGroupVersion DocumentGroupVersion { get; private set; }
        public int? DocumentGroupVersionId { get; private set; }

        /// <summary>
        /// User who uploaded the document
        /// </summary>
        public User UploadedBy { get; set; }
        public int? UploadedById { get; set; }

        protected Document()
        {
        }

        public  Document(string fileName, string extension, string containerName) : base(fileName, extension, containerName)
        {
            this.DocumentType = null;
            this.DocumentGroupVersion = null;
        }

        public Document(string fileName, string extension, string containerName, string displayName, FileExtensionTypeEnum fileExtensionType, DocumentType documentType, User uploadedBy)
            : base(fileName, extension, containerName)
        {
            this.DisplayName = displayName;
            this.DocumentType = documentType;
            this.DocumentGroupVersion = null;
            this.FileExtensionType = fileExtensionType;
            this.UploadedBy = uploadedBy;
        }

        public static Document FromSeedData(string fileName, string extension, string containerName, string displayName, FileExtensionTypeEnum fileExtensionType,
            int documentTypeId, int? uploadedById, int? documentGroupVersionId, Guid id)
        {
            return new Document(fileName, extension, containerName)
            {
                Id = id,
                DisplayName = displayName,
                FileExtensionType = fileExtensionType,
                DocumentTypeId = documentTypeId,
                UploadedById = uploadedById,
                DocumentGroupVersionId = documentGroupVersionId,
            };
        }

        public static Document FromSeedData(string fileName, string extension, string containerName, string displayName, FileExtensionTypeEnum fileExtensionType,
            int documentTypeId, int? uploadedById, DocumentGroupVersion documentGroupVersion, Guid id)
        {
            return new Document(fileName, extension, containerName)
            {
                Id = id,
                DisplayName = displayName,
                FileExtensionType = fileExtensionType,
                DocumentTypeId = documentTypeId,
                UploadedById = uploadedById,
                DocumentGroupVersion = documentGroupVersion,
                DocumentGroupVersionId = documentGroupVersion.Id
            };
        }
    }
}
