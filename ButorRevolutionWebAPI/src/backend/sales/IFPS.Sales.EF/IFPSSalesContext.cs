using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Seed;
using IFPS.Sales.Domain.Seed.CompanyTypes;
using IFPS.Sales.Domain.Seed.Countries;
using IFPS.Sales.Domain.Seed.Documents;
using IFPS.Sales.Domain.Seed.EmailDatas;
using IFPS.Sales.Domain.Seed.Employees;
using IFPS.Sales.Domain.Seed.EventTypes;
using IFPS.Sales.Domain.Seed.GroupingCategories;
using IFPS.Sales.Domain.Seed.Venues;
using IFPS.Sales.Domain.Services.Interfaces;
using IFPS.Sales.EF.EntityConfigurations;
using IFPS.Sales.EF.EntityConfigurations.OrderAggregate;
using IFPS.Sales.EF.EntityConfigurations.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Sales.EF
{
    public class IFPSSalesContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, DefaultRoleClaim, UserToken>
    {
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly ApplicationSettings settings;
        private readonly IConfiguration appConfig;

        #region TableAggregates

        #region BasketAggregate
        public virtual DbSet<Basket> Baskets { get; set; }

        #endregion

        #region LanguageAggregate
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageTranslation> LanguageTranslations { get; set; }
        #endregion

        #region CountryAggregate
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryTranslation> CountryTranslations { get; set; }
        #endregion

        #region Images
        public virtual DbSet<Image> Images { get; set; }
        #endregion

        #region CompanyTypeAggregate
        public virtual DbSet<CompanyType> CompanyTypes { get; set; }
        public virtual DbSet<CompanyTypeTranslation> CompanyTypeTranslations { get; set; }
        #endregion

        #region CompanyAggregate
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyData> CompanyData { get; set; }
        public virtual DbSet<CompanyDateRange> CompanyDateRanges { get; set; }
        #endregion

        #region CurrencyAggregate
        public virtual DbSet<Currency> Currencies { get; set; }
        #endregion

        #region DocumentStateAggregate
        public virtual DbSet<DocumentState> DocumentStates { get; set; }
        public virtual DbSet<DocumentStateTranslation> DocumentStateTranslations { get; set; }
        #endregion

        #region DocumentFolderAggregate
        public virtual DbSet<DocumentFolder> DocumentFolders { get; set; }
        public virtual DbSet<DocumentFolderTranslation> DocumentFolderTranslations { get; set; }
        #endregion

        #region DocumentAggregate
        public virtual DbSet<Document> Documents { get; set; }
        #endregion

        #region DocumentTypeAggregate
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<DocumentTypeTranslation> DocumentTypeTranslations { get; set; }
        #endregion

        #region DayTypeAggregate
        public virtual DbSet<DayType> DayTypes { get; set; }
        public virtual DbSet<DayTypeTranslation> DayTypeTranslations { get; set; }
        #endregion

        #region EmailDataAggregate
        public virtual DbSet<EmailData> EmailDatas { get; set; }
        public virtual DbSet<EmailDataTranslation> EmailDataTranslations { get; set; }
        #endregion

        #region OrderStateAggregate
        public virtual DbSet<OrderState> OrderStates { get; set; }
        public virtual DbSet<OrderStateTranslation> OrderStateTranslations { get; set; }
        #endregion

        #region EventTypeAggregate
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<EventTypeTranslation> EventTypeTranslations { get; set; }
        #endregion

        #region ExchangeRateAggregate
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        #endregion

        #region DivisionAggregate
        public virtual DbSet<Division> Divisons { get; set; }
        public virtual DbSet<DivisionTranslation> DivisonTranslations { get; set; }
        #endregion

        #region ClaimAggregate
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<DefaultRoleClaim> DefaultRoleClaims { get; set; }
        #endregion

        #region UserAggregate
        public virtual DbSet<UserData> UserData { get; set; }
        #endregion

        #region UserTeamAggregate
        public virtual DbSet<UserTeam> UserTeams { get; set; }
        public virtual DbSet<UserTeam> UserTeamMembers { get; set; }
        #endregion

        #region UserTeamTypeAggregate
        public virtual DbSet<UserTeamType> UserTeamTypes { get; set; }
        public virtual DbSet<UserTeamTypeTranslation> UserTeamTypeTranslations { get; set; }
        #endregion

        #region EmailAggregate
        public virtual DbSet<Email> Emails { get; set; }
        #endregion   

        #region EmployeeAggregate
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAbsenceDay> EmployeeAbsenceDays { get; set; }
        #endregion

        #region CustomerAggregate
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerFurnitureUnit> CustomerFurnitureUnits { get; set; }
        public virtual DbSet<CustomerNotificationMode> CustomerNotificationModes { get; set; }
        #endregion

        #region SalesPersonAggregate
        public virtual DbSet<SalesPerson> SalesPersons { get; set; }
        public virtual DbSet<SalesPersonDateRange> SalesPersonDateRanges { get; set; }
        public virtual DbSet<SalesPersonOffice> SalesPersonOffices { get; set; }
        #endregion

        #region ServiceAggregate

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<ServicePrice> ServicePrices { get; set; }

        #endregion

        #region ServiceTypeAggregate

        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<ServiceTypeTranslation> ServiceTypeTranslations { get; set; }

        #endregion

        #region AppointmentAggregate
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AnonymousUserData> AnonymousUserData { get; set; }
        #endregion

        #region GroupingCategoryAggregate
        public virtual DbSet<GroupingCategory> GroupingCategories { get; set; }
        public virtual DbSet<GroupingCategoryTranslation> GroupingCategoryTranslations { get; set; }
        #endregion

        #region CabinetMaterialAggregate
        public virtual DbSet<CabinetMaterial> CabinetMaterials { get; set; }
        #endregion

        #region MaterialAggregate
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<AccessoryMaterialFurnitureUnit> AccessoryMaterialFurnitureUnits { get; set; }
        public virtual DbSet<MaterialTranslation> MaterialTranslations { get; set; }
        public virtual DbSet<MaterialPrice> MaterialPrices { get; set; }
        #endregion

        #region FurnitureComponentAggregate
        public virtual DbSet<FurnitureComponent> FurnitureComponents { get; set; }
        #endregion

        #region FurnitureUnitAggregate
        public virtual DbSet<FurnitureUnit> FurnitureUnits { get; set; }
        public virtual DbSet<FurnitureUnitTranslation> FurnitureUnitTranslations { get; set; }
        public virtual DbSet<FurnitureUnitPrice> FurnitureUnitPrices { get; set; }
        #endregion

        #region FurnitureUnitTypeAggregate
        public virtual DbSet<FurnitureUnitType> FurnitureUnitTypes { get; set; }
        public virtual DbSet<FurnitureUnitTypeTranslation> FurnitureUnitTypeTranslations { get; set; }
        #endregion

        #region OrderAggregate
        public virtual DbSet<DocumentGroup> DocumentGroups { get; set; }
        public virtual DbSet<DocumentGroupVersion> DocumentGroupVersions { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OfferInformation> OfferInformation { get; set; }
        #endregion

        #region OrderedApplianceMaterialAggregate

        public virtual DbSet<OrderedApplianceMaterial> OrderedApplianceMaterials { get; set; }

        #endregion

        #region OrderedFurnitureUnitConfiguration

        public virtual DbSet<OrderedFurnitureUnit> OrderedFurnitureUnits { get; set; }

        #endregion

        #region OrderedServiceConfiguration

        public virtual DbSet<OrderedService> OrderedServices { get; set; }

        #endregion

        #region MessageChannelAggregate
        public virtual DbSet<MessageChannel> MessageChannels { get; set; }
        public virtual DbSet<MessageChannelParticipant> MessageChannelParticipants { get; set; }
        #endregion

        #region MessageChannelAggregate
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<ParticipantMessage> ParticipantMessages { get; set; }
        #endregion

        #region VenueAggregate
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<MeetingRoom> MeetingRooms { get; set; }
        public virtual DbSet<VenueDateRange> VenueDateRanges { get; set; }
        #endregion

        #region WebshopFurnitureUnitAggregate
        public virtual DbSet<WebshopFurnitureUnit> WebshopFurnitureUnits { get; set; }
        public virtual DbSet<WebshopFurnitureUnitImage> WebshopFurnitureUnitImages { get; set; }
        #endregion

        #region WebshopFurnitureUnitAggregate
        public virtual DbSet<WebshopOrder> WebshopOrders { get; set; }
        #endregion

        #endregion

        static double price1, price2, price3, price4, price5, price6, price7, price8, installationBasicFee, assemblyPrice;

        public IFPSSalesContext(
            DbContextOptions<IFPSSalesContext> options,
            IMediator mediator,
            IIdentityService identityService,
            IOptions<ApplicationSettings> appSettingsOptions,
            IConfiguration appConfig) : base(options)
        {
            Ensure.NotNull(mediator);

            this.mediator = mediator;
            this.identityService = identityService;
            this.settings = appSettingsOptions.Value;
            this.appConfig = appConfig;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            EntityAudit();

            DispatchDomainEventsAsync<int>().ConfigureAwait(false).GetAwaiter().GetResult();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            EntityAudit();

            await DispatchDomainEventsAsync<int>();
            await DispatchDomainEventsAsync<Guid>();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void EntityAudit()
        {
            var now = Clock.Now;
            var currentUserId = identityService.GetCurrentUserId();

            var fullAuditedEntities = ChangeTracker.Entries<IFullAudited>()
               .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Added || e.State == EntityState.Modified)
               .ToList();

            foreach (var entry in fullAuditedEntities)
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeletionTime = now;
                    entry.Entity.DeleterUserId = currentUserId;
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreationTime = now;
                    entry.Entity.CreatorUserId = currentUserId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModificationTime = now;
                    entry.Entity.LastModifierUserId = currentUserId;
                }
            }

            var deletedEntries = ChangeTracker.Entries<ISoftDelete>()
                .Where(e => e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in deletedEntries)
            {
                entry.Entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

        private async Task DispatchDomainEventsAsync<TKey>()
        {
            var domainEntities = ChangeTracker
                .Entries<IEntity<TKey>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async domainEvent => await mediator.Publish(domainEvent as object));

            foreach (var task in tasks)
            {
                await task;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region BasketAggregate
            modelBuilder.ApplyConfiguration(new BasketConfiguration());
            #endregion

            #region CurrencyAggregate
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            #endregion

            #region CountryAggregate
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CountryTranslationConfiguration());
            #endregion

            #region CompanyTypeAggregate
            modelBuilder.ApplyConfiguration(new CompanyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTypeTranslationConfiguration());
            #endregion

            #region ImageAggregate
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            #endregion

            #region DocumentStateAggregate
            modelBuilder.ApplyConfiguration(new DocumentStateConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentStateTranslationConfiguration());
            #endregion

            #region DocumentFolderAggregate
            modelBuilder.ApplyConfiguration(new DocumentFolderConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentFolderTranslationConfiguration());
            #endregion

            #region DocumentAggregate
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            #endregion

            #region DocumentTypeAggregate
            modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeTranslationConfiguration());
            #endregion

            #region DayTypeAggregate
            modelBuilder.ApplyConfiguration(new DayTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DayTypeTranslationConfiguration());
            #endregion

            #region EmailDataAggregate
            modelBuilder.ApplyConfiguration(new EmailDataConfiguration());
            modelBuilder.ApplyConfiguration(new EmailDataTranslationConfiguration());
            #endregion

            #region OrderStateAggregate
            modelBuilder.ApplyConfiguration(new OrderStateConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStateTranslationConfiguration());
            #endregion

            #region EventTypeAggregate            
            modelBuilder.ApplyConfiguration(new EventTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EventTypeTranslationConfiguration());
            #endregion

            #region LanguageAggregate
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageTranslationConfiguration());
            #endregion

            #region CompanyAggregate
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyDataConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyDateRangeConfiguration());
            #endregion

            #region VenueAggregate
            modelBuilder.ApplyConfiguration(new VenueConfiguration());
            modelBuilder.ApplyConfiguration(new VenueDateRangeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingRoomConfiguration());
            #endregion

            #region DivisionAggregate
            modelBuilder.ApplyConfiguration(new DivisionConfiguration());
            modelBuilder.ApplyConfiguration(new DivisionTranslationConfiguration());
            #endregion      

            #region RoleAggregate
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());
            modelBuilder.ApplyConfiguration(new DefaultRoleClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            #endregion

            #region UserAggregate
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserDataConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
            #endregion

            #region UserTeamAggregate
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
            #endregion

            #region UserTeamTypeAggregate
            modelBuilder.ApplyConfiguration(new UserTeamTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamTypeTranslationConfiguration());
            #endregion

            #region EmployeeAggregate
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeAbsenceDayConfiguration());
            #endregion

            #region CustomerAggregate
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerFurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerNotificationModeConfiguration());
            #endregion

            #region SalesPersonAggregate
            modelBuilder.ApplyConfiguration(new SalesPersonConfiguration());
            modelBuilder.ApplyConfiguration(new SalesPersonDateRangeConfiguration());
            modelBuilder.ApplyConfiguration(new SalesPersonOfficeConfiguration());
            #endregion

            #region ServiceAggregate

            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServicePriceConfiguration());

            #endregion

            #region ServiceType

            modelBuilder.ApplyConfiguration(new ServiceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeTranslationConfiguration());

            #endregion

            #region AppointmentAggregate
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new AnonymousUserDataConfiguration());
            #endregion

            #region GroupingCategoryAggregate
            modelBuilder.ApplyConfiguration(new GroupingCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new GroupingCategoryTranslationConfiguration());
            #endregion

            #region ExchangeRateAggregate
            modelBuilder.ApplyConfiguration(new ExchangeRateConfiguration());
            #endregion

            #region MaterialAggregate
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialPriceConfiguration());

            modelBuilder.ApplyConfiguration(new AccessoryMaterialFurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new AccessoryMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ApplianceMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new CabinetMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new FoilMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new BoardMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new DecorBoardMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new WorktopBoardMaterialConfiguration());

            #endregion

            #region FurnitureUnitAggregate
            modelBuilder.ApplyConfiguration(new FurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new FurnitureUnitTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new FurnitureUnitPriceConfiguration());
            #endregion

            #region FurnitureUnitTypeAggregate
            modelBuilder.ApplyConfiguration(new FurnitureUnitTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FurnitureUnitTypeTranslationConfiguration());
            #endregion

            #region FurnitureComponentAggregate
            modelBuilder.ApplyConfiguration(new FurnitureComponentConfiguration());
            #endregion

            #region OrderAggregate
            modelBuilder.ApplyConfiguration(new DocumentGroupConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentGroupVersionConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderedFurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new OrderedApplianceMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new OrderedServiceConfiguration());
            modelBuilder.ApplyConfiguration(new OrderPriceConfiguration());
            modelBuilder.ApplyConfiguration(new OfferInformationConfiguration());

            #endregion

            #region MessageChannelAggregate
            modelBuilder.ApplyConfiguration(new MessageChannelConfiguration());
            modelBuilder.ApplyConfiguration(new MessageChannelParticipantConfiguration());
            #endregion

            #region MessageAggregate
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantMessageConfiguration());
            #endregion

            #region WebshopFurnitureUnitAggregate
            modelBuilder.ApplyConfiguration(new WebshopFurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new WebshopFurnitureUnitImageConfiguration());
            #endregion

            #region WebshopOrders
            modelBuilder.ApplyConfiguration(new WebshopOrderConfiguration());
            modelBuilder.ApplyConfiguration(new WebshopOrderServiceConfiguration());
            #endregion

            price1 = double.Parse(settings.ServicePrice1);
            price2 = double.Parse(settings.ServicePrice2);
            price3 = double.Parse(settings.ServicePrice3);
            price4 = double.Parse(settings.ServicePrice4);
            price5 = double.Parse(settings.ServicePrice5);
            price6 = double.Parse(settings.ServicePrice6);
            price7 = double.Parse(settings.ServicePrice7);
            price8 = double.Parse(settings.ServicePrice8);
            assemblyPrice = double.Parse(settings.AssemblyPrice);
            installationBasicFee = double.Parse(settings.InstallationBasicFee);

            SeedInitialData(modelBuilder);
