using ENCO.DDD.Timing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENCO.DDD.Service
{
    public interface IServiceBaseDependencyAggregate
    {
        ILoggerFactory LoggerFactory { get; }
        IClockProvider Clock { get; }
    }
}
