using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Domain.Model
{
    public class DocumentType : FullAuditedAggregateRoot, IMultiLingualEntity<DocumentTypeTranslation>
    {        
        /// <summary>
        /// DocumentFolder to which the documentType belongs
        /// </summary>
        public virtual DocumentFolder DocumentFolder { get; set; }
        public int DocumentFolderId { get; set; }

        public DocumentTypeEnum Type { get; set; }

        private List<DocumentTypeTranslation> _translations;
        public ICollection<DocumentTypeTranslation> Translations
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

        public DocumentTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        public DocumentType()
        {
            _translations = new List<DocumentTypeTranslation>();
        }

        public DocumentType(DocumentTypeEnum type, int documentFolderId) : this()
        {
            this.Type = type;
            this.DocumentFolderId = documentFolderId;
        }

        public void AddTranslation(DocumentTypeTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding DocumentTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
