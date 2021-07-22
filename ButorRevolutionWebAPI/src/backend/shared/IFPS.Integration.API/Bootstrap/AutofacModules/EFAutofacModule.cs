using Autofac;
using ENCO.DDD.EntityFrameworkCore.Relational.UoW;
using ENCO.DDD.UoW;
using IFPS.Factory.EF;
using IFPS.Sales.EF;
using IFPS.Factory.EF.FileHandling;
using System.Linq;
using IFPS.Factory.Domain.FileHandling;

namespace IFPS.Integration.API.Bootstrap.AutofacModules
{
    public class EFAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new EfCoreUnitOfWork(c.Resolve<IFPSSalesContext>()))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IFPSSalesContext).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(c => new EfCoreUnitOfWork(c.Resolve<IFPSFactoryContext>()))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IFPSFactoryContext).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocalFileStorage>().As<IFileStorage>();
        }
    }
}