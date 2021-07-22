using Autofac;
using IFPS.Factory.Domain.Serializers.Implementations;
using IFPS.Factory.Domain.Serializers.Interfaces;
using IFPS.Factory.Domain.Services.Implementations;
using IFPS.Factory.Domain.Services.Interfaces;

namespace IFPS.Factory.API.Bootstrap.AutofacModules
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

            builder.RegisterType<IdentityService>().As<IIdentityService>()
                .InstancePerLifetimeScope();
        }
    }
}
