using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Seed;
using IFPS.Factory.Domain.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace IFPS.Factory.EF
{
    public class IFPSFactoryTestContext : IFPSFactoryContext
    {
        public IFPSFactoryTestContext(DbContextOptions options)
            : base(options)
        {
        }

        public IFPSFactoryTestContext(DbContextOptions options,
            IIdentityService identityService,
            IMediator mediator)
        : base(options)
        {
            this.identityService = identityService;
            this.mediator = mediator;
        }

        protected override void SeedDebugData(ModelBuilder modelBuilder) { }

        #region SeedInitialData
        protected override void SeedInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(new CurrencySeed().Entities);
            modelBuilder.Entity<EmailData>().HasData(new EmailDataTestSeed().Entities);
            modelBuilder.Entity<EmailDataTranslation>().HasData(new EmailDataTranslationTestSeed().Entities);
            modelBuilder.Entity<CFCProductionState>().HasData(new CFCProductionStateTestSeed().Entities);
            modelBuilder.Entity<CFCProductionStateTranslation>().HasData(new CFCProductionStateTranslationTestSeed().Entities);
            modelBuilder.Entity<Image>().HasData(new ImageTestSeed().Entities);
            modelBuilder.Entity<Country>().HasData(new CountrySeed().Entities);
            modelBuilder.Entity<CountryTranslation>().HasData(new CountryTranslationSeed().Entities);
            modelBuilder.Entity<GroupingCategory>().HasData(new GroupingCategoryTestSeed().Entities);
            modelBuilder.Entity<GroupingCategoryTranslation>().HasData(new GroupingCategoryTranslationTestSeed().Entities);

            modelBuilder.Entity<CompanyType>().HasData(new CompanyTypeTestSeed().Entities);
            modelBuilder.Entity<Company>().HasData(new CompanyTestSeed().Entities);
            modelBuilder.Entity<CompanyData>().HasData(new CompanyDataTestSeed().Entities);
            modelBuilder.Entity<CompanyData>(ent => ent.OwnsOne(e => e.HeadOffice).HasData(
                new
                {
                    CompanyDataId = 10000,
                    AddressValue = "Petőfi utca 52.",
                    City = "Hajós",
                    PostCode = 6344,
                    CountryId = 1,
                },
                new
                {
                    CompanyDataId = 10001,
                    AddressValue = "Bocskai út 77-79.",
                    City = "Budapest",
                    PostCode = 1117,
                    CountryId = 1,
                }
            ));

            modelBuilder.Entity<MaterialPrice>().HasData(new MaterialPriceTestSeed().Entities);
            modelBuilder.Entity<MaterialPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
               new
               {
                   MaterialPriceId = 10000,
                   Value = 100.0,
                   CurrencyId = 1,
               },
               new
               {
                   MaterialPriceId = 10001,
                   Value = 30.5,
                   CurrencyId = 2,
               }
            ));

            modelBuilder.Entity<BoardMaterial>().HasData(new BoardMaterialTestSeed().Entities);
            modelBuilder.Entity<BoardMaterial>(ent => ent.OwnsOne(e => e.Dimension).HasData(
                new
                {
                    BoardMaterialId = new Guid("c649fc22-247e-4e88-817d-e398c349257b"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("b71e3923-3441-46c7-a2ba-13da290ecd6d"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("b2e0b4a3-8327-4836-fff5-deaec8b3c93c"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                }
            ));

            modelBuilder.Entity<DecorBoardMaterial>().HasData(new DecorBoardMaterialTestSeed().Entities);
            modelBuilder.Entity<DecorBoardMaterial>(ent => ent.OwnsOne(e => e.Dimension).HasData(
                new
                {
                    BoardMaterialId = new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("8b0936ec-0e49-4473-bb1d-2b8bb0564d34"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("3c3fa639-6451-49d7-b7b2-965092ebccf1"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                },
                new
                {
                    BoardMaterialId = new Guid("fb39bf09-e5e3-49c4-8e11-50782d5a5cad"),
                    Width = 1.0,
                    Length = 1.0,
                    Thickness = 1.0
                }
            ));

            modelBuilder.Entity<MaterialPackage>().HasData(new MaterialPackageTestSeed().Entities);
            modelBuilder.Entity<MaterialPackage>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10000
                },
                new
                {
                    Value = 8000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10001
                },
                new
                {
                    Value = 12000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10002
                },
                new
                {
                    Value = 4000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10003
                },
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    MaterialPackageId = 10004
                }
            ));

            modelBuilder.Entity<SiUnit>().HasData(new SiUnitTestSeed().Entities);
            modelBuilder.Entity<FoilMaterial>().HasData(new FoilMaterialTestSeed().Entities);

            modelBuilder.Entity<RequiredMaterial>().HasData(new RequiredMaterialTestSeed().Entities);

            modelBuilder.Entity<Storage>().HasData(new StorageTestSeed().Entities);
            modelBuilder.Entity<Storage>(ent => ent.OwnsOne(e => e.Address).HasData(
                new
                {
                    StorageId = 10000,
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    StorageId = 10001,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    StorageId = 10002,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    StorageId = 10003,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                }
            ));
            modelBuilder.Entity<StorageCell>().HasData(new StorageCellTestSeed().Entities);
            modelBuilder.Entity<Stock>().HasData(new StockTestSeed().Entities);
            modelBuilder.Entity<StockedMaterial>().HasData(new StockedMaterialTestSeed().Entities);


            modelBuilder.Entity<Ticket>().HasData(new TicketTestSeed().Entities);

            modelBuilder.Entity<OrderState>().HasData(new OrderStateTestSeed().Entities);
            modelBuilder.Entity<OrderStateTranslation>().HasData(new OrderStateTranslationTestSeed().Entities);
            // modelBuilder.Entity<OrderPrice>().HasData(new OrderPriceTestSeed().Entities);
            // modelBuilder.Entity<OrderPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
            //    new
            //    {
            //        OrderPriceId = 10000,
            //        Value = 10000.0,
            //        CurrencyId = 1
            //    },
            //    new
            //    {
            //        OrderPriceId = 10001,
            //        Value = 120000.0,
            //        CurrencyId = 1
            //    }
            // ));
            modelBuilder.Entity<Order>().HasData(new OrderTestSeed().Entities);
            modelBuilder.Entity<Order>(entity => entity.OwnsOne(e => e.ShippingAddress).HasData(
                new
                {
                    OrderId = new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"),
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("9aa53060-4b7a-4f82-8784-2bcc6313fbd3"),
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("5bab1970-41de-428f-876f-ac220bd0e7b1"),
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Hegyalja út 27.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("b8dd50e9-155d-449a-a45f-d027a2d43eba"),
                    PostCode = 1118,
                    City = "Budapest",
                    AddressValue = "Villányi út 82.",
                    CountryId = 1
                },
                new
                {
                    OrderId = new Guid("8be5952b-bfe0-42cf-9364-7a8bcb226843"),
                    PostCode = 1118,
                    City = "Budapest",
                    AddressValue = "Villányi út 82.",
                    CountryId = 1
                }
            ));
            modelBuilder.Entity<FrequencyType>().HasData(new FrequencyTypeSeed().Entities);
            modelBuilder.Entity<FrequencyTypeTranslation>().HasData(new FrequencyTypeTranslationsSeed().Entities);

            modelBuilder.Entity<GeneralExpenseRule>().HasData(new GeneralExpenseRuleTestSeed().Entities);
            modelBuilder.Entity<GeneralExpenseRule>(entity => entity.OwnsOne(e => e.Amount).HasData(
                    new
                    {
                        Value = 6000.0,
                        CurrencyId = 1,
                        GeneralExpenseRuleId = 10000
                    },
                    new
                    {
                        Value = 60000.0,
                        CurrencyId = 1,
                        GeneralExpenseRuleId = 10001
                    },
                    new
                    {
                        Value = 6000000.0,
                        CurrencyId = 1,
                        GeneralExpenseRuleId = 10002
                    }
            ));

            modelBuilder.Entity<GeneralExpense>().HasData(new GeneralExpenseTestSeed().Entities);
            modelBuilder.Entity<GeneralExpense>(entity => entity.OwnsOne(e => e.Cost).HasData(
                    new
                    {
                        GeneralExpenseId = 10000,
                        Value = 6000.0,
                        CurrencyId = 1
                    }
            ));
            modelBuilder.Entity<Division>().HasData(new DivisionTestSeed().Entities);
            modelBuilder.Entity<DivisionTranslation>().HasData(new DivisionTranslationTestSeed().Entities);
            modelBuilder.Entity<User>().HasData(new UserTestSeed().Entities);
            modelBuilder.Entity<Employee>().HasData(new EmployeeTestSeed().Entities);
            modelBuilder.Entity<Claim>().HasData(new ClaimTestSeed().Entities);
            modelBuilder.Entity<Role>().HasData(new RoleTestSeed().Entities);
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslationTestSeed().Entities);
            modelBuilder.Entity<DefaultRoleClaim>().HasData(new DefaultRoleClaimsTestSeed().Entities);

            modelBuilder.Entity<UserRole>().HasData(new UserRoleTestSeed().Entities);
            modelBuilder.Entity<UserClaim>().HasData(new UserClaimTestSeed().Entities);
            modelBuilder.Entity<UserData>().HasData(new UserDataTestSeed().Entities);
            modelBuilder.Entity<UserData>(entity => entity.OwnsOne(e => e.ContactAddress).HasData(
                new
                {
                    UserDataId = 10000,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Baranyai utca 7.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10001,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Baranyai utca 8.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10002,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Baranyai utca 7.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10003,
                    PostCode = 1067,
                    City = "Budapest",
                    AddressValue = "Eötvös utca 20.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10004,
                    PostCode = 1076,
                    City = "Budapest",
                    AddressValue = "Garay tér 2.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10005,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10006,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10007,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10008,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10009,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10010,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10011,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Seholse utca 1.",
                    CountryId = 1
                },
                new
                {
                    UserDataId = 10012,
                    PostCode = 1117,
                    City = "Budapest",
                    AddressValue = "Baranyai utca 7.",
                    CountryId = 1
                }
            ));

            modelBuilder.Entity<Inspection>().HasData(new InspectionTestSeed().Entities);
            modelBuilder.Entity<Report>().HasData(new ReportTestSeed().Entities);
            modelBuilder.Entity<StockReport>().HasData(new StockReportTestSeed().Entities);
            modelBuilder.Entity<UserInspection>().HasData(new UserInspectionTestSeed().Entities);

            modelBuilder.Entity<CargoStatusType>().HasData(new CargoStatusTypeTestSeed().Entities);
            modelBuilder.Entity<CargoStatusTypeTranslation>().HasData(new CargoStatusTypeTranslationTestSeed().Entities);
            modelBuilder.Entity<Cargo>().HasData(new CargoTestSeed().Entities);
            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.NetCost).HasData(
                new
                {
                    Value = 10000.0,
                    CurrencyId = 1,
                    CargoId = 10000
                },
                new
                {
                    Value = 8000.0,
                    CurrencyId = 1,
                    CargoId = 10001
                },
                new
                {
                    Value = 7500.0,
                    CurrencyId = 1,
                    CargoId = 10002
                },
                new
                {
                    Value = 999999.0,
                    CurrencyId = 1,
                    CargoId = 10003
                },
                new
                {
                    Value = 999999.0,
                    CurrencyId = 1,
                    CargoId = 10004
                },
                new
                {
                    Value = 999999.0,
                    CurrencyId = 1,
                    CargoId = 10005
                },
                new
                {
                    Value = 87345.0,
                    CurrencyId = 1,
                    CargoId = 10006
                }
            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.Vat).HasData(
                new
                {
                    Value = 1000.0,
                    CurrencyId = 1,
                    CargoId = 10000
                },
                new
                {
                    Value = 800.0,
                    CurrencyId = 1,
                    CargoId = 10001
                },
                new
                {
                    Value = 750.0,
                    CurrencyId = 1,
                    CargoId = 10002
                },
                new
                {
                    Value = 9999.0,
                    CurrencyId = 1,
                    CargoId = 10003
                },
                new
                {
                    Value = 9999.0,
                    CurrencyId = 1,
                    CargoId = 10004
                },
                new
                {
                    Value = 999999.0,
                    CurrencyId = 1,
                    CargoId = 10005
                },
                new
                {
                    Value = 87345.0,
                    CurrencyId = 1,
                    CargoId = 10006
                }
            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.ShippingAddress).HasData(
                new
                {
                    CargoId = 10000,
                    PostCode = 1064,
                    City = "Budapest",
                    AddressValue = "Szinyei Merse utca 13.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 10001,
                    PostCode = 1212,
                    City = "Budapest",
                    AddressValue = "Kuruclesi út 4.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 10002,
                    PostCode = 1085,
                    City = "Budapest",
                    AddressValue = "Kis Salétrom utca 5.",
                    CountryId = 1
                }, new
                {
                    CargoId = 10003,
                    PostCode = 9999,
                    City = "Delete Town",
                    AddressValue = "Delete street 66.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 10004,
                    PostCode = 1052,
                    City = "Budapest",
                    AddressValue = "Madách tér 2.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 10005,
                    PostCode = 1087,
                    City = "Budapest",
                    AddressValue = "Markusovszky tér 4.",
                    CountryId = 1
                },
                new
                {
                    CargoId = 10006,
                    PostCode = 8000,
                    City = "Székesfehérvár",
                    AddressValue = "III. Béla király tér 5.",
                    CountryId = 1
                }
            ));

            modelBuilder.Entity<Cargo>(ent => ent.OwnsOne(e => e.ShippingCost).HasData(
                new
                {
                    Value = 1100.0,
                    CurrencyId = 1,
                    CargoId = 10000
                },
                new
                {
                    Value = 900.0,
                    CurrencyId = 1,
                    CargoId = 10001
                },
                new
                {
                    Value = 550.0,
                    CurrencyId = 1,
                    CargoId = 10002
                },
                new
                {
                    Value = 999.0,
                    CurrencyId = 1,
                    CargoId = 10003
                },
                new
                {
                    Value = 999.0,
                    CurrencyId = 1,
                    CargoId = 10004
                },
                new
                {
                    Value = 999999.0,
                    CurrencyId = 1,
                    CargoId = 10005
                },
                 new
                 {
                     Value = 87345.0,
                     CurrencyId = 1,
                     CargoId = 10006
                 }
             ));

            modelBuilder.Entity<OrderedMaterialPackage>().HasData(new OrderedMaterialPackageTestSeed().Entities);
            modelBuilder.Entity<OrderedMaterialPackage>(ent => ent.OwnsOne(e => e.UnitPrice).HasData(
                new
                {
                    Value = 12000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10000
                },
                new
                {
                    Value = 14000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10001
                },
                new
                {
                    Value = 9999.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10002
                },
                new
                {
                    Value = 21000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10003
                },
                new
                {
                    Value = 58000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10004
                },
                new
                {
                    Value = 74000.0,
                    CurrencyId = 1,
                    OrderedMaterialPackageId = 10005
                }
            ));

            modelBuilder.Entity<FurnitureUnit>().HasData(new FurnitureUnitTestSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>().HasData(new FurnitureUnitPriceTestSeed().Entities);
            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.Price).HasData(
                new
                {
                    FurnitureUnitPriceId = 10000,
                    Value = 20.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 10001,
                    Value = 30.0,
                    CurrencyId = 1
                }));

            modelBuilder.Entity<FurnitureUnitPrice>(ent => ent.OwnsOne(e => e.MaterialCost).HasData(
                new
                {
                    FurnitureUnitPriceId = 10000,
                    Value = 15.0,
                    CurrencyId = 1
                },
                new
                {
                    FurnitureUnitPriceId = 10001,
                    Value = 25.0,
                    CurrencyId = 1
                }));

            modelBuilder.Entity<FurnitureComponent>().HasData(new FurnitureComponentTestSeed().Entities);
            modelBuilder.Entity<FurnitureComponent>(entity => entity.OwnsOne(e => e.CenterPoint).HasData(
                new
                {
                    FurnitureComponentId = new Guid("b3acef50-88cb-410f-a823-6d6b391611a5"),
                    X = 2.3,
                    Y = 1.8,
                    Z = 1.9
                },
                new
                {
                    FurnitureComponentId = new Guid("6dc5cac9-0339-429f-8302-41269b6192fe"),
                    X = 2.1,
                    Y = 1.8,
                    Z = 2.4
                },
                new
                {
                    FurnitureComponentId = new Guid("4aa9bcaf-63e5-4cd6-aa6c-bf571fefb597"),
                    X = 2.1,
                    Y = 1.8,
                    Z = 2.4
                }
            ));
            modelBuilder.Entity<ConcreteFurnitureUnit>().HasData(new ConcreteFurnitureUnitTestSeed().Entities);
            modelBuilder.Entity<ConcreteFurnitureComponent>().HasData(new ConcreteFurnitureComponentTestSeed().Entities);

            modelBuilder.Entity<Machine>().HasData(new MachineTestSeed().Entities);
            modelBuilder.Entity<CuttingMachine>().HasData(new CuttingMachineTestSeed().Entities);
            modelBuilder.Entity<CncMachine>().HasData(new CncMachineTestSeed().Entities);
            modelBuilder.Entity<EdgingMachine>().HasData(new EdgingMachineTestSeed().Entities);
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.DrillPropeties).HasData(
                new
                {
                    CncMachineId = 10002,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 10003,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 10004,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 10005,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 10010,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                },
                new
                {
                    CncMachineId = 10012,
                    DrillSpeed = 24.0,
                    DrillType = DrillTypeEnum.Simple
                }
            ));
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.EstimatorProperties).HasData(
               new
               {
                   CncMachineId = 10002,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 10003,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 10004,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 10005,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 10010,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               },
               new
               {
                   CncMachineId = 10012,
                   EstimatedMillingSpeed = 24.0,
                   EstimatedDrillSpeed = 31.0,
                   EstimatedRapidSpeed = 42.0,
                   ToolChangeTime = 340.0,
                   PlaneChangeTime = 560.0
               }
           ));
            modelBuilder.Entity<CncMachine>(entity => entity.OwnsOne(e => e.MillingProperties).HasData(
                new
                {
                    CncMachineId = 10002,
                    MillingSpeed = 21.0,
                    MillingDiameter = 17.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 10003,
                    MillingSpeed = 21.0,
                    MillingDiameter = 17.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 10004,
                    MillingSpeed = 21.0,
                    MillingDiameter = 17.0,
                    SpinClockwise = true
                },
                new
                {
                    CncMachineId = 10005,
                    MillingSpeed = 21.0,
                    MillingDiameter = 17.0,
                    SpinClockwise = true
                },
                 new
                 {
                     CncMachineId = 10010,
                     MillingSpeed = 21.0,
                     MillingDiameter = 17.0,
                     SpinClockwise = true
                 },
                 new
                 {
                     CncMachineId = 10012,
                     MillingSpeed = 21.0,
                     MillingDiameter = 17.0,
                     SpinClockwise = true
                 }
            ));

            modelBuilder.Entity<WorkStationType>().HasData(new WorkStationTypeTestSeed().Entities);
            modelBuilder.Entity<WorkStationTypeTranslation>().HasData(new WorkStationTypeTranslationTestSeed().Entities);
            modelBuilder.Entity<WorkStation>().HasData(new WorkStationTestSeed().Entities);
            modelBuilder.Entity<WorkStationCamera>().HasData(new WorkstationCameraTestSeed().Entities);

            modelBuilder.Entity<SortingStrategyType>().HasData(new SortingStrategyTypeTestSeed().Entities);
            modelBuilder.Entity<SortingStrategyTypeTranslation>().HasData(new SortingStrategyTypeTranslationTestSeed().Entities);

            modelBuilder.Entity<Optimization>().HasData(new OptimizationTestSeed().Entities);

            modelBuilder.Entity<LayoutPlan>().HasData(new LayoutPlanTestSeed().Entities);
            modelBuilder.Entity<CncPlan>().HasData(new CncPlanTestSeed().Entities);
            modelBuilder.Entity<Plan>().HasData(new EdgingPlanTestSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new AssemblyPlanTestSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new SortingPlanTestSeed().Entities);
            modelBuilder.Entity<ManualLaborPlan>().HasData(new PackingPlanTestSeed().Entities);
            modelBuilder.Entity<ProductionProcess>().HasData(new ProductionProcessTestSeed().Entities);
            modelBuilder.Entity<ProcessWorker>().HasData(new ProcessWorkerTestSeed().Entities);


            modelBuilder.Entity<Camera>().HasData(new CameraTestSeed().Entities);
        }

        #endregion SeedInitialDatas
    }
}