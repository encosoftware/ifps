using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class DocumentStateTranslation : Entity, IEntityTranslation<DocumentState>
    {
        public string Name { get; set; }
        public DocumentState Core { get; private set; }

        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private DocumentStateTranslation()
        {

        }

        public DocumentStateTranslation(string name, LanguageTypeEnum language)
        {
            this.Name = name;
            this.Language = language;
        }

        public DocumentStateTranslation(int coreId, string name, LanguageTypeEnum language)
            : this(name, language)
        {
            this.CoreId = coreId;
        }
    }
}
