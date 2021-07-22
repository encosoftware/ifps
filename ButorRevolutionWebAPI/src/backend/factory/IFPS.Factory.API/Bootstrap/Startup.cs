using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using IFPS.Factory.API.Bootstrap.AutofacModules;
using IFPS.Factory.API.Controllers;
using IFPS.Factory.API.Infrastructure.Filters;
using IFPS.Factory.API.Infrastructure.Middlewares;
using IFPS.Factory.API.Infrastructure.Providers;
using IFPS.Factory.API.Infrastructure.Services.Implementations;
using IFPS.Factory.API.Infrastructure.Services.Interfaces;
using IFPS.Factory.Application.BackgroundJobs;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.EF;
using IFPS.Factory.EF.FileHandling;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace IFPS.Factory.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _isDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }
        protected readonly bool _isDevelopment = false;

        public virtual void ConfigureDbContext(IServiceCollection services, bool isDevelopment = false)
        {
            services.AddDbContext<IFPSFactoryContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });
        }

        public virtual void ConfigureHangfireDbContext(IServiceCollection services, bool isDevelopment = false)
        {
            services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                             .UseSimpleAssemblyNameTypeSerializer()
                             .UseRecommendedSerializerSettings()
                             .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), options: new SqlServerStorageOptions
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
            var interval = int.Parse(Configuration.GetSection("EmailConfiguration").GetSection("HangfireTimeInterval").Value);
            RecurringJob.AddOrUpdate<IEmailSenderJob>(emailjob => emailjob.SendAllEmails(), $"0 0 */{interval} ? * *");
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddHttpClient<ImagesController>();
            services.AddHttpClient<DocumentController>();

            var signingSecret = Encoding.UTF8.GetBytes(Configuration["Jwt:SigningSecret"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ClaimPolicyEnum.GetRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetRoles.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateRoles.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteRoles.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCompanies.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateCompanies.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteCompanies.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetUsers.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateUsers.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteUsers.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetMaterialPackages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetMaterialPackages.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateMaterialPackages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateMaterialPackages.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteMaterialPackages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteMaterialPackages.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOrderSchedulings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrderSchedulings.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateOrderSchedulings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateOrderSchedulings.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOptimizations.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOptimizations.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateOptimizations.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateOptimizations.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetCuttings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCuttings.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetCncs.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCncs.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetEdgeBandings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetEdgeBandings.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetAssemblies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetAssemblies.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetSortings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetSortings.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetPackings.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetPackings.ToString()));

                options.AddPolicy(ClaimPolicyEnum.UpdateProductionProcess.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateProductionProcess.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetCameras.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCameras.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateCameras.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateCameras.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteCameras.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteCameras.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetWorkstations.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetWorkstations.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateWorkstations.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateWorkstations.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteWorkstations.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteWorkstations.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetMachines.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetMachines.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateMachines.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateMachines.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteMachines.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteMachines.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetWorkloads.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetWorkloads.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetCargos.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCargos.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateCargos.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateCargos.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteCargos.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteCargos.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetRequiredMaterials.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetRequiredMaterials.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateRequiredMaterials.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateRequiredMaterials.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetGeneralExpenses.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetGeneralExpenses.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateGeneralExpenses.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateGeneralExpenses.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteGeneralExpenses.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteGeneralExpenses.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOrderExpenses.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrderExpenses.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetFinanceStatistics.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetFinanceStatistics.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetInspections.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetInspections.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateInspections.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateInspections.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteInspections.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteInspections.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetStocks.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetStocks.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateStocks.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateStocks.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteStocks.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteStocks.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetStorageCells.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetStorageCells.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateStorageCells.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateStorageCells.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteStorageCells.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteStorageCells.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetStorages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetStorages.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateStorages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateStorages.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteStorages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteStorages.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetStockStatistics.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetStockStatistics.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetFurnitureUnits.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetFurnitureUnits.ToString()));
            });
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailConfiguration"));
            services.Configure<LocalFileStorageConfiguration>(Configuration.GetSection("Site"));
            services.Configure<LocalConnectionConfiguration>(Configuration.GetSection("APIURLs"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddMediatR(
                Assembly.Load("IFPS.Factory.API"),
                Assembly.Load("IFPS.Factory.Application"),
                Assembly.Load("IFPS.Factory.Domain"));

            ConfigureDbContext(services, _isDevelopment);
            //ConfigureHangfireDbContext(services, _isDevelopment);
            services.AddDistributedMemoryCache();

            var allowedOrigins = Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>();
            services.AddCors(options =>
                options.AddPolicy("DefaultPolicy",
                builder =>
                {
                    builder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }
            ));

            services.AddIdentity<User, Role>(
               options =>
               {
                   options.Password.RequiredLength = 6;
                   options.Password.RequiredUniqueChars = 1;
                   options.Password.RequireDigit = true;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireLowercase = false;
                   options.User.AllowedUserNameCharacters = "AÁBCDEÉFGHIÍJKLMNOÓÖŐPQRSTUÚÜŰVWXYZaábcdeéfghiíjklmnoóöőpqrstuúüűvwxyz_0123456789._";
               })
               .AddEntityFrameworkStores<IFPSFactoryContext>()
               .AddDefaultTokenProviders();
            ConfigureAuth(services);

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
                opt.RegisterValidatorsFromAssembly(Assembly.Load("IFPS.Factory.Application"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "IFPS Factory API", Version = "v1" });
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                IList<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("hu-HU"),
                    new CultureInfo("en-GB")
                };

                options.DefaultRequestCulture = new RequestCulture("hu-HU");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider(),
                    new UserLanguagePreferenceProvider(),
                    new QueryStringRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            return SetupAutofac(services);
        }

        public virtual IServiceProvider SetupAutofac(IServiceCollection services)
        {
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new EncoDDDAutofacModule());
            container.RegisterModule(new AppAutofacModule());
            container.RegisterModule(new DomainAutofacModule());
            container.RegisterModule(new EFAutofacModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRequestLocalization();

            app.UseHealthChecks("/health");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint("/swagger/v1/swagger.json", "IFPS Factory API v1");
            });

            //   AddHangfire(app);

            app.UseCors("DefaultPolicy");
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}