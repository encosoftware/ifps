using Autofac;
using Autofac.Extras.AggregateService;
using ENCO.DDD.Service;
using IFPS.Integration.Application.Services;
using System.Linq;

namespace IFPS.Integration.API.Bootstrap.AutofacModules
{
    public class AppAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<IApplicationServiceDependencyAggregate>();

            builder.RegisterAssemblyTypes(typeof(SynchronizationAppService).Assembly)
                .Where(t => t.Name.EndsWith("AppService"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Factory.Application.Services.ConcreteFurnitureComponentAppService).Assembly)
                .Where(t => t.Name.EndsWith("AppService"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}