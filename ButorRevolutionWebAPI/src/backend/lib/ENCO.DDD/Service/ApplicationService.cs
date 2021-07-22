using System;
using System.Collections.Generic;
using System.Text;
using ENCO.DDD.UoW;

namespace ENCO.DDD.Service
{
    public abstract class ApplicationService : ServiceBase
    {
        protected readonly IUnitOfWork unitOfWork;       

        public ApplicationService(
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            unitOfWork = aggregate.UnitOfWork;
        }
    }
}
