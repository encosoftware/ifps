using ENCO.DDD.Timing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENCO.DDD.Service
{
    public abstract class ServiceBase
    {
        protected readonly ILogger logger;
        protected readonly IClockProvider clock;

        public ServiceBase(IServiceBaseDependencyAggregate aggregate)
        {
            logger = aggregate.LoggerFactory.CreateLogger(GetType());
            clock = aggregate.Clock;
        }
    }
}
