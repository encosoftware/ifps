using Autofac;
using IFPS.Factory.Domain.Serializers.Implementations;
using IFPS.Factory.Domain.Serializers.Interfaces;
using IFPS.Factory.Domain.Services.Implementations;
using IFPS.Factory.Domain.Services.Interfaces;
using IFPS.Factory.FunctionalTests.Services;

namespace IFPS.Factory.FunctionalTests
{
    class TestDomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonByteSerializer>().As<IByteSerializer>();
            builder.RegisterAssemblyTypes(typeof(FileHandlerService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestIdentityService>().As<IIdentityService>()
                .InstancePerLifetimeScope();
        }
    }
}
