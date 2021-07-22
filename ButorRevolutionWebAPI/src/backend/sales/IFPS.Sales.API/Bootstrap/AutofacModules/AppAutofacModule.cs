using Autofac;
using Autofac.Extras.AggregateService;
using ENCO.DDD.Service;
using IFPS.Sales.Application.Services;
using System.Linq;

namespace IFPS.Sales.API.Bootstrap.AutofacModules
{
    public class AppAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<IApplicationServiceDependencyAggregate>();

            builder.RegisterAssemblyTypes(typeof(UserAppService).Assembly)
                .Where(t => t.Name.EndsWith("AppService"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}