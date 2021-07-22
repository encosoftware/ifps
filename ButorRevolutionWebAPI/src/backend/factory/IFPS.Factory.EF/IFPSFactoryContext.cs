using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Seed;
using IFPS.Factory.EF.EntityConfigurations;
using IFPS.Factory.EF.EntityConfigurations.ExpenseAggregate;
using IFPS.Factory.EF.EntityConfigurations.OrderAggregate;
using IFPS.Factory.EF.EntityConfigurations.SiUnitAggregate;
using IFPS.Factory.EF.EntityTypeConfiguration.ExpenseAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using IFPS.Factory.Domain.Services.Interfaces;
using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Factory.EF
{
    public class IFPSFactoryContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, DefaultRoleClaim, UserToken>
    {
        protected IMediator mediator;
        protected IIdentityService identityService;

        public IFPSFactoryContext(DbContextOptions<IFPSFactoryContext> options) : base(options)
        {
        }

        public IFPSFactoryContext(DbContextOptions options) : base(options)
        {
        }

        public IFPSFactoryContext(DbContextOptions<IFPSFactoryContext> options,
            IMediator mediator,
            IIdentityService identityService) : base(options)
        {
            Ensure.NotNull(mediator);

            this.mediator = mediator;
            this.identityService = identityService;
        }

        #region CameraAggregate
        public virtual DbSet<Camera> Cameras { get; set; }
        #endregion

        #region CargoAggregate

        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<OrderedMaterialPackage> OrderedMaterialPackages { get; set; }

        #endregion CargoAggregate

        #region CargoStatusTypeAggregate

        public virtual DbSet<CargoStatusType> CargoStatusTypes { get; set; }
        public virtual DbSet<CargoStatusTypeTranslation> CargoStatusTypeTranslations { get; set; }

        #endregion CargoStatusTypeAggregate

        #region CFCProductionStateAggregate

        public virtual DbSet<CFCProductionState> CFCProductionStates { get; set; }
        public virtual DbSet<CFCProductionStateTranslation> CFCProductionStateTranslations { get; set; }

        #endregion

        #region CompanyAggregate

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyData> CompanyDatas { get; set; }
        public virtual DbSet<CompanyDateRange> CompanyDateRanges { get; set; }

        #endregion CompanyAggregate

        #region CompanyTypeAggregate

        public virtual DbSet<CompanyType> GetCompanyTypes { get; set; }
        public virtual DbSet<CompanyTypeTranslation> GetCompanyTypeTranslations { get; set; }

        #endregion CompanyTypeAggregate

        #region ConcreteFurnitureComponentAggregate

        public virtual DbSet<ConcreteFurnitureComponent> ConcreteFurnitureComponents { get; set; }

        #endregion ConcreteFurnitureComponentAggregate

        #region ConcreteFurnitureUnitAggregate

        public virtual DbSet<ConcreteFurnitureUnit> ConcreteFurnitureUnits { get; set; }

        #endregion ConcreteFurnitureUnitAggregate

        #region CountryAggregate

        public virtual DbSet<Country> Countries { get; set; }

        #endregion CountryAggregate

        #region CurrencyAggregate

        public virtual DbSet<Currency> Currencies { get; set; }

        #endregion CurrencyAggregate

        #region DayTypeAggregate

        public virtual DbSet<DayType> DayTypes { get; set; }

        #endregion DayTypeAggregate

        #region DivisionAggregate

        public virtual DbSet<Division> Divisons { get; set; }
        public virtual DbSet<DivisionTranslation> DivisonTranslations { get; set; }

        #endregion DivisionAggregate

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

        #region EmailAggregate
        public virtual DbSet<Email> Emails { get; set; }
        #endregion

        #region EmailDataAggregate
        public virtual DbSet<EmailData> EmailDatas { get; set; }
        public virtual DbSet<EmailDataTranslation> EmailDataTranslations { get; set; }
        #endregion

        #region EmployeeAggregate

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAbsenceDay> GetEmployeeAbsenceDays { get; set; }

        #endregion EmployeeAggregate

        #region GeneralExpenseAggregate

        public virtual DbSet<GeneralExpense> GeneralExpenses { get; set; }
        public virtual DbSet<GeneralExpenseRule> GeneralExpenseRules { get; set; }
        public virtual DbSet<FrequencyType> FrequencyTypes { get; set; }
        public virtual DbSet<FrequencyTypeTranslation> FrequencyTypeTranslations { get; set; }

        #endregion GeneralExpenseAggregate

        #region FurnitureComponentAggregate

        public virtual DbSet<FurnitureComponent> FurnitureComponents { get; set; }

        #endregion FurnitureComponentAggregate

        #region FurnitureUnitAggregate

        public virtual DbSet<FurnitureUnit> FurnitureUnits { get; set; }
        public virtual DbSet<FurnitureUnitPrice> FurnitureUnitPrices { get; set; }
        public virtual DbSet<FurnitureUnitTranslation> FurnitureUnitTranslations { get; set; }

        #endregion FurnitureUnitAggregate

        #region GroupingCategoryAggregate

        public virtual DbSet<GroupingCategory> GroupingCategories { get; set; }
        public virtual DbSet<GroupingCategoryTranslation> GroupingCategoryTranslations { get; set; }

        #endregion GroupingCategoryAggregate

        #region ImageAggregate

        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ScheduleZipFile> ScheduleZipFiles { get; set; }
        public virtual DbSet<LayoutPlanZipFile> LayoutPlanZipFiles { get; set; }

        #endregion

        #region InspectionAggregate

        public virtual DbSet<Inspection> Inspections { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<StockReport> StockReports { get; set; }

        #endregion InspectionAggregate

        #region LanguageAggregate
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageTranslation> LanguageTranslations { get; set; }
        #endregion

        #region MachineAggregate

        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<CuttingMachine> CuttingMachines { get; set; }
        public virtual DbSet<EdgingMachine> EdgingMachines { get; set; }
        public virtual DbSet<CncMachine> CncMachines { get; set; }
        #endregion

        #region MaterialAggregate
        public virtual DbSet<BoardMaterial> BoardMaterials { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<DecorBoardMaterial> DecorBoardMaterials { get; set; }
        public virtual DbSet<WorktopBoardMaterial> WorktopBoardMaterials { get; set; }
        public virtual DbSet<FoilMaterial> FoilMaterials { get; set; }
        public virtual DbSet<ApplianceMaterial> ApplianceMaterials { get; set; }
        public virtual DbSet<AccessoryMaterial> AccessoryMaterials { get; set; }

        #endregion MaterialAggregate

        #region AccessoryMaterialFurnitureUnitAggregate

        public virtual DbSet<AccessoryMaterialFurnitureUnit> AccessoryMaterialFurnitureUnits { get; set; }

        #endregion

        #region OrderAggregate

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<RequiredMaterial> RequiredMaterials { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<OrderPrice> OrderPrices { get; set; }

        #endregion OrderAggregate

        #region OptimizationAggregate

        public virtual DbSet<Optimization> Optimizations { get; set; }

        #endregion OptimizationAggregate

        #region OrderedApplianceMaterialAggregate

        public virtual DbSet<ConcreteApplianceMaterial> ConcreteApplianceMaterials { get; set; }

        #endregion

        #region PackageAggregate

        public virtual DbSet<MaterialPackage> MaterialPackages { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        #endregion PackageAggregate

        #region PlanAggregate

        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Cutting> Cuttings { get; set; }
        #endregion PlanAggregate

        #region RelativePointAggregate

        public virtual DbSet<RelativePoint> RelativePoints { get; set; }

        #endregion RelativePointAggregate

        #region RoleAggregate

        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<DefaultRoleClaim> DefaultRoleClaims { get; set; }
        public virtual DbSet<RoleTranslation> RoleTranslations { get; set; }

        #endregion RoleAggregate

        #region SequenceAggregate

        public virtual DbSet<Sequence> Sequences { get; set; }

        #endregion

        #region SiUnitAggregate

        public virtual DbSet<SiUnit> SiUnits { get; set; }
        public virtual DbSet<SiUnitTranslation> SiUnitTranslations { get; set; }

        #endregion SiUnitAggregate

        #region SortingStrategyTypeAggregate

        public virtual DbSet<SortingStrategyType> SortingStrategyTypes { get; set; }
        public virtual DbSet<SortingStrategyTypeTranslation> SortingStrategyTypeTranslations { get; set; }

        #endregion

        #region StockAggregate

        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockedMaterial> StockedMaterials { get; set; }

        #endregion StockAggregate

        #region StorageCellAggregate

        public virtual DbSet<StorageCell> StorageCells { get; set; }

        #endregion StorageCellAggregate

        #region StorageAggregate

        public virtual DbSet<Storage> Storages { get; set; }

        #endregion StorageAggregate

        #region ToolAggregate

        public virtual DbSet<Tool> Tools { get; set; }

        #endregion ToolAggregate

        #region UserAggregate
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<UserInspection> UserInspections { get; set; }
        #endregion UserAggregate

        #region UserTeamAggregate

        public virtual DbSet<UserTeam> UserTeams { get; set; }

        #endregion UserTeamAggregate

        #region WorkStationAggregate

        public virtual DbSet<WorkStation> WorkStations { get; set; }

        #endregion

        #region WorkStationTypeAggregate

        public virtual DbSet<WorkStationType> WorkStationTypes { get; set; }
        public virtual DbSet<WorkStationTypeTranslation> WorkStationTranslations { get; set; }

        #endregion

        #region VenueAggregate
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<MeetingRoom> MeetingRooms { get; set; }
        public virtual DbSet<MeetingRoomTranslation> MeetingRoomTranslations { get; set; }
        public virtual DbSet<VenueDateRange> VenueDateRanges { get; set; }
        #endregion

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

            await Task.WhenAll(tasks);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

            #region CameraAggregate
            modelBuilder.ApplyConfiguration(new CameraConfiguration());
            #endregion

            #region CargoAggregate

            modelBuilder.ApplyConfiguration(new CargoConfiguration());
            modelBuilder.ApplyConfiguration(new OrderedMaterialPackageConfiguration());

            #endregion CargoAggregate

            #region CargoStatusTypeAggregate

            modelBuilder.ApplyConfiguration(new CargoStatusTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CargoStatusTypeTranslationConfiguration());

            #endregion CargoStatusTypeAggregate

            #region CFCProductionState

            modelBuilder.ApplyConfiguration(new CFCProductionStateConfiguration());
            modelBuilder.ApplyConfiguration(new CFCProductionStateTranslationConfiguration());

            #endregion

            #region CompanyAggregate

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyDataConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyDateRangeConfiguration());

            #endregion CompanyAggregate

            #region CompanyTypeAggregate

            modelBuilder.ApplyConfiguration(new CompanyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTypeTranslationConfiguration());

            #endregion CompanyTypeAggregate

            #region ConcreteFurnitureUnitAggregate

            modelBuilder.ApplyConfiguration(new ConcreteFurnitureUnitConfiguration());

            #endregion ConcreteFurnitureUnitAggregate

            #region ConcreteFurnitureComponentAgggregate

            modelBuilder.ApplyConfiguration(new ConcreteFurnitureComponentConfiguration());

            #endregion ConcreteFurnitureComponentAgggregate        

            #region CountryAggregate

            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CountryTranslationConfiguration());

            #endregion CountryAggregate

            #region DayTypeAggregate

            modelBuilder.ApplyConfiguration(new DayTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DayTypeTranslationConfiguration());

            #endregion DayTypeAggregate

            #region DivisionAggregate

            modelBuilder.ApplyConfiguration(new DivisionConfiguration());
            modelBuilder.ApplyConfiguration(new DivisionTranslationConfiguration());

            #endregion DivisionAggregate

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

            #region EmailAggregate
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
            #endregion

            #region EmailDataAggregate
            modelBuilder.ApplyConfiguration(new EmailDataConfiguration());
            modelBuilder.ApplyConfiguration(new EmailDataTranslationConfiguration());
            #endregion

            #region FurnitureComponentAggregate

            modelBuilder.ApplyConfiguration(new FurnitureComponentConfiguration());

            #endregion FurnitureComponentAggregate

            #region FurnitureUnitAggregate

            modelBuilder.ApplyConfiguration(new FurnitureUnitConfiguration());
            modelBuilder.ApplyConfiguration(new FurnitureUnitPriceConfiguration());
            modelBuilder.ApplyConfiguration(new FurnitureUnitTranslationConfiguration());

            #endregion FurnitureUnitAggregate

            #region GeneralExpenseAggregate

            modelBuilder.ApplyConfiguration(new GeneralExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new GeneralExpensesTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FrequencyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FrequencyTypeTranslationsConfiguration());

            #endregion GeneralExpenseAggregate

            #region GroupingCategoryAggregate

            modelBuilder.ApplyConfiguration(new GroupingCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new GroupingCategoryTranslationConfiguration());

            #endregion GroupingCategoryAggregate

            #region ImageAggregate

            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new LayoutPlanZipFileConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleHtmlFileConfiguration());

            #endregion ImageAggregate

            #region InspectionAggregate

            modelBuilder.ApplyConfiguration(new InspectionConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new StockReportConfiguration());

            #endregion InspectionAggregate

            #region LanguageAggregate
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageTranslationConfiguration());
            #endregion

            #region MachineAggregate

            modelBuilder.ApplyConfiguration(new MachineConfiguration());
            modelBuilder.ApplyConfiguration(new EdgingMachineConfiguration());
            modelBuilder.ApplyConfiguration(new CncMachineConfiguration());
            modelBuilder.ApplyConfiguration(new CuttingMachineConfiguration());
            modelBuilder.ApplyConfiguration(new ToolConfiguration());

            #endregion MachineAggregate

            #region MaterialAggregate

            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialPriceConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new AccessoryMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new ApplianceMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new FoilMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new BoardMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new DecorBoardMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new WorktopBoardMaterialConfiguration());

            #endregion MaterialAggregate

            #region AccessoryMaterialFurnitureUnitAggregate

            modelBuilder.ApplyConfiguration(new AccessoryMaterialFurnitureUnitConfiguration());

            #endregion

            #region OrderAggregate

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ConcreteApplianceMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new RequiredMaterialConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new OrderPriceConfiguration());

            #endregion OrderAggregate

            #region OptimizationAggregate

            modelBuilder.ApplyConfiguration(new OptimizatonConfiguration());

            #endregion OptimizationAggregate

            #region OrderStateAggregate

            modelBuilder.ApplyConfiguration(new OrderStateConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStateTranslationConfiguration());

            #endregion OrderStateAggregate

            #region OrderStatusTypeAggregate

            modelBuilder.ApplyConfiguration(new OrderStatusTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusTypeTranslationConfiguration());

            #endregion OrderStatusTypeAggregate

            #region PackageAggregate
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialPackageConfiguration());
            #endregion

            #region PlanAggregate

            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new LayoutPlanConfiguration());

            modelBuilder.ApplyConfiguration(new CncPlanConfiguration());
            modelBuilder.ApplyConfiguration(new ManualLaborPlanConfiguration());
            modelBuilder.ApplyConfiguration(new CuttingConfiguration());

            #endregion PlanAggregate

            #region PlaneTypeAggregate

            modelBuilder.ApplyConfiguration(new PlaneTypeConfiguration());

            #endregion PlaneTypeAggregate

            #region ProcessAggregate

            modelBuilder.ApplyConfiguration(new ProductionProcessConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessWorkerConfiguration());

            #endregion

            #region RoleAggregate

            modelBuilder.ApplyConfiguration(new ClaimConfiguration());
            modelBuilder.ApplyConfiguration(new DefaultRoleClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleTranslationConfiguration());

            #endregion RoleAggregate

            #region SequenceAggregate

            modelBuilder.ApplyConfiguration(new SequenceConfiguration());
            modelBuilder.ApplyConfiguration(new RectangleConfiguration());

            modelBuilder.ApplyConfiguration(new CircleConfiguration());
            modelBuilder.ApplyConfiguration(new PolygonConfiguration());
            modelBuilder.ApplyConfiguration(new DrillConfiguration());
            modelBuilder.ApplyConfiguration(new CommadConfiguration());
            modelBuilder.ApplyConfiguration(new ArcConfiguration());
            modelBuilder.ApplyConfiguration(new LineConfiguration());
            modelBuilder.ApplyConfiguration(new HoleConfiguration());
            modelBuilder.ApplyConfiguration(new RelativePointConfiguration());

            #endregion SequenceAggregate

            #region SiUnitAggregate

            modelBuilder.ApplyConfiguration(new SiUnitConfiguration());
            modelBuilder.ApplyConfiguration(new SiUnitTranslationConfiguration());

            #endregion SiUnitAggregate

            #region SortingStrategyTypes

            modelBuilder.ApplyConfiguration(new SortingStrategyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SortingStrategyTypeTranslationConfiguration());

            #endregion

            #region StorageAggregate

            modelBuilder.ApplyConfiguration(new StorageConfiguration());

            #endregion StorageAggregate

            #region StorageCellAggregate

            modelBuilder.ApplyConfiguration(new StorageCellConfiguration());

            #endregion StorageCellAggregate

            #region StockAggregate

            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new StockedMaterialConfiguration());

            #endregion StockAggregate

            #region UserAggregate

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserDataConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserInspectionConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());

            #endregion UserAggregate

            #region UserTeamAggregate

            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());

            #endregion UserTeamAggregate

            #region VenueAggregate
            modelBuilder.ApplyConfiguration(new VenueConfiguration());
            modelBuilder.ApplyConfiguration(new VenueDateRangeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingRoomConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingRoomTranslationConfiguration());
            #endregion

            #region WorkStationAggregate

            modelBuilder.ApplyConfiguration(new WorkStationConfiguration());

            #endregion

            #region WorkStationTypeAggregate

            modelBuilder.ApplyConfiguration(new WorkStationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WorkStationTypeTranslationConfiguration());

            #endregion

            SeedInitialData(modelBuilder);
