using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Division : FullAuditedAggregateRoot, IMultiLingualEntity<DivisionTranslation>
    {
        /// <summary>
        /// Type of the division
        /// </summary>
        public DivisionTypeEnum DivisionType { get; private set; }

        /// <summary>
        /// Private list of claims 
        /// </summary>
        private List<Claim> _claims;
        public IEnumerable<Claim> Claims => _claims.AsEnumerable();

        /// <summary>
        /// Private list for managing translations
        /// </summary>
        private List<DivisionTranslation> _translations;
        public ICollection<DivisionTranslation> Translations => _translations.AsReadOnly();

        public DivisionTranslation CurrentTranslation => Translations.GetCurrentTranslation();

        private Division()
        {
            _claims = new List<Claim>();
            _translations = new List<DivisionTranslation>();
        }

        public Division(DivisionTypeEnum divisionType) :this()
        {
            DivisionType = divisionType;
        }

        public void AddTranslation(DivisionTranslation translation)
        {
            Ensure.NotNull(translation);

            if (_translations.Any(ent => ent.Language.Equals(translation.Language)))
            {
                throw new IFPSDomainException($"Error at adding DivisionTranslation: duplicate language: {translation.Language}");
            }

            _translations.Add(translation);
        }
    }
}
