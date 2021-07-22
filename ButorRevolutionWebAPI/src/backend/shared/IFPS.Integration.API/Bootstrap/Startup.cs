using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using IFPS.Factory.EF;
using IFPS.Integration.API.Bootstrap.AutofacModules;
using IFPS.Integration.API.Infrastructure.Filters;
using IFPS.Integration.API.Infrastructure.Middlewares;
using IFPS.Integration.Application.Interfaces;
using IFPS.Integration.Application.Services;
using IFPS.Sales.EF;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using IFPS.Integration.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using IFPS.Integration.API.BackgroundJob;
using Hangfire;
using Hangfire.SqlServer;
using System.Collections.Generic;

namespace IFPS.Integration.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<IFPSFactoryContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FactoryConnection"));
            });

            services.AddDbContext<IFPSSalesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SalesConnection"));
            });
        }

        public virtual void ConfigureHangfireDbContext(IServiceCollection services)
        {
            services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                             .UseSimpleAssemblyNameTypeSerializer()
                             .UseRecommendedSerializerSettings()
                             .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), options: new SqlServerStorageOptions
                             {
                                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                 QueuePollInterval = TimeSpan.FromSeconds(15),
                                 UseRecommendedIsolationLevel = true,
                                 UsePageLocksOnDequeue = true,
                                 DisableGlobalLocks = true
                             }));

            services.AddHangfireServer();
        }

        public virtual void AddHangfire(IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
            var interval = int.Parse(Configuration.GetSection("HangfireConfiguration").GetSection("TimeInterval").Value);
            RecurringJob.AddOrUpdate<ISynchronizationJob>(job => job.Syncronise(), $"0 */{interval} * ? * *");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.Configure<LocalConnectionConfiguration>(Configuration.GetSection("APIURLs"));
            services.Configure<Sales.Domain.Model.ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.AddHttpClient<ISynchronizationAppService, SynchronizationAppService>();
            services.AddMediatR(
                Assembly.Load("IFPS.Integration.API"),
                Assembly.Load("IFPS.Integration.Application"),
                Assembly.Load("IFPS.Sales.Domain"),
                Assembly.Load("IFPS.Sales.EF"),
                Assembly.Load("IFPS.Factory.Application"),
                Assembly.Load("IFPS.Factory.Domain"),
                Assembly.Load("IFPS.Factory.EF"));

            services.AddDistributedMemoryCache();

            var allowedOrigins = Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>();
            services.AddCors(options =>
                options.AddPolicy("AllowCredentials",
                builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }
            ));

            services.AddScoped<SynchronizationJob>();
            services.AddSingleton<ISynchronizationJob, SynchronizationJob>();

            ConfigureDbContext(services);
            ConfigureHangfireDbContext(services);

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidateDtoActionFilter));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddFluentValidation(opt =>
            {
                opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                opt.RegisterValidatorsFromAssembly(Assembly.Load("IFPS.Integration.Application"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "IFPS Integration API", Version = "v1" });
                options.EnableAnnotations();
                options.OrderActionsBy(a => a.RelativePath);
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                //options.AddFluentValidationRules();
            });

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new EncoDDDAutofacModule());
            container.RegisterModule(new AppAutofacModule());
            container.RegisterModule(new DomainAutofacModule());
            container.RegisterModule(new EFAutofacModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRequestLocalization();

            app.UseHealthChecks("/health");

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            AddHangfire(app);

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint("/swagger/v1/swagger.json", "IFPS API v1");
            });
            app.UseCors("AllowCredentials");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