#if DEBUG
            SeedDebugData(modelBuilder);
#endif
        }

        protected virtual void SeedInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(new CurrencySeed().Entities);
            modelBuilder.Entity<EmailData>().HasData(new EmailDataSeed().Entities);
            modelBuilder.Entity<EmailDataTranslation>().HasData(new EmailDataTranslationSeed().Entities);
            modelBuilder.Entity<CFCProductionState>().HasData(new CFCProductionStateSeed().Entities);
            modelBuilder.Entity<CFCProductionStateTranslation>().HasData(new CFCProductionStateTranslationSeed().Entities);
            modelBuilder.Entity<Language>().HasData(new LanguageSeed().Entities);
            modelBuilder.Entity<LanguageTranslation>().HasData(new LanguageTranslationSeed().Entities);
            modelBuilder.Entity<DayType>().HasData(new DayTypeSeed().Entities);
            modelBuilder.Entity<DayTypeTranslation>().HasData(new DayTypeTranslationSeed().Entities);
            modelBuilder.Entity<GroupingCategory>().HasData(new GroupingCategorySeed().Entities);
            modelBuilder.Entity<GroupingCategoryTranslation>().HasData(new GroupingCategoryTranslationSeed().Entities);
            modelBuilder.Entity<Country>().HasData(new CountrySeed().Entities);
            modelBuilder.Entity<CountryTranslation>().HasData(new CountryTranslationSeed().Entities);
            modelBuilder.Entity<CompanyType>().HasData(new CompanyTypeSeed().Entities);
            modelBuilder.Entity<CompanyTypeTranslation>().HasData(new CompanyTypeTranslationSeed().Entities);
            modelBuilder.Entity<SiUnit>().HasData(new SiUnitSeed().Entities);
            modelBuilder.Entity<CargoStatusTypeTranslation>().HasData(new CargoStatusTypeTranslationSeed().Entities);
            modelBuilder.Entity<CargoStatusType>().HasData(new CargoStatusTypeSeed().Entities);
            modelBuilder.Entity<OrderState>().HasData(new OrderStateSeed().Entities);
            modelBuilder.Entity<OrderStateTranslation>().HasData(new OrderStateTranslationSeed().Entities);
            modelBuilder.Entity<FrequencyType>().HasData(new FrequencyTypeSeed().Entities);
            modelBuilder.Entity<FrequencyTypeTranslation>().HasData(new FrequencyTypeTranslationsSeed().Entities);
            modelBuilder.Entity<Division>().HasData(new DivisionSeed().Entities);
            modelBuilder.Entity<DivisionTranslation>().HasData(new DivisionTranslationSeed().Entities);
            modelBuilder.Entity<Image>().HasData(new ImageSeed().Entities);
            modelBuilder.Entity<Claim>().HasData(new ClaimSeed().Entities);
            modelBuilder.Entity<Role>().HasData(new RoleSeed().Entities);
            modelBuilder.Entity<DefaultRoleClaim>().HasData(new DefaultRoleClaimsSeed().Entities);
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslationSeed().Entities);
            modelBuilder.Entity<WorkStationType>().HasData(new WorkStationTypeSeed().Entities);
            modelBuilder.Entity<WorkStationTypeTranslation>().HasData(new WorkStationTypeTranslationSeed().Entities);
            modelBuilder.Entity<SortingStrategyType>().HasData(new SortingStrategyTypeSeed().Entities);
            modelBuilder.Entity<SortingStrategyTypeTranslation>().HasData(new SortingStrategyTypeTranslationSeed().Entities);


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
                },
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
                },
                new
                {
                    CompanyDataId = 7,
                    AddressValue = "Váci út 80",
                    City = "Budapest",
                    PostCode = 1133,
                    CountryId = 1
                }
                ));

            modelBuilder.Entity<User>().HasData(new UserSeed().ReleaseEntities);
            modelBuilder.Entity<UserRole>().HasData(new UserRoleSeed().ReleaseEntities);
            modelBuilder.Entity<UserClaim>().HasData(new UserClaimSeed().ReleaseEntities);
            modelBuilder.Entity<UserData>().HasData(new UserDataSeed().ReleaseEntities);
            modelBuilder.Entity<UserData>(entity => entity.OwnsOne(e => e.ContactAddress).HasData(
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

        #region SeedDebugData
        protected virtual void SeedDebugData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>().HasData(new CameraSeed().Entities);

            modelBuilder.Entity<MaterialPackage>().HasData(new MaterialPackageSeed().Entities);
            modelBuilder.Entity<MaterialPackage>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 1
                },
                new
                {
                    Value = 8000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 2
                },
                new
                {
                    Value = 12000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 3
                },
                new
                {
                    Value = 4000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 4
                },
                new
                {
                    Value = 500.0,
                    CurrencyId = 1,
                    MaterialPackageId = 5
                },
                new
                {
                    Value = 1700.0,
                    CurrencyId = 1,
                    MaterialPackageId = 6
                },
                new
                {
                    Value = 1250.0,
                    CurrencyId = 1,
                    MaterialPackageId = 7
                },
                new
                {
                    Value = 8500.0,
                    CurrencyId = 1,
                    MaterialPackageId = 8
                },
                new
                {
                    Value = 2500.0,
                    CurrencyId = 1,
                    MaterialPackageId = 9
                },
                new
                {
                    Value = 4600.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10
                },
                new
                {
                    Value = 3700.0,
                    CurrencyId = 1,
                    MaterialPackageId = 11
                }
            ));



            modelBuilder.Entity<DecorBoardMaterial>().HasData(new DecorBoardMaterialSeed().Entities);
            modelBuilder.Entity<DecorBoardMaterial>(ent => ent.OwnsOne(e => e.Dimension).HasData(
                new
                {
                    BoardMaterialId = new Guid("4a7b9b0a-2299-4bb2-95ad-4f1b0f23a47f"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("0b02521a-1442-4eae-a41a-b65623502b60"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"),
                    Width = 2000.0,
                    Length = 2700.0,
                    Thickness = 4.0
                },
                new
                {
                    BoardMaterialId = new Guid("62d6de6f-3e2f-4549-b219-c9b0eeaa5424"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                }));

            modelBuilder.Entity<MaterialPrice>().HasData(new MaterialPriceSeed().Entities);
            modelBuilder.Entity<MaterialPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
               new
               {
                   MaterialPriceId = 1,
                   Value = 100.0,
                   CurrencyId = 1,
               },
               new
               {
                   MaterialPriceId = 2,
                   Value = 30.5,
                   CurrencyId = 2,
               }
            ));

            modelBuilder.Entity<FoilMaterial>().HasData(new FoilMaterialSeed().Entities);
            modelBuilder.Entity<AccessoryMaterial>().HasData(new AccessoryMaterialSeed().Entities);
            modelBuilder.Entity<ApplianceMaterial>().HasData(new ApplianceMaterialSeed().Entities);
            modelBuilder.Entity<ApplianceMaterial>(ent => ent.OwnsOne(e => e.SellPrice).HasData(
               new
               {
                   ApplianceMaterialId = new Guid("62d75a9f-702e-406b-bdc2-29152ca58f36"),
                   Value = 10000.0,
                   CurrencyId = 1
               },
               new
               {
                   ApplianceMaterialId = new Guid("54153d0d-7d9e-4491-8e78-f505f6440e93"),
                   Value = 120000.0,
                   CurrencyId = 1
               }
            ));

            modelBuilder.Entity<WorktopBoardMaterial>().HasData(new WorkTopBoardMaterialSeed().Entities);
            modelBuilder.Entity<WorktopBoardMaterial>(ent => ent.OwnsOne(e => e.Dimension).HasData(
                new
                {
                    BoardMaterialId = new Guid("c634ff73-b129-4cc7-822b-8658ef9b882f"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                }
            ));

            modelBuilder.Entity<RequiredMaterial>().HasData(new RequiredMaterialSeed().Entities);

            modelBuilder.Entity<Company>().HasData(new CompanySeed().Entities);
            modelBuilder.Entity<CompanyData>().HasData(new CompanyDataSeed().Entities);


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
                }));

            modelBuilder.Entity<MeetingRoom>().HasData(new MeetingRoomSeed().Entities);
            modelBuilder.Entity<MeetingRoomTranslation>().HasData(new MeetingRoomTranslationSeed().Entities);

            modelBuilder.Entity<Storage>().HasData(new StorageSeed().Entities);
            modelBuilder.Entity<Storage>(ent => ent.OwnsOne(e => e.Address).HasData(
                new
                {
                    StorageId = 1,
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    StorageId = 2,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                }
            ));

            modelBuilder.Entity<StorageCell>().HasData(new StorageCellSeed().Entities);
            modelBuilder.Entity<Stock>().HasData(new StockSeed().Entities);
            modelBuilder.Entity<StockedMaterial>().HasData(new StockedMaterialSeed().Entities);


            modelBuilder.Entity<Cargo>().HasData(new CargoSeed().Entities);
            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.NetCost).HasData(
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    CargoId = 1
                },
                new
                {
                    Value = 8000.0,
                    CurrencyId = 1,
                    CargoId = 2
                },
                new
                {
                    Value = 7000.0,
                    CurrencyId = 1,
                    CargoId = 3
                },
                new
                {
                    Value = 6000.0,
                    CurrencyId = 1,
                    CargoId = 4
                },
                new
                {
                    Value = 5000.0,
                    CurrencyId = 1,
                    CargoId = 5
                },
                new
                {
                    Value = 12000.0,
                    CurrencyId = 1,
                    CargoId = 6
                },
                new
                {
                    Value = 11000.0,
                    CurrencyId = 1,
                    CargoId = 7
                }

            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.Vat).HasData(
                new
                {
                    Value = 1000.0,
                    CurrencyId = 1,
                    CargoId = 1
                },
                new
                {
                    Value = 800.0,
                    CurrencyId = 1,
                    CargoId = 2
                },
                new
                {
                    Value = 700.0,
                    CurrencyId = 1,
                    CargoId = 3
                },
                new
                {
                    Value = 600.0,
                    CurrencyId = 1,
                    CargoId = 4
                },
                new
                {
                    Value = 500.0,
                    CurrencyId = 1,
                    CargoId = 5
                },
                new
                {
                    Value = 1200.0,
                    CurrencyId = 1,
                    CargoId = 6
                },
                new
                {
                    Value = 1100.0,
                    CurrencyId = 1,
                    CargoId = 7
                }
            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.ShippingAddress).HasData(
                new
                {
                    CargoId = 1,
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 2,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 3,
                    PostCode = 1076,
                    City = "Budapest",
                    AddressValue = "Damjanich utca 18.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 4,
                    PostCode = 1032,
                    City = "Budapest",
                    AddressValue = "Bud Spencer park 1.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 5,
                    PostCode = 1012,
                    City = "Budapest",
                    AddressValue = "Fő utca 9.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 6,
                    PostCode = 1118,
                    City = "Budapest",
                    AddressValue = "Sasadi út 56.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 7,
                    PostCode = 1067,
                    City = "Budapest",
                    AddressValue = "Szív utca 7.",
                    CountryId = 1
                }
            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.ShippingCost).HasData(
                new
                {
                    Value = 1100.0,
                    CurrencyId = 1,
                    CargoId = 1
                },
                new
                {
                    Value = 900.0,
                    CurrencyId = 1,
                    CargoId = 2
                },
                new
                {
                    Value = 750.0,
                    CurrencyId = 1,
                    CargoId = 3
                },
                new
                {
                    Value = 650.0,
                    CurrencyId = 1,
                    CargoId = 4
                },
                new
                {
                    Value = 550.0,
                    CurrencyId = 1,
                    CargoId = 5
                },
                new
                {
                    Value = 1250.0,
                    CurrencyId = 1,
                    CargoId = 6
                },
                new
                {
                    Value = 1150.0,
                    CurrencyId = 1,
                    CargoId = 7
                }
            ));

            modelBuilder.Entity<Ticket>().HasData(new TicketSeed().Entities);

            modelBuilder.Entity<Optimization>().HasData(new OptimizationSeed().Entities);

            modelBuilder.Entity<OrderPrice>().HasData(new OrderPriceSeed().Entities);
            modelBuilder.Entity<OrderPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
               new
               {
                   OrderPriceId = 1,
                   Value = 10000.0,
                   CurrencyId = 1
               },
               new
               {
                   OrderPriceId = 2,
                   Value = 120000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 3,
                   Value = 170000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 4,
                   Value = 170000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 5,
                   Value = 100000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 6,
                   Value = 190000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 7,
                   Value = 130000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 8,
                   Value = 125000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 9,
                   Value = 129000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 10,
                   Value = 100000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 11,
                   Value = 220000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 12,
                   Value = 260000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 13,
                   Value = 180000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 14,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 15,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 16,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 17,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 18,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 19,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 20,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 21,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 22,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 23,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 24,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 25,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 26,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 27,
                   Value = 320000.0,
                   CurrencyId = 1
               }, new
               {
                   OrderPriceId = 28,
                   Value = 320000.0,
                   CurrencyId = 1
               }
            ));
            modelBuilder.Entity<Order>().HasData(new OrderSeed().Entities);
            modelBuilder.Entity<Order>(entity => entity.OwnsOne(e => e.ShippingAddress).HasData(
                new
                {
                    OrderId = new Guid("D116F5CA-58AF-47A4-92A2-1FB2ABC31F51"),
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("409F08EE-7FAD-43D2-A6CB-E0ED274D9CB9"),
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("0D9F83DC-9143-49E5-9DD7-8DC702F130CB"),
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Hegyalja út 27.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("83B6E1CB-215B-42D2-93BB-5DBA12FE039E"),
                    PostCode = 1118,
                    City = "Budapest",
                    AddressValue = "Villányi út 82.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("50EBFCBD-33DC-4BF4-B82D-6336E1EA6F48"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 1.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("627966DE-6BD4-4EED-B07C-8D78046509C8"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 2.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("B4B68154-674F-44E0-86C5-8FF47914DC1C"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 3.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("3B290920-BDA8-41DA-BF7E-B3F8770CD5B5"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 4.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("AB6F5475-C916-40D9-989A-59ECDA3833A9"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 5.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("E090C246-CF62-4BDC-B545-E1B50C4BF8B1"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 6.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("FBABFC83-F103-4E1C-9592-365E62CAF909"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 1.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("38281E86-36AC-4AF7-A0C5-B04FAE72499B"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 8.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("BE290328-EDF9-4A23-8225-8058F107AD98"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 7.",
                    CountryId = 1
                }, new
                {
                    OrderId = new Guid("0C60CDBC-FCE3-4833-8FFA-D46664A68DA3"),
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Móricz Zsigmond körtér 8.",
                    CountryId = 1
                }
            ));
            modelBuilder.Entity<OrderedMaterialPackage>().HasData(new OrderedMaterialPackageSeed().Entities);
            modelBuilder.Entity<OrderedMaterialPackage>(ent => ent.OwnsOne(e => e.UnitPrice).HasData(
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 1
                },
                new
                {
                    Value = 6200.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 2
                },
                new
                {
                    Value = 5500.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 3
                },
                new
                {
                    Value = 8400.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 4
                },
                new
                {
                    Value = 7100.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 5
                },
                new
                {
                    Value = 3400.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 6
                },
                new
                {
                    Value = 20000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 7
                },
                new
                {
                    Value = 42000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 8
                }
            ));

            modelBuilder.Entity<GeneralExpenseRule>().HasData(new GeneralExpenseRuleSeed().Entities);
            modelBuilder.Entity<GeneralExpenseRule>(entity => entity.OwnsOne(e => e.Amount).HasData(
                new
                {
                    Value = 6000.0,
                    CurrencyId = 1,
                    GeneralExpenseRuleId = 1
                },
                new
                {
                    Value = 60000.0,
                    CurrencyId = 1,
                    GeneralExpenseRuleId = 2
                },
                new
                {
                    Value = 6000000.0,
                    CurrencyId = 1,
                    GeneralExpenseRuleId = 3
                }
            ));
            modelBuilder.Entity<GeneralExpense>().HasData(new GeneralExpenseSeed().Entities);
            modelBuilder.Entity<GeneralExpense>(entity => entity.OwnsOne(e => e.Cost).HasData(
                    new
                    {
                        GeneralExpenseId = 1,
                        Value = 6000.0,
                        CurrencyId = 1
                    }
            ));

            modelBuilder.Entity<User>().HasData(new UserSeed().Entities);
            modelBuilder.Entity<UserRole>().HasData(new UserRoleSeed().Entities);
            modelBuilder.Entity<UserClaim>().HasData(new UserClaimSeed().Entities);
            modelBuilder.Entity<UserData>().HasData(new UserDataSeed().Entities);
            modelBuilder.Entity<UserData>(entity => entity.OwnsOne(e => e.ContactAddress).HasData(
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
                    CountryId = 1
                },
                new
                {
                    UserDataId = 4,
                    PostCode = 1067,
                    City = "Budapest",
                    AddressValue = "Eötvös utca 20.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 5,
                    PostCode = 1076,
                    City = "Budapest",
                    AddressValue = "Garay tér 2.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 6,
                    PostCode = 1067,
                    City = "Budapest",
                    AddressValue = "Eötvös utca 20.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 7,
                    PostCode = 1067,
                    City = "Budapest",
                    AddressValue = "Csengery utca 13.",
                    CountryId = 1
                }
            ));

            modelBuilder.Entity<FurnitureUnit>().HasData(new FurnitureUnitSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>().HasData(new FurnitureUnitPriceSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    FurnitureUnitPriceId = 1,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 2,
                    Value = 30.0,
                    CurrencyId = 1
                }));
            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.MaterialCost).HasData(
                new
                {
                    FurnitureUnitPriceId = 1,
                    Value = 15.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 2,
                    Value = 25.0,
                    CurrencyId = 1
                }));

            modelBuilder.Entity<FurnitureComponent>().HasData(new FurnitureComponentSeed().Entities);
            modelBuilder.Entity<FurnitureComponent>(entity => entity.OwnsOne(e => e.CenterPoint).HasData(
                new
                {
                    FurnitureComponentId = new Guid("b17bfd96-adec-4e40-92ec-89f874499204"),
                    X = 2.3,
                    Y = 1.8,
                    Z = 1.9
                },
                new
                {
                    FurnitureComponentId = new Guid("bca55c43-55e1-4c7c-b080-60a12a6af765"),
                    X = 2.1,
                    Y = 1.8,
                    Z = 2.4
                },
                new
                {
                    FurnitureComponentId = new Guid("efbb21c9-39de-417e-8de4-3453d8fc3c1c"),
                    X = 4.1,
                    Y = 2.8,
                    Z = 2.0
                },
                new
                {
                    FurnitureComponentId = new Guid("cd44769e-7d10-4b72-8513-fcec1bba447a"),
                    X = 3.0,
                    Y = 1.4,
                    Z = 1.7
                },
                new
                {
                    FurnitureComponentId = new Guid("8f092adb-cf14-4d7f-999c-62c68a1d9e87"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("6696676b-12d0-4511-b08f-b7d820d2d395"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("dd9f3d57-bf6f-4de1-b5a9-43f0f581f693"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("72171d05-4b62-41c8-ad47-e45e606a0da8"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("225ca25c-b36a-42f7-bed8-964e4f7623ee"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("9e0bfab1-3642-46d8-94ac-1494e4416b11"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("3d3a82bc-b660-4d55-9bd6-314c08eb0dad"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                },
                new
                {
                    FurnitureComponentId = new Guid("c63f05f0-cb6b-480c-a4f1-71ca9d3aa7f5"),
                    X = 1.5,
                    Y = 1.9,
                    Z = 2.1
                }
            ));


            modelBuilder.Entity<ConcreteFurnitureComponent>().HasData(new ConcreteFurnitureComponentSeed().Entities);
            modelBuilder.Entity<ConcreteFurnitureUnit>().HasData(new ConcreteFurnitureUnitSeed().Entities);

            modelBuilder.Entity<AccessoryMaterialFurnitureUnit>().HasData(new AccessoryMaterialFurnitureUnitSeed().Entities);

            modelBuilder.Entity<Machine>().HasData(new MachineSeed().Entities);
            modelBuilder.Entity<CuttingMachine>().HasData(new CuttingMachineSeed().Entities);
            modelBuilder.Entity<EdgingMachine>().HasData(new EdgingMachineSeed().Entities);
            modelBuilder.Entity<CncMachine>().HasData(new CncMachineSeed().Entities);
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.DrillPropeties).HasData(
                new
                {
                    CncMachineId = 3,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 4,
                    DrillSpeed = 48.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 8,
                    DrillSpeed = 48.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 9,
                    DrillSpeed = 48.0,
                    DrillType = DrillTypeEnum.Simple
                }
            ));
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.EstimatorProperties).HasData(
               new
               {
                   CncMachineId = 3,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 4,
                   EstimatedMillingSpeed = 17.0,
                   EstimatedDrillSpeed = 28.0,
                   EstimatedRapidSpeed = 53.0,
                   ToolChangeTime = 430.0,
                   PlaneChangeTime = 570.0
               },
               new
               {
                   CncMachineId = 8,
                   EstimatedMillingSpeed = 17.0,
                   EstimatedDrillSpeed = 28.0,
                   EstimatedRapidSpeed = 53.0,
                   ToolChangeTime = 430.0,
                   PlaneChangeTime = 570.0
               },
              new
              {
                  CncMachineId = 9,
                  EstimatedMillingSpeed = 17.0,
                  EstimatedDrillSpeed = 28.0,
                  EstimatedRapidSpeed = 53.0,
                  ToolChangeTime = 430.0,
                  PlaneChangeTime = 570.0
              }

           ));
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.MillingProperties).HasData(
                new
                {
                    CncMachineId = 3,
                    MillingSpeed = 21.0,
                    MillingDiameter = 17.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 4,
                    MillingSpeed = 19.0,
                    MillingDiameter = 13.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 8,
                    MillingSpeed = 19.0,
                    MillingDiameter = 13.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 9,
                    MillingSpeed = 19.0,
                    MillingDiameter = 13.0,
                    SpinClockwise = true
                }
            ));


            modelBuilder.Entity<WorkStation>().HasData(new WorkStationSeed().Entities);

            modelBuilder.Entity<LayoutPlan>().HasData(new LayoutPlanSeed().Entities);
            modelBuilder.Entity<CncPlan>().HasData(new CncPlanSeed().Entities);
            modelBuilder.Entity<Plan>().HasData(new EdgingPlanSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new AssemblyPlanSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new SortingPlanSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new PackingPlanSeed().Entities);
            modelBuilder.Entity<ProductionProcess>().HasData(new ProductionProcessSeed().Entities);
            modelBuilder.Entity<ProcessWorker>().HasData(new ProcessWorkerSeed().Entities);

            modelBuilder.Entity<Sequence>().HasData(new SequenceSeed().Entities);
            modelBuilder.Entity<Command>().HasData(new CommandSeed().Entites);
            modelBuilder.Entity<Command>(ent => ent.OwnsOne(e => e.Point).HasData(
               new
               {
                   CommandId = 1,
                   X = 0.0,
                   Y = 0.0,
                   Z = 0.0

               }));
            modelBuilder.Entity<Drill>().HasData(new DrillSeed().Entities);
            //modelBuilder.Entity<Hole>().HasData(new HoleSeed().Entities);

            modelBuilder.Entity<Report>().HasData(new ReportSeed().Entities);
            modelBuilder.Entity<Inspection>().HasData(new InspectionSeed().Entities);
            modelBuilder.Entity<UserInspection>().HasData(new UserInspectionSeed().Entities);
            modelBuilder.Entity<StockReport>().HasData(new StockReportSeed().Entities);
        }
        #endregion
    }
}