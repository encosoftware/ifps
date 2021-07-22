using System;
using System.Collections.Generic;
using System.Text;

namespace ENCO.DDD.Domain.Model.Entities
{
    public interface IVersionedEntity<TVersion>
        where TVersion : class, IEntityVersion
    {
        int? CurrentVersionId { get; set; }
        TVersion CurrentVersion { get; set; }
        IEnumerable<TVersion> Versions { get; }
    }
}
