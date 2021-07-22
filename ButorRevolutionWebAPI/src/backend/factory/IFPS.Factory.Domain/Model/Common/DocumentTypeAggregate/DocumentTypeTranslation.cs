using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class DocumentTypeTranslation : Entity, IEntityTranslation<DocumentType>
    {
        public string Name { get; set; }
        public DocumentType Core { get; private set; }

        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private DocumentTypeTranslation()
        {

        }

        public DocumentTypeTranslation(string name, LanguageTypeEnum language)
        {
            this.Name = name;
            this.Language = language;
        }

        public DocumentTypeTranslation(int coreId, string name, LanguageTypeEnum language) 
            : this(name, language)
        {            
            this.CoreId = coreId;         
        }
    }
}
