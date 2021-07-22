using Autofac;
using ENCO.DDD.EntityFrameworkCore.Relational.UoW;
using ENCO.DDD.UoW;
using IFPS.Factory.Application.BackgroundJobs;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.EF;
using IFPS.Factory.EF.FileHandling;
using System.Linq;

namespace IFPS.Factory.API.Bootstrap.AutofacModules
{
    public class EFAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new EfCoreUnitOfWork(c.Resolve<IFPSFactoryContext>()))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IFPSFactoryContext).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocalFileStorage>().As<IFileStorage>();
            builder.RegisterType<EmailSenderJob>().As<IEmailSenderJob>();
        }
    }
}
