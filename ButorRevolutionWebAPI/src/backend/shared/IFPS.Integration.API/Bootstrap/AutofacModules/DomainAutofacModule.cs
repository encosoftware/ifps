using Autofac;
using IFPS.Factory.Domain.Serializers.Implementations;
using IFPS.Factory.Domain.Serializers.Interfaces;
using IFPS.Factory.Domain.Services.Implementations;
using System.Linq;

namespace IFPS.Integration.API.Bootstrap.AutofacModules
{
    public class DomainAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonByteSerializer>().As<IByteSerializer>();
            builder.RegisterAssemblyTypes(typeof(FileHandlerService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<Sales.Domain.Services.Implementations.IdentityService>().As<Sales.Domain.Services.Interfaces.IIdentityService>()
                .InstancePerLifetimeScope();
        }
    }
}