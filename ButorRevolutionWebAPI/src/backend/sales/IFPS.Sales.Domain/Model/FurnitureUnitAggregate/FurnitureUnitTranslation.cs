using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using ENCO.DDD.Domain.Model.Enums;
using System;

namespace IFPS.Sales.Domain.Model
{
    public class FurnitureUnitTranslation : FullAuditedAggregateRoot, IEntityTranslation<FurnitureUnit, Guid>
    {
        /// <summary>
        /// Description of the furniture unit
        /// </summary>
        public string Description { get; set; }

        public FurnitureUnit Core { get; set; }
        public Guid CoreId { get; set; }
        public LanguageTypeEnum Language { get; set; }
    }    
}
