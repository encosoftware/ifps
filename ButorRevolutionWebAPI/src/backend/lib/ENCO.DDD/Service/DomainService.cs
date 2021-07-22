using System;
using System.Collections.Generic;
using System.Text;

namespace ENCO.DDD.Service
{
    public class DomainService : ServiceBase
    {
        public DomainService(IServiceBaseDependencyAggregate aggregate) : base(aggregate)
        {
        }
    }
}
