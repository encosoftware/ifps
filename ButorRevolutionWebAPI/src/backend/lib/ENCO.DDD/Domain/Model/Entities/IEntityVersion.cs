using System;
using System.Collections.Generic;
using System.Text;

namespace ENCO.DDD.Domain.Model.Entities
{
    public interface IEntityVersion
    {
        DateTime ValidFrom { get; }

        DateTime? ValidTo { get; }
    }

    public interface IEntityVersion<TEntity> : IEntityVersion
    {
        TEntity Core { get; }
        int? CoreId { get; }
    }
}
