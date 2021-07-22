using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;


namespace IFPS.Sales.Domain.Model
{
    public class DocumentFolderTranslation : Entity, IEntityTranslation<DocumentFolder>
    {
        public string Name { get; set; }
        public DocumentFolder Core { get; private set; }

        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private DocumentFolderTranslation()
        {

        }

        public DocumentFolderTranslation(string name, LanguageTypeEnum language)
        {
            this.Name = name;
            this.Language = language;
        }

        public DocumentFolderTranslation(int coreId, string name, LanguageTypeEnum language)
            : this(name, language)
        {
            this.CoreId = coreId;
        }
    }
}
