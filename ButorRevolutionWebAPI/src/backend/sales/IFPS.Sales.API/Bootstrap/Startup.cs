using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using IFPS.Sales.API.Bootstrap.AutofacModules;
using IFPS.Sales.API.Controllers;
using IFPS.Sales.API.Infrastructure.Filters;
using IFPS.Sales.API.Infrastructure.Middlewares;
using IFPS.Sales.API.Infrastructure.Providers;
using IFPS.Sales.API.Infrastructure.Services.Implementations;
using IFPS.Sales.API.Infrastructure.Services.Interfaces;
using IFPS.Sales.API.Infrastructure.Swagger;
using IFPS.Sales.Domain;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Services.Implementations;
using IFPS.Sales.Domain.Services.Interfaces;
using IFPS.Sales.EF;
using IFPS.Sales.Application.BackgroundJobs;
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
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace IFPS.Sales.API
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
            services.AddDbContext<IFPSSalesContext>(options =>
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

                options.AddPolicy(ClaimPolicyEnum.GetMaterials.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetMaterials.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateMaterials.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateMaterials.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteMaterials.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteMaterials.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetRoles.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateRoles.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteRoles.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteRoles.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetVenues.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetVenues.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateVenues.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateVenues.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteVenues.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteVenues.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCompanies.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateCompanies.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteCompanies.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteCompanies.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetGroupingCategories.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetGroupingCategories.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateGroupingCategories.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateGroupingCategories.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteGroupingCategories.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteGroupingCategories.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetFurnitureUnits.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetFurnitureUnits.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateFurnitureUnits.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateFurnitureUnits.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteFurnitureUnits.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteFurnitureUnits.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetUsers.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateUsers.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteUsers.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteUsers.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetAppointments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetAppointments.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateAppointments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateAppointments.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteAppointments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteAppointments.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOrdersBySales.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrdersBySales.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetOrders.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrders.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateOrders.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateOrders.ToString()));
                options.AddPolicy(ClaimPolicyEnum.DeleteOrders.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.DeleteOrders.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetMessages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetMessages.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateMessages.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateMessages.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOrderDocuments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrderDocuments.ToString()));
                options.AddPolicy(ClaimPolicyEnum.UpdateOrderDocuments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateOrderDocuments.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetTickets.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetTickets.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetTrends.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetTrends.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetStatistics.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetStatistics.ToString()));

                options.AddPolicy(ClaimPolicyEnum.GetOrdersByCustomer.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrdersByCustomer.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetCustomerAppointments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetCustomerAppointments.ToString()));
                options.AddPolicy(ClaimPolicyEnum.ApproveOrderDocuments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.ApproveOrderDocuments.ToString()));

                options.AddPolicy(ClaimPolicyEnum.UpdateOrdersByPartner.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.UpdateOrdersByPartner.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetPartnerAppointments.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetPartnerAppointments.ToString()));
                options.AddPolicy(ClaimPolicyEnum.GetOrdersByPartner.ToString(), policy => policy.RequireClaim("IFPSClaim", ClaimPolicyEnum.GetOrdersByPartner.ToString()));
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
            services.Configure<OrderStateDeadlineConfiguration>(Configuration.GetSection("OrderStateConfiguration"));
            services.AddHttpClient<ImagesController>();
            services.AddHttpClient<DocumentController>();
            services.AddHttpContextAccessor();

            services.AddMediatR(
                Assembly.Load("IFPS.Sales.API"),
                Assembly.Load("IFPS.Sales.Application"),
                Assembly.Load("IFPS.Sales.Domain"));

            ConfigureDbContext(services, _isDevelopment);
            ConfigureHangfireDbContext(services, _isDevelopment);

            services.AddDistributedMemoryCache();

            var allowedOrigins = Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>();
            services.AddCors(options =>
                options.AddPolicy("AllowCredentials",
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
                   options.SignIn.RequireConfirmedEmail = true;
                   options.User.RequireUniqueEmail = true;
                   options.User.AllowedUserNameCharacters = "AÁBCDEÉFGHIÍJKLMNOÓÖÕPQRSTUÚÜÛVWXYZaábcdeéfghiíjklmnoóöõpqrstuúüûvwxyz_0123456789 ";
               })
               .AddEntityFrameworkStores<IFPSSalesContext>()
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
                opt.RegisterValidatorsFromAssembly(Assembly.Load("IFPS.Sales.Application"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "IFPS Sales API", Version = "v1" });
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
                options.OperationFilter<SwaggerFileUploadOperationFilter>();

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

            services.AddScoped<EmailSenderJob>();
            services.AddSingleton<IEmailService, EmailService>();

            services.AddSignalR();
            //services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();

            var autofacServiceProvider = SetupAutofac(services);
            return autofacServiceProvider;
        }

        public virtual IServiceProvider SetupAutofac(IServiceCollection services)
        {
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new EncoDDDAutofacModule());
            container.RegisterModule(new AppAutofacModule());
            container.RegisterModule(new DomainAutofacModule());
            container.RegisterModule(new EFAutofacModule());

            var buildContainer = container.Build();
            GlobalConfiguration.Configuration.UseAutofacActivator(buildContainer);

            return new AutofacServiceProvider(buildContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRequestLocalization();

            app.UseHealthChecks("/health");

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();

            }

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint("/swagger/v1/swagger.json", "IFPS API v1");
            });

            AddHangfire(app);

            app.UseCors("AllowCredentials");

            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<MessageHub>("/api/notify");
            });

            app.UseMvc();
        }
    }
}