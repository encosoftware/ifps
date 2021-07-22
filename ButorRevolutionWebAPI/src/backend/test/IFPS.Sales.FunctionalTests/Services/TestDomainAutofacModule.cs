using Autofac;
using IFPS.Sales.Domain.Serializers.Implementations;
using IFPS.Sales.Domain.Serializers.Interfaces;
using IFPS.Sales.Domain.Services.Implementations;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.FunctionalTests.Services
{
    public class TestDomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonByteSerializer>().As<IByteSerializer>();
            builder.RegisterType<MessageHub>().As<MessageHub>();
            builder.RegisterAssemblyTypes(typeof(DocumentService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestIdentityService>().As<IIdentityService>()
                .InstancePerLifetimeScope();
        }
    }
}