#if DEBUG
            SeedDebugData(modelBuilder);
#endif
        }

        #region SeedInitialData
        protected virtual void SeedInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(new CountrySeed().Entities);
            modelBuilder.Entity<CountryTranslation>().HasData(new CountryTranslationSeed().Entities);
            modelBuilder.Entity<EmailData>().HasData(new EmailDataSeed().Entities);
            modelBuilder.Entity<EmailDataTranslation>().HasData(new EmailDataTranslationSeed().Entities);
            modelBuilder.Entity<GroupingCategory>().HasData(new GroupingCategorySeed().Entities);
            modelBuilder.Entity<GroupingCategoryTranslation>().HasData(new GroupingCategoryTranslationSeed().Entities);
            modelBuilder.Entity<Claim>().HasData(new ClaimSeed().Entities);
            modelBuilder.Entity<CompanyType>().HasData(new CompanyTypeSeed().Entities);
            modelBuilder.Entity<CompanyTypeTranslation>().HasData(new CompanyTypeTranslationSeed().Entities);
            modelBuilder.Entity<Language>().HasData(new LanguageSeed().Entities);
            modelBuilder.Entity<LanguageTranslation>().HasData(new LanguageTranslationSeed().Entities);
            modelBuilder.Entity<Currency>().HasData(new CurrencySeed().Entities);
            modelBuilder.Entity<DayType>().HasData(new DayTypeSeed().Entities);
            modelBuilder.Entity<DayTypeTranslation>().HasData(new DayTypeTranslationSeed().Entities);
            modelBuilder.Entity<DocumentState>().HasData(new DocumentStateSeed().Entities);
            modelBuilder.Entity<DocumentStateTranslation>().HasData(new DocumentStateTranslationSeed().Entities);
            modelBuilder.Entity<DocumentFolder>().HasData(new DocumentFolderSeed().Entities);
            modelBuilder.Entity<DocumentFolderTranslation>().HasData(new DocumentFolderTranslationSeed().Entities);
            modelBuilder.Entity<DocumentType>().HasData(new DocumentTypeSeed().Entities);
            modelBuilder.Entity<DocumentTypeTranslation>().HasData(new DocumentTypeTranslationSeed().Entities);
            modelBuilder.Entity<FurnitureUnitType>().HasData(new FurnitureUnitTypeSeed().Entities);
            modelBuilder.Entity<FurnitureUnitTypeTranslation>().HasData(new FurnitureUnitTypeTranslationSeed().Entities);
            modelBuilder.Entity<OrderState>().HasData(new OrderStateSeed().Entities);
            modelBuilder.Entity<OrderStateTranslation>().HasData(new OrderStateTranslationSeed().Entities);
            modelBuilder.Entity<EventType>().HasData(new EventTypeSeed().Entities);
            modelBuilder.Entity<EventTypeTranslation>().HasData(new EventTypeTranslationSeed().Entities);
            modelBuilder.Entity<Division>().HasData(new DivisionSeed().Entities);
            modelBuilder.Entity<DivisionTranslation>().HasData(new DivisionTranslationSeed().Entities);
            modelBuilder.Entity<Role>().HasData(new RoleSeed().Entities);
            modelBuilder.Entity<DefaultRoleClaim>().HasData(new DefaultRoleClaimsSeed().Entities);
            modelBuilder.Entity<Image>().HasData(new ImageSeed().Entities);
            modelBuilder.Entity<Service>().HasData(new ServiceSeed().Entities);
            modelBuilder.Entity<ServiceType>().HasData(new ServiceTypeSeed().Entities);
            modelBuilder.Entity<ServiceTypeTranslation>().HasData(new ServiceTypeTranslationSeed().Entities);
            modelBuilder.Entity<ServicePrice>().HasData(new ServicePriceSeed().Entities);
            modelBuilder.Entity<ServicePrice>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    ServicePriceId = 1,
                    Value = price1,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 2,
                    Value = price2,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 3,
                    Value = price3,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 4,
                    Value = price4,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 5,
                    Value = price5,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 6,
                    Value = price6,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 7,
                    Value = price7,
                    CurrencyId = 1
                },
                new
                {
                    ServicePriceId = 8,
                    Value = price8,
                    CurrencyId = 1
                }
            ));
            modelBuilder.Entity<UserTeamType>().HasData(new UserTeamTypeSeed().Entities);
            modelBuilder.Entity<UserTeamTypeTranslation>().HasData(new UserTeamTypeTransationSeed().Entities);

            modelBuilder.Entity<Company>().HasData(new CompanySeed().ReleaseEntities);
            modelBuilder.Entity<CompanyData>().HasData(new CompanyDataSeed().ReleaseEntities);
            modelBuilder.Entity<CompanyData>(ent => ent.OwnsOne(e => e.HeadOffice).HasData(
                new
                {
                    CompanyDataId = 1,
                    AddressValue = "Petőfi utca 52.",
                    City = "Hajós",
                    PostCode = 6344,
                    CountryId = 1,
                }
            ));

            modelBuilder.Entity<User>().HasData(new UserSeed().ReleaseEntities);
            modelBuilder.Entity<UserClaim>().HasData(new UserClaimSeed().ReleaseEntities);
            modelBuilder.Entity<UserRole>().HasData(new UserRoleSeed().ReleaseEntities);
            modelBuilder.Entity<UserData>().HasData(new UserDataSeed().ReleaseEntities);
            modelBuilder.Entity<UserData>(ent => ent.OwnsOne(e => e.ContactAddress).HasData(
                new
                {
                    UserDataId = 1,
                    AddressValue = "Kossuth tér 1.",
                    City = "Szolnok",
                    PostCode = 5000,
                    CountryId = 1,
                }
            ));
        }
        #endregion

        #region SeedDebugData
        private static void SeedDebugData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>().HasData(new UserTeamSeed().Entities);
            modelBuilder.Entity<User>().HasData(new UserSeed().Entities);
            modelBuilder.Entity<UserClaim>().HasData(new UserClaimSeed().Entities);
            modelBuilder.Entity<UserRole>().HasData(new UserRoleSeed().Entities);
            modelBuilder.Entity<UserData>().HasData(new UserDataSeed().Entities);
            modelBuilder.Entity<UserData>(ent => ent.OwnsOne(e => e.ContactAddress).HasData(
                new
                {
                    UserDataId = 2,
                    AddressValue = "Irinyi József utca 42.",
                    City = "Budapest",
                    PostCode = 1117,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 3,
                    AddressValue = "Irinyi József utca 42.",
                    City = "Budapest",
                    PostCode = 1117,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 4,
                    AddressValue = "Andrássy út 92.",
                    City = "Budapest",
                    PostCode = 1062,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 5,
                    AddressValue = "Dohány utca 1.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 6,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 7,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                },
                new
                {
                    UserDataId = 8,
                    AddressValue = "Dohány utca 87.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 9,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 10,
                    AddressValue = "Dohány utca 44.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 11,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 12,
                    AddressValue = "Damjanich utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 13,
                    AddressValue = "Nefelejcs utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 14,
                    AddressValue = "Szív utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 15,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 16,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }, new
                {
                    UserDataId = 17,
                    AddressValue = "Dohány utca 88.",
                    City = "Budapest",
                    PostCode = 1074,
                    CountryId = 1,
                }
                ));

            modelBuilder.Entity<Company>().HasData(new CompanySeed().Entities);
            modelBuilder.Entity<CompanyData>().HasData(new CompanyDataSeed().Entities);
            modelBuilder.Entity<CompanyData>(ent => ent.OwnsOne(e => e.HeadOffice).HasData(
            new
            {
                CompanyDataId = 2,
                AddressValue = "Bocskai út 77-79.",
                City = "Budapest",
                PostCode = 1117,
                CountryId = 1,
            },
            new
            {
                CompanyDataId = 3,
                AddressValue = "Hegyalja út 26",
                City = "Budapest",
                PostCode = 1016,
                CountryId = 1
            },
            new
            {
                CompanyDataId = 4,
                AddressValue = "József Körút 28.",
                City = "Budapest",
                PostCode = 1085,
                CountryId = 1
            },
            new
            {
                CompanyDataId = 5,
                AddressValue = "Váci út 80",
                City = "Budapest",
                PostCode = 1133,
                CountryId = 1
            },
            new
            {
                CompanyDataId = 6,
                AddressValue = "Váci út 80",
                City = "Budapest",
                PostCode = 1133,
                CountryId = 1
            }
            ));


            modelBuilder.Entity<Venue>().HasData(new VenueSeed().Entities);
            modelBuilder.Entity<Venue>(ent => ent.OwnsOne(e => e.OfficeAddress).HasData(
                new
                {
                    VenueId = 1,
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    VenueId = 2,
                    AddressValue = "H-6724 Szeged, Kossuth Lajos sugárút 72/B",
                    City = "Szeged",
                    PostCode = 6724,
                    CountryId = 1
                }
                ));

            modelBuilder.Entity<MeetingRoom>().HasData(new MeetingRoomSeed().Entities);

            modelBuilder.Entity<Appointment>().HasData(new AppointmentSeed().Entities);
            modelBuilder.Entity<Appointment>(ent => ent.OwnsOne(e => e.Address).HasData(
                new
                {
                    AppointmentId = 1,
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    AppointmentId = 2,
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    AppointmentId = 3,
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    AppointmentId = 4,
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                }));

            modelBuilder.Entity<Appointment>(ent => ent.OwnsOne(e => e.ScheduledDateTime).HasData(
                new
                {
                    AppointmentId = 1,
                    From = DateTime.Now,
                    To = DateTime.Now.AddMinutes(30)
                },
                new
                {
                    AppointmentId = 2,
                    From = DateTime.Now.AddHours(1),
                    To = DateTime.Now.AddHours(2)
                },
                new
                {
                    AppointmentId = 3,
                    From = DateTime.Now.AddHours(3),
                    To = DateTime.Now.AddHours(4)
                },
                new
                {
                    AppointmentId = 4,
                    From = DateTime.Now.AddDays(1),
                    To = DateTime.Now.AddDays(1).AddMinutes(45)
                }));


            modelBuilder.Entity<AccessoryMaterial>().HasData(new AccessoryMaterialSeed().Entities);

            modelBuilder.Entity<DecorBoardMaterial>().HasData(new DecorBoardMaterialSeed().Entities);
            modelBuilder.Entity<DecorBoardMaterial>(ent => ent.OwnsOne(e => e.Dimension).HasData(
                new
                {
                    BoardMaterialId = new Guid("7793fb49-f76c-49c4-ad17-85d8e7c92937"),
                    Width = 2700.0,
                    Length = 2000.0,
                    Thickness = 4.0
                },
                new
                {
                    BoardMaterialId = new Guid("3b2edf78-39ef-440f-a780-9714685e51af"),
                    Width = 2500.0,
                    Length = 1800.0,
                    Thickness = 4.0
                },
                new
                {
                    BoardMaterialId = new Guid("b4346264-619f-4193-b2d0-7fab00b16b13"),
                    Width = 2400.0,
                    Length = 1200.0,
                    Thickness = 5.0
                },
                new
                {
                    BoardMaterialId = new Guid("b7ee9180-76a7-4854-9108-99e7d7295404"),
                    Width = 2700.0,
                    Length = 2000.0,
                    Thickness = 3.0
                }
            ));

            modelBuilder.Entity<FoilMaterial>().HasData(new FoilMaterialSeed().Entities);

            modelBuilder.Entity<ApplianceMaterial>().HasData(new ApplianceMaterialSeed().Entities);
            modelBuilder.Entity<ApplianceMaterial>(ent => ent.OwnsOne(e => e.SellPrice).HasData(
                new
                {
                    ApplianceMaterialId = new Guid("d7b28f76-2766-425f-a9b2-b7646ae8c77f"),
                    Value = 10000.0,
                    CurrencyId = 1
                },
                new
                {
                    ApplianceMaterialId = new Guid("f3ac5748-9bb5-47f6-8b64-8fa891024fe5"),
                    Value = 120000.0,
                    CurrencyId = 1
                },
                new
                {
                    ApplianceMaterialId = new Guid("2903aac4-3564-4117-8a21-c7358814c306"),
                    Value = 114900.0,
                    CurrencyId = 1
                }
            ));

            modelBuilder.Entity<MaterialPrice>().HasData(new MaterialPriceSeed().Entities);
            modelBuilder.Entity<MaterialPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
               new
               {
                   MaterialPriceId = 1,
                   Value = 6000.0,
                   CurrencyId = 1,
               },
               new
               {
                   MaterialPriceId = 2,
                   Value = 3000.0,
                   CurrencyId = 1,
               },
               new
               {
                   MaterialPriceId = 3,
                   Value = 74187.0,
                   CurrencyId = 1,
               }
            ));
            modelBuilder.Entity<FurnitureUnit>().HasData(new FurnitureUnitSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>().HasData(new FurnitureUnitPriceSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    FurnitureUnitPriceId = 1,
                    Value = 20000.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 2,
                    Value = 100000.0,
                    CurrencyId = 1
                }
            ));
            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.MaterialCost).HasData(
                new
                {
                    FurnitureUnitPriceId = 1,
                    Value = 9000.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 2,
                    Value = 15000.0,
                    CurrencyId = 1
                }
            ));

            modelBuilder.Entity<FurnitureComponent>().HasData(new FurnitureComponentSeed().Entities);
            modelBuilder.Entity<AccessoryMaterialFurnitureUnit>().HasData(new AccessoryMaterialFurnitureUnitSeed().Entities);

            modelBuilder.Entity<Customer>().HasData(new CustomerSeed().Entities);
            modelBuilder.Entity<CustomerFurnitureUnit>().HasData(new CustomerFurnitureUnitSeed().Entities);

            modelBuilder.Entity<SalesPerson>().HasData(new SalesPersonSeed().Entities);
            modelBuilder.Entity<Employee>().HasData(new EmployeeSeed().Entities);
            modelBuilder.Entity<Ticket>().HasData(new TicketSeed().Entities);
            modelBuilder.Entity<Order>().HasData(new OrderSeed().Entities);
            modelBuilder.Entity<Order>(ent => ent.OwnsOne(e => e.ShippingAddress).HasData(
                new
                {
                    OrderId = new Guid("5C75E657-4BB7-4791-A829-5E85C2EA7D12"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("2418B030-A64B-4724-9702-964CF5EB04C6"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("DD504088-3F36-4AD9-9CC1-84545AFA497D"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("C7237D77-ADCF-45CB-8835-4DB99DF213FA"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("791FF638-7B97-4118-AE40-677EDAD1A64D"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("509B7D7A-46B5-42A4-A944-4C46BC8B91FB"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("55E7F6D7-9444-400A-BFB4-987DC59C8A55"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("4C9602FD-6139-4D35-9D7C-5468AC4F2F64"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("3E2D98BE-1C68-4DB6-B180-1C6D6CD03ACC"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("3C9361FB-23C2-4952-8A27-F19CE4D2603D"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("73D46114-FE92-4AB8-B8A6-719B8206A6BC"),
                    AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                    City = "Budapest",
                    PostCode = 1113,
                    CountryId = 1
                }
            ));
            modelBuilder.Entity<Order>(ent => ent.OwnsOne(e => e.Budget).HasData(
                new
                {
                    OrderId = new Guid("5C75E657-4BB7-4791-A829-5E85C2EA7D12"),
                    Value = 0.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("2418B030-A64B-4724-9702-964CF5EB04C6"),
                    Value = 0.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("DD504088-3F36-4AD9-9CC1-84545AFA497D"),
                    Value = 0.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("C7237D77-ADCF-45CB-8835-4DB99DF213FA"),
                    Value = 100000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("791FF638-7B97-4118-AE40-677EDAD1A64D"),
                    Value = 200000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("509B7D7A-46B5-42A4-A944-4C46BC8B91FB"),
                    Value = 300000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("55E7F6D7-9444-400A-BFB4-987DC59C8A55"),
                    Value = 400000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("4C9602FD-6139-4D35-9D7C-5468AC4F2F64"),
                    Value = 500000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("3E2D98BE-1C68-4DB6-B180-1C6D6CD03ACC"),
                    Value = 600000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("3C9361FB-23C2-4952-8A27-F19CE4D2603D"),
                    Value = 700000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderId = new Guid("73D46114-FE92-4AB8-B8A6-719B8206A6BC"),
                    Value = 1000000.0,
                    CurrencyId = 1
                }
            ));

            modelBuilder.Entity<OfferInformation>().HasData(new OfferInformationSeed().Entities);
            modelBuilder.Entity<OfferInformation>(ent => ent.OwnsOne(e => e.ProductsPrice).HasData(
                new
                {
                    OfferInformationId = 1,
                    Value = 10000.0,
                    CurrencyId = 1
                },
                new
                {
                    OfferInformationId = 2,
                    Value = 20000.0,
                    CurrencyId = 1
                }
                ));

            modelBuilder.Entity<OfferInformation>(ent => ent.OwnsOne(e => e.ServicesPrice).HasData(
                new
                {
                    OfferInformationId = 1,
                    Value = 10000.0,
                    CurrencyId = 1
                },
                new
                {
                    OfferInformationId = 2,
                    Value = 20000.0,
                    CurrencyId = 1
                }
                ));

            modelBuilder.Entity<DocumentGroup>().HasData(new DocumentGroupSeed().Entities);
            modelBuilder.Entity<DocumentGroupVersion>().HasData(new DocumentGroupVersionSeed().Entities);
            modelBuilder.Entity<Document>().HasData(new DocumentSeed().Entities);


            modelBuilder.Entity<OrderPrice>().HasData(new OrderPriceSeed().Entities);
            modelBuilder.Entity<OrderPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    OrderPriceId = 1,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 2,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 3,
                    Value = 50000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 4,
                    Value = 50000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 5,
                    Value = 100000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 6,
                    Value = 100000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 7,
                    Value = 150000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 8,
                    Value = 150000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 9,
                    Value = 200000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 10,
                    Value = 200000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 11,
                    Value = 250000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 12,
                    Value = 250000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 13,
                    Value = 300000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 14,
                    Value = 300000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 15,
                    Value = 350000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 16,
                    Value = 350000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 17,
                    Value = 500000.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderPriceId = 18,
                    Value = 500000.0,
                    CurrencyId = 1
                }
            ));

            modelBuilder.Entity<WebshopFurnitureUnit>().HasData(new WebshopFurnitureUnitSeed().Entities);
            modelBuilder.Entity<WebshopFurnitureUnit>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    WebshopFurnitureUnitId = 1,
                    Value = 20000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 2,
                    Value = 54000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 3,
                    Value = 119000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 4,
                    Value = 70000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 5,
                    Value = 100000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 6,
                    Value = 250000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 7,
                    Value = 30000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 8,
                    Value = 27000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 9,
                    Value = 18000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 10,
                    Value = 22000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 11,
                    Value = 34000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 12,
                    Value = 10000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 13,
                    Value = 9000.0,
                    CurrencyId = 1
                }, new
                {
                    WebshopFurnitureUnitId = 14,
                    Value = 11000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 15,
                    Value = 28000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 16,
                    Value = 55000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 17,
                    Value = 7000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 18,
                    Value = 6000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 19,
                    Value = 19000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 20,
                    Value = 21000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 21,
                    Value = 31000.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 22,
                    Value = 24500.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 23,
                    Value = 24500.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 24,
                    Value = 24500.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 25,
                    Value = 24500.0,
                    CurrencyId = 1
                },
                new
                {
                    WebshopFurnitureUnitId = 26,
                    Value = 24500.0,
                    CurrencyId = 1
                }
            ));
            modelBuilder.Entity<WebshopFurnitureUnitImage>().HasData(new WebshopFurnitureUnitImageSeed().Entities);

            modelBuilder.Entity<WebshopOrder>().HasData(new WebshopOrderSeed().Entities);
            modelBuilder.Entity<WebshopOrder>(ent => ent.OwnsOne(e => e.ShippingAddress).HasData(
               new
               {
                   WebshopOrderId = new Guid("23cf9320-3062-4db4-aa29-a0594a726f3f"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("1f8087e2-8134-4801-af76-8276fbf246b7"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("989d1357-1aed-4ee9-9b40-65e494a23784"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("6eaa291b-f125-4f0a-a0b3-5c878d251ba3"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("98e32021-bcaa-497f-bbf9-e61853ecf295"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("20b33f75-09b7-4bdc-9470-e49db63dc3de"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("1055b58c-c922-4f13-a35c-af0e56083a51"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("fd53912a-9a3c-4586-86b1-d704b5fbb180"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("52f9338b-263c-4054-81dd-70d6345a171e"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               },
               new
               {
                   WebshopOrderId = new Guid("c654a3f5-3949-4df2-bf6d-ee4a62622368"),
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               }
            ));

            modelBuilder.Entity<OrderedFurnitureUnit>().HasData(new OrderedFurnitureUnitSeed().Entities);
            modelBuilder.Entity<OrderedFurnitureUnit>(ent => ent.OwnsOne(e => e.UnitPrice).HasData(
                new
                {
                    OrderedFurnitureUnitId = 1,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 2,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 3,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 4,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 5,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 6,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 7,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 8,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 9,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 10,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 11,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 12,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 13,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 14,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 15,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 16,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 17,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 18,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 19,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 20,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 21,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 22,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 23,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 24,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 25,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 26,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 27,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 28,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 29,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 30,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 31,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 32,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    OrderedFurnitureUnitId = 33,
                    Value = 20.0,
                    CurrencyId = 1
                }
            ));
            modelBuilder.Entity<Basket>().HasData(new BasketSeed().Entities);
            modelBuilder.Entity<Basket>(ent => ent.OwnsOne(e => e.BillingAddress).HasData(
               new
               {
                   BasketId = 1,
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               }
            ));
            modelBuilder.Entity<Basket>(ent => ent.OwnsOne(e => e.DelieveryAddress).HasData(
               new
               {
                   BasketId = 1,
                   AddressValue = "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em",
                   City = "Budapest",
                   PostCode = 1113,
                   CountryId = 1
               }
            ));
            modelBuilder.Entity<Basket>(ent => ent.OwnsOne(e => e.SubTotal).HasData(
               new
               {
                   BasketId = 1,
                   Value = 120000.0,
                   CurrencyId = 1
               }
            ));
            modelBuilder.Entity<Basket>(ent => ent.OwnsOne(e => e.DelieveryPrice).HasData(
               new
               {
                   BasketId = 1,
                   Value = 5000.0,
                   CurrencyId = 1
               }
            ));

            //modelBuilder.Entity<MessageChannel>().HasData(new MessageChannelSeed().Entities);
            //modelBuilder.Entity<MessageChannelParticipant>().HasData(new MessageChannelParticipantSeed().Entities);
            //modelBuilder.Entity<Message>().HasData(new MessageSeed().Entities);
            //modelBuilder.Entity<ParticipantMessage>().HasData(new ParticipantMessageSeed().Entities);
        }
        #endregion
    }
}
