using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using IFPS.Sales.API;
using IFPS.Sales.API.Bootstrap.AutofacModules;
using IFPS.Sales.EF;
using IFPS.Sales.FunctionalTests.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace IFPS.Sales.FunctionalTests
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
            container.RegisterModule(new EFAutofacModule());

            return new AutofacServiceProvider(container.Build());
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;
            var db = serviceProvider.GetRequiredService<IFPSSalesContext>();
            var logger = serviceProvider
                .GetRequiredService<ILogger<IFPSSalesWebApplicationFactory>>();

            db.Database.OpenConnection();
            db.Database.EnsureCreated();

            try
            {
                // Seed the database with test data.
                SeedDataBase(db);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred seeding the " +
                    "database with test messages. Error: {ex.Message}");
                throw ex;
            }

            base.Configure(app, env);

            var cultureInfo = new CultureInfo("hu-HU");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            
        }

        public virtual void SeedDataBase(IFPSSalesContext db)
        {
            TestDataSeed.PopulateTestData(db);
        }

        public override void ConfigureDbContext(IServiceCollection services, bool isDevelopment = false)
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            services.AddDbContext<IFPSSalesContext>(options =>
            {
                options.UseSqlite(connection);
                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                }
            });
        }

        public override void ConfigureHangfireDbContext(IServiceCollection services, bool isDevelopment = false)
        {
            
        }

        public override void AddHangfire(IApplicationBuilder app)
        {
        }
    }
}