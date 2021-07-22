using Autofac;
using Autofac.Extensions.DependencyInjection;
using IFPS.Factory.API;
using IFPS.Factory.API.Bootstrap.AutofacModules;
using IFPS.Factory.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace IFPS.Factory.FunctionalTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env)
        {
        }

        public override IServiceProvider SetupAutofac(IServiceCollection services)
        {
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new EncoDDDAutofacModule());
            container.RegisterModule(new AppAutofacModule());
            container.RegisterModule(new TestDomainAutofacModule());
            container.RegisterModule(new TestEFAutofacModule());

            return new AutofacServiceProvider(container.Build());
        }
        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            base.Configure(app, env);

            var cultureInfo = new CultureInfo("hu-HU");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public override void ConfigureDbContext(IServiceCollection services, bool isDevelopment = false)
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            services.AddDbContext<IFPSFactoryTestContext>(options =>
            {
                options.UseSqlite(connection);
                options.EnableSensitiveDataLogging();
            });

            //Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<IFPSFactoryTestContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<IFPSFactoryWebApplicationFactory>>();

                db.Database.OpenConnection();
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred seeding the " +
                        "database with test messages. Error: {ex.Message}");
                    throw ex;
                }
            }
        }

        public override void ConfigureHangfireDbContext(IServiceCollection services, bool isDevelopment = false)
        {

        }

        public override void AddHangfire(IApplicationBuilder app)
        {
        }
    }
}