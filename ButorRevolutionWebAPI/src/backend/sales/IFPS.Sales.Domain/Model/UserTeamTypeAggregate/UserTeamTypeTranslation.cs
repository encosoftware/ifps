using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Domain.Model
{
    public class UserTeamTypeTranslation : Entity, IEntityTranslation<UserTeamType>
    {
        public string Name { get; set; }

        public UserTeamType Core { get; private set; }
        public int CoreId { get; private set; }

        public LanguageTypeEnum Language { get; private set; }

        private UserTeamTypeTranslation() { }

        public UserTeamTypeTranslation(string name, int coreId, LanguageTypeEnum language)
        {
            this.Name = name;
            this.CoreId = coreId;
            this.Language = language;
        }

    }
}
