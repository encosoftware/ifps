using Autofac;
using Autofac.Extras.AggregateService;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Services;
using IFPS.Factory.Domain.Services.Implementations;
using IFPS.Factory.Domain.Services.Interfaces;

namespace IFPS.Factory.API.Bootstrap.AutofacModules
{
    public class AppAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterAggregateService<IApplicationServiceDependencyAggregate>();

            builder.RegisterAssemblyTypes(typeof(GeneralExpensesAppService).Assembly)
                .Where(t => t.Name.EndsWith("AppService"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
