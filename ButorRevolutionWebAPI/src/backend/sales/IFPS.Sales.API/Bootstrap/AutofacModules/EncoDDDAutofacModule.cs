using Autofac;
using ENCO.DDD.Timing;
using System;

namespace IFPS.Sales.API.Bootstrap.AutofacModules
{
    public class EncoDDDAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Clock.Provider = ClockProviders.Local;
            builder.RegisterInstance(ClockProviders.Local)
                .As<IClockProvider>()
                .SingleInstance();
        }
    }
}