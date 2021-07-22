using System.Collections.Generic;

namespace IFPS.Factory.Domain.Model.Interfaces
{
    public interface IPricedEntity<TPrice>
        where TPrice : class, IEntityPrice
    {
        IEnumerable<TPrice> Prices { get; }
    }
}