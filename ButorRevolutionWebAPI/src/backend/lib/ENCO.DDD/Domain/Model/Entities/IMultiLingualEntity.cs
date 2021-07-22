using System.Collections.Generic;

namespace ENCO.DDD.Domain.Model.Entities
{
    public interface IMultiLingualEntity<TTranslation>
        where TTranslation : class, IEntityTranslation
    {
        ICollection<TTranslation> Translations { get; }

        TTranslation CurrentTranslation { get; }
    }
}
