using ENCO.DDD.Domain.Model.Enums;

namespace ENCO.DDD.Domain.Model.Entities
{
    public interface IEntityTranslation
    {
        LanguageTypeEnum Language { get; }
    }

    public interface IEntityTranslation<TEntity, TPrimaryKeyOfMultiLingualEntity> : IEntityTranslation
    {
        TEntity Core { get; }

        TPrimaryKeyOfMultiLingualEntity CoreId { get; }
    }

    public interface IEntityTranslation<TEntity> : IEntityTranslation<TEntity, int>
    {

    }
}
