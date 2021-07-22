using System;

namespace IFPS.Sales.Domain.Model.Interfaces
{
    public interface IEntityPrice
    {
        Price Price { get; set; }
        
        DateTime ValidFrom { get; set; }
        DateTime? ValidTo { get; set; }
    }

    public interface IEntityPrice<TEntity> : IEntityPrice
    {
        TEntity Core { get; set; }

        int? CoreId { get; set; }
    }

    public interface IEntityPrice<TEntity, Guid> : IEntityPrice
    {
        TEntity Core { get; set; }
    }
}
