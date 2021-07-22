using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class DocumentFolder : FullAuditedAggregateRoot, IMultiLingualEntity<DocumentFolderTranslation>
    {
        /// <summary>
        /// DocumentTypes which belongs to the DocumentFolder
        /// </summary>
        private List<DocumentType> _documentTypes = new List<DocumentType>();
        public IEnumerable<DocumentType> DocumentTypes
        {
            get
            {
                return _documentTypes.AsReadOnly();
            }
            private set
            { }
        }

        public bool IsHistorized { get; set; }

        private List<DocumentFolderTranslation> _translations;
        public ICollection<DocumentFolderTranslation> Translations
        {
            get
            {
                return _translations;
            }
            set
            {
                if (value == null)
                {
                    throw new IFPSDomainException($"Error setting Translations, value is null.");
                }
                _translations = value.ToList();
            }
        }

        public DocumentFolderTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        public DocumentFolder(bool isHistorized)
        {
            _translations = new List<DocumentFolderTranslation>();
            IsHistorized = isHistorized;
        }

        public void AddTranslation(DocumentFolderTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding DocumentTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }

        public bool CanAddDocument(DocumentType documentType)
        {
            Ensure.NotNull(DocumentTypes);
            return DocumentTypes.Any(x => x.Type == documentType.Type);
        }

        public void AddDocumentType(DocumentType type)
        {
            Ensure.NotNull(type);
            _documentTypes.Add(type);
        }
    }
}
