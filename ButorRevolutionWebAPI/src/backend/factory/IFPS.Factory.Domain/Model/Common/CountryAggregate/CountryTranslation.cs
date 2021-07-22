using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class CountryTranslation : Entity, IEntityTranslation<Country>
    {
        /// <summary>
        /// Name of the country
        /// </summary>
        public string Name { get; private set; }

        public Country Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private CountryTranslation() { }

        public CountryTranslation(int coreId, string name, LanguageTypeEnum language)
        {
            Name = name;
            CoreId = coreId;
            Language = language;
        }
    }
}
