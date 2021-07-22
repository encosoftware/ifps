using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Domain.Model
{
    public class DivisionTranslation : Entity, IEntityTranslation<Division>
    {
        /// <summary>
        /// Name of the division
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the division
        /// </summary>
        public string Description { get; private set; }

        public virtual Division Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private DivisionTranslation() { }

        public DivisionTranslation(int coreId, string name, string description, LanguageTypeEnum language)
        {
            CoreId = coreId;
            Name = name;
            Description = description;
            Language = language;
        }
    }
}
