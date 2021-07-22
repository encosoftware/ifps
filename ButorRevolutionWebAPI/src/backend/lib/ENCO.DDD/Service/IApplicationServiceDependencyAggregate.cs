using System;
using System.Collections.Generic;
using System.Text;
using ENCO.DDD.UoW;

namespace ENCO.DDD.Service
{
    public interface IApplicationServiceDependencyAggregate : IServiceBaseDependencyAggregate
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
