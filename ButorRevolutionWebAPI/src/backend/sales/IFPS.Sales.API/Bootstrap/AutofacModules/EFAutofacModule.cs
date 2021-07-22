using Autofac;
using ENCO.DDD.EntityFrameworkCore.Relational.UoW;
using ENCO.DDD.UoW;
using IFPS.Sales.Domain.FileHandling;
using IFPS.Sales.EF;
using IFPS.Sales.Application.BackgroundJobs;
using IFPS.Sales.EF.FileHandling;
using System.Linq;

namespace IFPS.Sales.API.Bootstrap.AutofacModules
{
    public class EFAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>new EfCoreUnitOfWork(c.Resolve<IFPSSalesContext>()))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IFPSSalesContext).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<LocalFileStorage>().As<IFileStorage>();
            builder.RegisterType<EmailSenderJob>().As<IEmailSenderJob>();
        }
    }
}