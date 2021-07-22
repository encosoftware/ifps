using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class DocumentState : FullAuditedAggregateRoot, IMultiLingualEntity<DocumentStateTranslation>
    {
        public DocumentStateEnum State { get; private set; }
        

        private List<DocumentStateTranslation> _translations;
        public ICollection<DocumentStateTranslation> Translations
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

        public DocumentStateTranslation CurrentTranslation => (DocumentStateTranslation)Translations.GetCurrentTranslation();

        private DocumentState()
        {
            _translations = new List<DocumentStateTranslation>();
        }

        public DocumentState(DocumentStateEnum state) : this()
        {
            this.State = state;
        }

        public void AddTranslation(DocumentStateTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding DocumentTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
