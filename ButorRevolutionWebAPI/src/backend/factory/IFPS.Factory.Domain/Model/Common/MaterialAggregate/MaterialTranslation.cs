using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Enums;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class MaterialTranslation : Entity, IEntityTranslation<Material, Guid>
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public virtual Material Core { get; set; }

        public Guid CoreId { get; set; }

        public LanguageTypeEnum Language { get; set; }
    }
}
