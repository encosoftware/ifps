using Autofac;
using ENCO.DDD.EntityFrameworkCore.Relational.UoW;
using ENCO.DDD.UoW;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.EF;
using IFPS.Factory.EF.FileHandling;

namespace IFPS.Factory.FunctionalTests
{
    public class TestEFAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new EfCoreUnitOfWork(c.Resolve<IFPSFactoryTestContext>()))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFPSFactoryTestContext>()).As<IFPSFactoryContext>();

            builder.RegisterAssemblyTypes(typeof(IFPSFactoryTestContext).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocalFileStorage>().As<IFileStorage>();
        }
    }
}