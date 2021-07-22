using System.Collections.Generic;
using System.Linq;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;

namespace IFPS.Sales.Domain.Model
{
    public class UserTeamType : AggregateRoot, IMultiLingualEntity<UserTeamTypeTranslation>
    {
        public UserTeamTypeEnum Type { get; private set; }

        /// <summary>
        /// Private list of translations
        /// </summary>
        private List<UserTeamTypeTranslation> _translations;
        public ICollection<UserTeamTypeTranslation> Translations => _translations.AsReadOnly();
        public UserTeamTypeTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private UserTeamType()
        {
            _translations = new List<UserTeamTypeTranslation>();
        }

        public UserTeamType(UserTeamTypeEnum type) : this()
        {
            this.Type = type;
        }

        public void AddTranslation(UserTeamTypeTranslation translation)
        {
            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding UserTeamTypeTranslation: duplicate language: {translation.Language}");
            }

            this._translations.Add(translation);
        }
    }
}
