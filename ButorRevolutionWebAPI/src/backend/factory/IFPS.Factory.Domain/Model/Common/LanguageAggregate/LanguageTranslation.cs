using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class LanguageTranslation : Entity, IEntityTranslation<Language>
    {
        /// <summary>
        /// Name of the language
        /// </summary>
        public string LanguageName { get; set; }

        public Language Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        public LanguageTranslation() { }

        public LanguageTranslation(string name, int coreId, LanguageTypeEnum language)
        {
            LanguageName = name;
            CoreId = coreId;
            Language = language;
        }
    }
}