using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.FunctionalTests
{
    public static class TestDataSeed
    {
        public static void PopulateTestData(IFPSSalesContext context)
        {
            PopulateDivisionTestData(context);
            PopulateCompanyTestData(context);
            PopulateCountryTestData(context);
            PopulateGroupingCategoryTestData(context);
            PopulateUserTestData(context);
            PopulateMaterialTestData(context);
            PopulateFurnitureUnitData(context);
            PopulateFurnitureComponentData(context);
            PopulateAppointmentData(context);
            PopulateTicketData(context);
            PopulateMessagesData(context);
            PopulateVenuesData(context);
            PopulateOrderData(context);
            PopulateEventtypeData(context);
            PopulateCustomerFurnitureUnitData(context);
            PopulateWebshopFurnitureUnitData(context);
            PopulateWebshopOrderData(context);
            PopulateUserTeamTypeData(context);
            context.SaveChanges();

            FixUserTestDataAfterSeed(context);
        }

        private static void PopulateEventtypeData(IFPSSalesContext context)
        {
            var type1 = new EventType(EventTypeEnum.NewAppointment) { Id = 10000 };
            type1.AddTranslation(new EventTypeTranslation(1, "Új időpont", "A felhasználó értesítést kap az új események létrehozásáról", LanguageTypeEnum.HU) { Id = 10000 });
            context.EventTypes.Add(type1);

            var type2 = new EventType(EventTypeEnum.AppointmentReminder) { Id = 10001 };
            type2.AddTranslation(new EventTypeTranslation(2, "Esemény emlékezetető", "A felhasználó értesítést kap a közelgő eseményekről", LanguageTypeEnum.HU) { Id = 10001 });
            context.EventTypes.Add(type2);

            var type3 = new EventType(EventTypeEnum.ChangedOrderState) { Id = 10002 };
            type3.AddTranslation(new EventTypeTranslation(3, "Állapot változás", "A felhasználó értesítést kap a hozzá tartozó megrendelés állapotváltozásáról", LanguageTypeEnum.HU) { Id = 10002 });
            context.EventTypes.Add(type3);

            var type4 = new EventType(EventTypeEnum.NewFilesUploaded) { Id = 10003 };
            type4.AddTranslation(new EventTypeTranslation(4, "Új dokumentumok", "A felhasználó értesítést kap, amennyiben számára új dokumentum lett feltöltve", LanguageTypeEnum.HU) { Id = 10003 });
            context.EventTypes.Add(type4);

            var type5 = new EventType(EventTypeEnum.NewMessages) { Id = 10004 };
            type5.AddTranslation(new EventTypeTranslation(5, "Új üzenet", "A felhasználó értesítést kap, amennyiben új üzenete érkezett", LanguageTypeEnum.HU) { Id = 10004 });
            context.EventTypes.Add(type5);

            var type6 = new EventType(EventTypeEnum.OrderEvaluation) { Id = 10005 };
            type6.AddTranslation(new EventTypeTranslation(6, "Értékelés", "A felhasználó értesítést kap, amennyiben közeleg a megrendelésének az értékelési határideje", LanguageTypeEnum.HU) { Id = 10005 });
            context.EventTypes.Add(type6);
        }

        private static void PopulateCountryTestData(IFPSSalesContext context)
        {
            var country1 = new Country("HU") { Id = 10000 };
            country1.AddTranslation(new CountryTranslation(1, "Magyarország", LanguageTypeEnum.HU) { Id = 10000 });
            context.Countries.Add(country1);

            var country2 = new Country("SK") { Id = 10001 };
            country2.AddTranslation(new CountryTranslation(2, "Szlovákia", LanguageTypeEnum.HU) { Id = 10001 });
            context.Countries.Add(country2);
        }

        private static void PopulateDivisionTestData(IFPSSalesContext context)
        {
            context.Divisons.Add(new Division(DivisionTypeEnum.Admin) { Id = 10001 });
            context.DivisonTranslations.Add(new DivisionTranslation(10001, "Admin", "Admin division", LanguageTypeEnum.EN) { Id = 10001 });
            context.DivisonTranslations.Add(new DivisionTranslation(10001, "Admin", "Admin szerepkör", LanguageTypeEnum.HU) { Id = 10002 });

            context.Divisons.Add(new Division(DivisionTypeEnum.Sales) { Id = 10002 });
            context.DivisonTranslations.Add(new DivisionTranslation(10002, "Sales", "Sales division", LanguageTypeEnum.EN) { Id = 10003 });
            context.DivisonTranslations.Add(new DivisionTranslation(10002, "Sales", "Sales szerepkör", LanguageTypeEnum.HU) { Id = 10004 });

            context.Divisons.Add(new Division(DivisionTypeEnum.Partner) { Id = 10003 });
            context.DivisonTranslations.Add(new DivisionTranslation(10003, "Partner", "Partner division", LanguageTypeEnum.EN) { Id = 10005 });
            context.DivisonTranslations.Add(new DivisionTranslation(10003, "Partner", "Partner szerepkör", LanguageTypeEnum.HU) { Id = 10006 });

            context.Divisons.Add(new Division(DivisionTypeEnum.Financial) { Id = 10004 });
            context.DivisonTranslations.Add(new DivisionTranslation(10004, "Financial", "Financial division", LanguageTypeEnum.EN) { Id = 10007 });
            context.DivisonTranslations.Add(new DivisionTranslation(10004, "Pénzügy", "Pénzügy szerepkör", LanguageTypeEnum.HU) { Id = 10008 });

            context.Divisons.Add(new Division(DivisionTypeEnum.Customer) { Id = 10005 });
            context.DivisonTranslations.Add(new DivisionTranslation(10005, "Customer", "Customer division", LanguageTypeEnum.EN) { Id = 10009 });
            context.DivisonTranslations.Add(new DivisionTranslation(10005, "Megrendelő", "Megrendelői szerepkör", LanguageTypeEnum.HU) { Id = 10010 });
        }

        private static void PopulateCompanyTestData(IFPSSalesContext context)
        {
            context.Companies.Add(new Company("Test EN-CO Software", 1)
            {
                Id = 10000,
                CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 6 },
                CurrentVersionId = 10000
            });
            context.Companies.Add(new Company("Test Bosch", 1)
            { Id = 10001, CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 7 }, CurrentVersionId = 10001 });

            context.Roles.Add(new Role("Test Expert admin", 10001) { Id = 10000 });
            context.Roles.Add(new Role("Test Customer", 10005) { Id = 10001 });

            var encoContact = new User("enco2@enco.hu") { Id = 10000 };
            encoContact.AddVersion(new UserData("Kellem Etlen", "06301112222", Clock.Now,
                new Address(6500, "Cegléd", "Kossuth tér 3.", 1))
            { Id = 12000 });
            encoContact.AddVersion(new UserData("Kelemen Elemér", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1))
            { Id = 10000 });
            context.Users.Add(encoContact);

            var encoEmployee = new User("le@enco.hu") { Id = 10030, CompanyId = 10000 };
            encoEmployee.AddVersion(new UserData("Lapos Elemér", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 1.", 1))
            { Id = 10030 });
            context.Users.Add(encoEmployee);

            context.CompanyData.Add(new CompanyData("1111", "1111",
                new Address(6344, "Hajós", "Petőfi utca 52.", 1), 10000, Clock.Now)
            { Id = 10000, ContactPersonId = 10000 });
            context.CompanyData.Add(new CompanyData("1111", "1111",
                new Address(1117, "Budapest", "Bocskai út 77-79.", 1), null, Clock.Now)
            { Id = 10001 });
        }

        private static void PopulateGroupingCategoryTestData(IFPSSalesContext context)
        {
            var materialsCategory = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(materialsCategory.Id, "Teszt alapanyagok", LanguageTypeEnum.HU) { Id = 10002 });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(materialsCategory.Id, "Materials", LanguageTypeEnum.EN) { Id = 10003 });
            context.GroupingCategories.Add(materialsCategory);

            var divisionsCategory = new GroupingCategory(GroupingCategoryEnum.DivisionType) { Id = 10001 };
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(divisionsCategory.Id, "Teszt Jogosultsági körök", LanguageTypeEnum.HU) { Id = 10004 });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(divisionsCategory.Id, "Divisons", LanguageTypeEnum.EN) { Id = 10005 });
            context.GroupingCategories.Add(divisionsCategory);

            var appliancesCategory = new GroupingCategory(materialsCategory) { Id = 10002 };
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(appliancesCategory.Id, "Konyhagépek", LanguageTypeEnum.HU) { Id = 10006 });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(appliancesCategory.Id, "Appliances", LanguageTypeEnum.EN) { Id = 10007 });
            context.GroupingCategories.Add(appliancesCategory);

            var washingMachines = new GroupingCategory(appliancesCategory) { Id = 10003 };
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(washingMachines.Id, "Mosogatógépek", LanguageTypeEnum.HU) { Id = 10008 });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(washingMachines.Id, "Washing machines", LanguageTypeEnum.EN) { Id = 10009 });
            context.GroupingCategories.Add(washingMachines);

            var microwaveMachines = new GroupingCategory(appliancesCategory) { Id = 10004 };
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(microwaveMachines.Id, "Mikrohúllámú sütők", LanguageTypeEnum.HU) { Id = 10010 });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation(microwaveMachines.Id, "Microwave ovens", LanguageTypeEnum.EN) { Id = 10011 });
            context.GroupingCategories.Add(microwaveMachines);
        }

        private static void PopulateUserTestData(IFPSSalesContext context)
        {
            context.Claims.Add(new Claim(ClaimPolicyEnum.GetTestUsers, divisionId: 1) { Id = 10000 });
            context.Claims.Add(new Claim(ClaimPolicyEnum.UpdateTestUsers, divisionId: 1) { Id = 10001 });
            context.Claims.Add(new Claim(ClaimPolicyEnum.DeleteTestUsers, divisionId: 2) { Id = 10002 });
            context.Claims.Add(new Claim(ClaimPolicyEnum.GetTestFurnitureUnits, divisionId: 2) { Id = 10003 });
            context.Claims.Add(new Claim(ClaimPolicyEnum.UpdateTestFurnitureUnits, divisionId: 10005) { Id = 10004 });
            context.Claims.Add(new Claim(ClaimPolicyEnum.DeleteTestFurnitureUnits, divisionId: 10005) { Id = 10005 });

            context.Roles.Add(new Role("Teszt Very Expert admin", divisionId: 10001) { Id = 10002 });
            context.Roles.Add(new Role("Teszt Expert Customer", divisionId: 10005) { Id = 10003 });
            context.Roles.Add(new Role("Teszt Expert SalesPerson", divisionId: 10002) { Id = 10004 });
            context.Roles.Add(new Role("Teszt Super God", divisionId: 10001) { Id = 10005 });
            context.Roles.Add(new Role("Teszt Empty Role", divisionId: 10003) { Id = 10006 });
            context.Roles.Add(new Role("Teszt Very Expert admin 2", divisionId: 10001) { Id = 10007 });
            context.Roles.Add(new Role("Teszt Expert Customer 2", divisionId: 10005) { Id = 10008 });
            context.Roles.Add(new Role("Teszt Expert SalesPerson 2", divisionId: 10002) { Id = 10009 });
            context.Roles.Add(new Role("Teszt Partner", divisionId: 10005) { Id = 10010 });


            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10002, claimId: 10000) { Id = 10000 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10003, claimId: 10004) { Id = 10001 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10004, claimId: 10002) { Id = 10002 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10005, claimId: 10001) { Id = 10003 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10005, claimId: 10003) { Id = 10004 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10005, claimId: 10005) { Id = 10005 });
            context.DefaultRoleClaims.Add(new DefaultRoleClaim(roleId: 10010, claimId: 10002) { Id = 10006 });


            var companyNokia = new Company("Teszt Nokia", 1) { Id = 10002, CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10000 } };
            context.Companies.Add(companyNokia);
            var companyNokiaData = new CompanyData("1111", "1111",
                new Address(6344, "Hajós", "Kossuth utca 52.", 1), null, Clock.Now)
            { Id = 10002, CoreId = 10002 };
            context.CompanyData.Add(companyNokiaData);
            context.Companies.Add(new Company("Teszt Bakosfa", 1) { Id = 10003, CompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10001 } });
            context.CompanyData.Add(new CompanyData("1111", "1111",
                new Address(1117, "Budapest", "Szerémi út 30.", 1), null, Clock.Now)
            { Id = 10003, CoreId = 10003 });

            //GetUsers
            var weUser = new User("we@enco.test.hu") { Id = 10001, CreationTime = new DateTime(2019, 03, 01), CompanyId = 10002 };
            weUser.AddVersion(new UserData("Wincs Eszter", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(weUser);
            context.UserRoles.Add(new UserRole(10001, 10002));

            var eeUser = new User("ee@enco.test.hu") { Id = 10002, CreationTime = new DateTime(2019, 05, 10), CompanyId = 10003 };
            eeUser.AddVersion(new UserData("Ebéd Elek", "06301112222", Clock.Now,
                new Address(5000, "Szolnok", "Kossuth tér 3.", 1)));
            context.Users.Add(eeUser);
            context.UserRoles.Add(new UserRole(10002, 10003));

            var elUser = new User("el@enco.test.hu") { Id = 10003, CreationTime = new DateTime(2019, 06, 05) };
            elUser.AddVersion(new UserData("Eszte Lenke", "06309876543", Clock.Now,
                new Address(1117, "Budapest", "Irinyi József utca 42.", 1)));
            context.Users.Add(elUser);

            //DeactivateUser
            var mkUser = new User("mk@enco.hu") { Id = 10004, CreationTime = new DateTime(2019, 06, 05) };
            mkUser.AddVersion(new UserData("Mikorka Kálmán", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(mkUser);

            //ActivateUser
            var szszUser = new User("szsz@enco.hu") { Id = 10005, CreationTime = new DateTime(2019, 06, 05), IsActive = false };
            szszUser.AddVersion(new UserData("Szikla Szilárd", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(szszUser);

            //UserDetails
            var neUser = new User("ne@enco.hu") { Id = 10006, CreationTime = new DateTime(2019, 06, 05), CompanyId = 10003 };
            neUser.AddVersion(new UserData("Nemer Eszti", "06701234567", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser);
            context.UserRoles.Add(new UserRole(10006, 10003));
            context.UserRoles.Add(new UserRole(10006, 10004));
            context.UserClaims.Add(new UserClaim(10006, 10000));
            context.UserClaims.Add(new UserClaim(10006, 10001));
            context.UserClaims.Add(new UserClaim(10006, 10002));
            context.UserClaims.Add(new UserClaim(10006, 10003));
            context.UserClaims.Add(new UserClaim(10006, 10004));
            context.UserClaims.Add(new UserClaim(10006, 10005));

            var neSalesPerson = new SalesPerson(10006, Clock.Now)
            {
                Id = 10000,
                SupervisorId = 10001,
                MinDiscount = 1,
                MaxDiscount = 10,
            };
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(1, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 12, 0, 0), neSalesPerson.Id) { Id = 10001 });
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(1, new DateTime(0001, 01, 01, 13, 0, 0), new DateTime(0001, 01, 01, 16, 0, 0), neSalesPerson.Id) { Id = 10002 });
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(2, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 14, 0, 0), neSalesPerson.Id) { Id = 10003 });
            neSalesPerson.AddOffices(new List<int>() { 1 });
            context.SalesPersons.Add(neSalesPerson);

            var userTeamType = new UserTeamType(UserTeamTypeEnum.ShippingGroup) { Id = 10000 };
            var userTeamTypeTranslationEN = new UserTeamTypeTranslation("Shippers", userTeamType.Id, LanguageTypeEnum.EN) { Id = 10000 };
            var userTeamTypeTranslationHU = new UserTeamTypeTranslation("Kiszállítók", userTeamType.Id, LanguageTypeEnum.HU) { Id = 10001 };

            context.UserTeamTypeTranslations.Add(userTeamTypeTranslationEN);
            context.UserTeamTypeTranslations.Add(userTeamTypeTranslationHU);
            context.UserTeamTypes.Add(userTeamType);

            //userTeamType.AddTranslation(userTeamTypeTranslationEN);
            //userTeamType.AddTranslation(userTeamTypeTranslationHU);

            var utUser = new User("technikaluser@enco.hu") { Id = 10100, CreationTime = new DateTime(2019, 06, 05), IsTechnicalAccount = true };
            utUser.AddVersion(new UserData("Bakosfa User Team", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(utUser);
            var userTeam = new UserTeam(10003, 10100, 10006, userTeamType.Id) { UserTeamType = userTeamType }; // TODO UserTeamTypeId, negyedik az
            context.UserTeams.Add(userTeam);
            var neCustomer = new Customer(10006, Clock.Now)
            {
                Id = 10000,
                NotificationType = NotificationTypeFlag.Email | NotificationTypeFlag.SMS
            };
            neCustomer.AddNotificationModes(new List<int> { 1, 2, 3, 4 });
            context.Customers.Add(neCustomer);
            context.Employees.Add(new Employee(10006) { Id = 10000 });

            var toUser = new User("to@enco.hu") { Id = 10007, CreationTime = new DateTime(2019, 06, 05) };
            toUser.AddVersion(new UserData("Tök Ödön", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(toUser);
            context.UserClaims.Add(new UserClaim(10007, 10002));
            context.UserRoles.Add(new UserRole(10007, 10004));
            context.SalesPersons.Add(new SalesPerson(10007, Clock.Now)
            {
                Id = 10001,
                SupervisorId = null,
            });

            var ddUser = new User("dd@enco.hu") { Id = 10008, CreationTime = new DateTime(2019, 06, 05) };
            ddUser.AddVersion(new UserData("Dia Dóra", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(ddUser);
            context.UserClaims.Add(new UserClaim(10008, 10002));
            context.UserRoles.Add(new UserRole(10008, 10004));
            context.SalesPersons.Add(new SalesPerson(10008, Clock.Now)
            {
                Id = 10002,
                SupervisorId = 10000,
            });

            var hzUser = new User("hz@enco.hu") { Id = 10009, CreationTime = new DateTime(2019, 06, 05) };
            hzUser.AddVersion(new UserData("Hú Zóra", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(hzUser);
            context.UserClaims.Add(new UserClaim(10009, 10002));
            context.UserRoles.Add(new UserRole(10009, 10004));
            context.SalesPersons.Add(new SalesPerson(10009, Clock.Now)
            {
                Id = 10003,
                SupervisorId = 10000,
            });

            //UserUpdate
            var neUser2 = new User("ne@enco.hu") { Id = 10010, CreationTime = new DateTime(2019, 06, 05), CompanyId = 10003 };
            neUser2.AddVersion(new UserData("Nemer Eszti2", "06701234567", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser2);
            context.UserRoles.Add(new UserRole(10010, 10003));
            context.UserRoles.Add(new UserRole(10010, 10004));
            context.UserClaims.Add(new UserClaim(10010, 10000));
            context.UserClaims.Add(new UserClaim(10010, 10001));
            context.UserClaims.Add(new UserClaim(10010, 10002));
            context.UserClaims.Add(new UserClaim(10010, 10003));
            context.UserClaims.Add(new UserClaim(10010, 10004));
            context.UserClaims.Add(new UserClaim(10010, 10005));

            var neSalesPerson2 = new SalesPerson(10010, Clock.Now)
            {
                Id = 10004,
                SupervisorId = 10005,
                MinDiscount = 1,
                MaxDiscount = 10,
            };
            //neSalesPerson2.AddDefaultTimeTableEntry(1, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 12, 0, 0));

            //context.SalesPersonDateRanges.Add(new SalesPersonDateRange(1, new DateTime(0001, 01, 01, 13, 0, 0), new DateTime(0001, 01, 01, 16, 0, 0), neSalesPerson2.Id) { Id = 10005 });
            //context.SalesPersonDateRanges.Add(new SalesPersonDateRange(2, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 14, 0, 0), neSalesPerson2.Id) { Id = 10006 });
            neSalesPerson2.AddOffices(new List<int>() { 1 });
            context.SalesPersons.Add(neSalesPerson2);

            var neCustomer2 = new Customer(neUser2.Id, Clock.Now)
            {
                Id = 10001,
                NotificationType = NotificationTypeFlag.Email | NotificationTypeFlag.SMS
            };
            neCustomer2.AddNotificationModes(new List<int> { 1, 2, 3, 4 });
            context.Customers.Add(neCustomer2);
            context.Employees.Add(new Employee(neUser2.Id) { Id = 10002 });

            var toUser2 = new User("to@enco.hu") { Id = 10011, CreationTime = new DateTime(2019, 06, 05) };
            toUser2.AddVersion(new UserData("Tök Ödön2", "", Clock.Now,
                 Address.GetEmptyAddress()));
            context.Users.Add(toUser2);
            context.UserClaims.Add(new UserClaim(toUser2.Id, 10002));
            context.UserRoles.Add(new UserRole(toUser2.Id, 10004));
            context.SalesPersons.Add(new SalesPerson(toUser2.Id, Clock.Now)
            {
                Id = 10005,
                SupervisorId = null,
            });

            var paUser = new User("pa@enco.hu") { Id = 10012, CreationTime = new DateTime(2019, 03, 01), CompanyId = 10002 };
            paUser.AddVersion(new UserData("Para Zita", "06301234567", Clock.Now,
                new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(paUser);
            context.UserRoles.Add(new UserRole(paUser.Id, 10006));

            var hmUser = new User("hm@enco.hu") { Id = 10013, CreationTime = new DateTime(2019, 03, 01) };
            hmUser.AddVersion(new UserData("Har Mónika", "", Clock.Now,
                 new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(hmUser);

            var hmUser1 = new User("hm1@enco.hu") { Id = 10014, CreationTime = new DateTime(2019, 03, 01), };
            hmUser1.AddVersion(new UserData("Har Mónika1", "", Clock.Now,
                 new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(hmUser1);
            companyNokiaData.ContactPersonId = hmUser1.Id;

            var hmUser2 = new User("hm2@enco.hu") { Id = 10015, CreationTime = new DateTime(2019, 03, 01), CompanyId = 10003 };
            hmUser2.AddVersion(new UserData("Har Mónika2", "", Clock.Now,
                 new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(hmUser2);

            var hmUser3 = new User("hm3@enco.hu")
            {
                UserName = "hm enco",
                PasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==",
                SecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR",
                EmailConfirmed = true,
                ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),
                Id = 10016,
                CreationTime = new DateTime(2019, 03, 01)
            };
            hmUser3.AddVersion(new UserData("Har Mónika3", "", Clock.Now,
                 new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(hmUser3);

            var neUser3 = new User("ne3@enco.hu") { Id = 10017, CreationTime = new DateTime(2019, 06, 05) };
            neUser3.AddVersion(new UserData("Nemer Eszti3", "", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser3);
            context.UserRoles.Add(new UserRole(neUser3.Id, 10002));
            context.UserRoles.Add(new UserRole(neUser3.Id, 10003));
            context.UserRoles.Add(new UserRole(neUser3.Id, 10004));
            context.UserClaims.Add(new UserClaim(neUser3.Id, 10000));
            context.UserClaims.Add(new UserClaim(neUser3.Id, 10002));
            context.UserClaims.Add(new UserClaim(neUser3.Id, 10004));
            var neSalesPerson3 = new SalesPerson(neUser3.Id, Clock.Now)
            {
                Id = 10006,
                MinDiscount = 1,
                MaxDiscount = 10,
            };
            context.SalesPersons.Add(neSalesPerson3);
            var neCustomer3 = new Customer(neUser3.Id, Clock.Now)
            {
                Id = 10006,
                NotificationType = NotificationTypeFlag.Email | NotificationTypeFlag.SMS
            };
            context.Customers.Add(neCustomer3);
            context.Employees.Add(new Employee(neUser3.Id) { Id = 10006 });

            var neUser4 = new User("ne4@enco.hu") { Id = 10018, CreationTime = new DateTime(2019, 06, 05) };
            neUser4.AddVersion(new UserData("Nemer Eszti4", "", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser4);
            context.UserClaims.Add(new UserClaim(neUser4.Id, 10002));
            var neSalesPerson4 = new SalesPerson(neUser4.Id, Clock.Now)
            {
                Id = 10007,
                SupervisorId = 10006,
                MinDiscount = 1,
                MaxDiscount = 10,
            };
            neSalesPerson4.AddDefaultTimeTableEntry(1, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 12, 0, 0));
            neSalesPerson4.AddDefaultTimeTableEntry(1, new DateTime(0001, 01, 01, 13, 0, 0), new DateTime(0001, 01, 01, 16, 0, 0));
            neSalesPerson4.AddDefaultTimeTableEntry(2, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 14, 0, 0));
            neSalesPerson4.AddDefaultTimeTableEntry(3, new DateTime(0001, 01, 01, 7, 0, 0), new DateTime(0001, 01, 01, 11, 0, 0));
            neSalesPerson4.AddDefaultTimeTableEntry(4, new DateTime(0001, 01, 01, 14, 0, 0), new DateTime(0001, 01, 01, 17, 0, 0));
            neSalesPerson4.AddDefaultTimeTableEntry(5, new DateTime(0001, 01, 01, 8, 0, 0), new DateTime(0001, 01, 01, 13, 0, 0));
            neSalesPerson4.AddOffices(new List<int>() { 1 });
            context.SalesPersons.Add(neSalesPerson4);

            var neUser5 = new User("ne5@enco.hu") { Id = 10019, CreationTime = new DateTime(2019, 06, 05) };
            neUser5.AddVersion(new UserData("Nemer Eszti5", "", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser5);
            context.UserClaims.Add(new UserClaim(neUser5.Id, 10002));
            var neSalesPerson5 = new SalesPerson(neUser5.Id, Clock.Now)
            {
                Id = 10008,
                SupervisorId = 10006,
                MinDiscount = 1,
                MaxDiscount = 10,
            };
            neSalesPerson5.AddOffices(new List<int>() { 1 });
            context.SalesPersons.Add(neSalesPerson5);
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(1, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 12, 0, 0), neSalesPerson5.Id) { Id = 10010 });    //neSalesPerson3
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(1, new DateTime(0001, 01, 01, 13, 0, 0), new DateTime(0001, 01, 01, 16, 0, 0), neSalesPerson5.Id) { Id = 10011 });   //neSalesPerson4, Id = 10011
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(2, new DateTime(0001, 01, 01, 9, 0, 0), new DateTime(0001, 01, 01, 14, 0, 0), neSalesPerson5.Id) { Id = 10012 });    // Id = 10012
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(3, new DateTime(0001, 01, 01, 7, 0, 0), new DateTime(0001, 01, 01, 11, 0, 0), neSalesPerson5.Id) { Id = 10013 });    // Id = 10096
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(4, new DateTime(0001, 01, 01, 14, 0, 0), new DateTime(0001, 01, 01, 17, 0, 0), neSalesPerson5.Id) { Id = 10014 }); //10098
            context.SalesPersonDateRanges.Add(new SalesPersonDateRange(5, new DateTime(0001, 01, 01, 8, 0, 0), new DateTime(0001, 01, 01, 13, 0, 0), neSalesPerson5.Id) { Id = 10015 }); // 10015

            var hmUser4 = new User("hm4@enco.hu") { Id = 10020, CreationTime = new DateTime(2019, 03, 01) };
            hmUser4.AddVersion(new UserData("Har Mónika4", "", Clock.Now,
                 new Address(1117, "Budapest", "Seholse utca 1.", 1)));
            context.Users.Add(hmUser4);

            var neUser6 = new User("ne6@enco.hu") { Id = 10021, CreationTime = new DateTime(2019, 06, 05) };
            neUser6.AddVersion(new UserData("Nemer Eszti6", "", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser6);
            context.UserClaims.Add(new UserClaim(neUser6.Id, 10004));
            var neCustomer6 = new Customer(neUser6.Id, Clock.Now)
            {
                Id = 10009,
                NotificationType = NotificationTypeFlag.Email | NotificationTypeFlag.SMS
            };
            neCustomer6.AddNotificationModes(new List<int> { 1, 2, 3, 4 });
            context.Customers.Add(neCustomer6);

            var neUser7 = new User("ne7@enco.hu") { Id = 10022, CreationTime = new DateTime(2019, 06, 05) };
            neUser7.AddVersion(new UserData("Nemer Eszti7", "", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser7);


        }

        private static void FixUserTestDataAfterSeed(IFPSSalesContext context)
        {
            Dictionary<int, DateTime> idAddedOnPairs = new Dictionary<int, DateTime>()
            {
                {10001,new DateTime(2019, 03, 01)},
                {10002,new DateTime(2019, 05, 10)},
                {10003,new DateTime(2019, 06, 05)},
            };
            var users = context.Users.Where(u => idAddedOnPairs.Keys.Contains(u.Id)).ToList();

            context.Users.UpdateRange(users);
            foreach (var user in users)
            {
                user.CreationTime = idAddedOnPairs[user.Id];
            }
            context.SaveChanges();
        }

        private static void PopulateMaterialTestData(IFPSSalesContext context)
        {
            context.Images.Add(new Image("accessory.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_accessory.jpg" });
            context.Images.Add(new Image("appliance.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("2f768741-e02c-4f36-a11b-8c193197ddcc"), ThumbnailName = "thumbnail_appliance.jpg" });
            context.Images.Add(new Image("decorboard.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("cd42cda2-6d48-4a79-8ec4-1b46486ef203"), ThumbnailName = "thumbnail_decorboard.jpg" });
            context.Images.Add(new Image("worktop.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da"), ThumbnailName = "thumbnail_worktop.jpg" });
            context.Images.Add(new Image("foil.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51"), ThumbnailName = "thumbnail_foil.jpg" });

            context.MaterialPrices.Add(new MaterialPrice() { Id = 10000, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10001, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10002, Price = new Price(10, 1) });
            context.Materials.Add(new AccessoryMaterial(true, true, "HA123457", 10) { Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"), Description = "Description", CategoryId = 10000, ImageId = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), CurrentPriceId = 10000 });
            context.Materials.Add(new AccessoryMaterial(true, true, "HA123458", 10) { Id = new Guid("92aa6e7c-a524-45ec-8d67-5bcfa2393f21"), Description = "Description", CategoryId = 10000, ImageId = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), CurrentPriceId = 10001 });
            context.Materials.Add(new AccessoryMaterial(true, true, "HA123459", 10) { Id = new Guid("91a00f8b-f350-4778-abd3-2123a707693f"), Description = "Description", CategoryId = 10000, ImageId = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), CurrentPriceId = 10002 });

            context.MaterialPrices.Add(new MaterialPrice() { Id = 10003, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10004, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10005, Price = new Price(10, 1) });
            context.Materials.Add(new ApplianceMaterial("HA124457") { Id = new Guid("c81a3824-1685-414e-9641-e3c916c27302"), Description = "Description", CategoryId = 10000, SellPrice = new Price(10, 1), BrandId = 10000, HanaCode = "Test", ImageId = new Guid("2f768741-e02c-4f36-a11b-8c193197ddcc"), CurrentPriceId = 10003 });
            context.Materials.Add(new ApplianceMaterial("HA124458") { Id = new Guid("012e2fd4-12de-4148-9187-90c321daac16"), Description = "Description", CategoryId = 10000, SellPrice = new Price(10, 1), BrandId = 10000, HanaCode = "Test", ImageId = new Guid("2f768741-e02c-4f36-a11b-8c193197ddcc"), CurrentPriceId = 10004 });
            context.Materials.Add(new ApplianceMaterial("HA124459") { Id = new Guid("bf79421c-a97b-4da2-8191-9cd19e328aea"), Description = "Description", CategoryId = 10000, SellPrice = new Price(10, 1), BrandId = 10000, HanaCode = "Test", ImageId = new Guid("2f768741-e02c-4f36-a11b-8c193197ddcc"), CurrentPriceId = 10005 });

            context.MaterialPrices.Add(new MaterialPrice() { Id = 10006, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10007, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10008, Price = new Price(10, 1) });
            context.Materials.Add(new DecorBoardMaterial("HA125457", 10) { Id = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("cd42cda2-6d48-4a79-8ec4-1b46486ef203"), CurrentPriceId = 10006 });
            context.Materials.Add(new DecorBoardMaterial("HA125458", 10) { Id = new Guid("d0022a4e-3339-4ee7-8988-f4af75d22d1f"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("cd42cda2-6d48-4a79-8ec4-1b46486ef203"), CurrentPriceId = 10007 });
            context.Materials.Add(new DecorBoardMaterial("HA125459", 10) { Id = new Guid("5e05089f-1dac-4883-9f62-6d16bf682dcd"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("cd42cda2-6d48-4a79-8ec4-1b46486ef203"), CurrentPriceId = 10008 });

            context.MaterialPrices.Add(new MaterialPrice() { Id = 10009, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10010, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10011, Price = new Price(10, 1) });
            context.Materials.Add(new WorktopBoardMaterial("HA126457", 10) { Id = new Guid("b33fe42e-0f53-4e2e-acb2-68843b1dacee"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da"), CurrentPriceId = 10009 });
            context.Materials.Add(new WorktopBoardMaterial("HA126458", 10) { Id = new Guid("31a21d26-26a0-46a8-8092-ccb06bba12f1"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da"), CurrentPriceId = 10010 });
            context.Materials.Add(new WorktopBoardMaterial("HA126459", 10) { Id = new Guid("b8675809-1172-48de-8778-17710493eca6"), Description = "Description", CategoryId = 10000, Dimension = new Dimension(2, 2, 2), ImageId = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da"), CurrentPriceId = 10011 });

            context.MaterialPrices.Add(new MaterialPrice() { Id = 10012, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10013, Price = new Price(10, 1) });
            context.MaterialPrices.Add(new MaterialPrice() { Id = 10014, Price = new Price(10, 1) });
            context.Materials.Add(new FoilMaterial("HA127457", 10) { Id = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), Description = "Description", Thickness = 2, ImageId = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51"), CurrentPriceId = 10012 });
            context.Materials.Add(new FoilMaterial("HA127458", 10) { Id = new Guid("4b2334b4-4436-4085-bcd7-c5cb939c7e97"), Description = "Description", Thickness = 2, ImageId = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51"), CurrentPriceId = 10013 });
            context.Materials.Add(new FoilMaterial("HA127459", 10) { Id = new Guid("9e0a2348-f1e2-4d09-9594-37c0e63c4fed"), Description = "Description", Thickness = 2, ImageId = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51"), CurrentPriceId = 10014 });
        }

        private static void PopulateFurnitureUnitData(IFPSSalesContext context)
        {
            var currency = new Currency("HUF") { Id = 10870 };
            context.Currencies.Add(currency);
            context.Images.Add(new Image("furnitureunit.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), ThumbnailName = "thumbnail_furnitureunit.jpg" });

            var type = new FurnitureUnitType(FurnitureUnitTypeEnum.Top) { Id = 10200 };
            context.FurnitureUnitTypes.Add(type);
            var translationEN = new FurnitureUnitTypeTranslation(type.Id, "Top", LanguageTypeEnum.EN) { Id = 10290 };
            var translationHU = new FurnitureUnitTypeTranslation(type.Id, "Felső", LanguageTypeEnum.HU) { Id = 10291 };
            context.FurnitureUnitTypeTranslations.Add(translationEN);
            context.FurnitureUnitTypeTranslations.Add(translationHU);

            context.FurnitureUnitPrices.Add(new FurnitureUnitPrice() { Id = 10000, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id) });
            context.FurnitureUnitPrices.Add(new FurnitureUnitPrice() { Id = 10001, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id) });
            context.FurnitureUnitPrices.Add(new FurnitureUnitPrice() { Id = 10002, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id) });
            context.FurnitureUnitPrices.Add(new FurnitureUnitPrice() { Id = 10003, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id) });

            context.FurnitureUnits.Add(new FurnitureUnit("HA123457", 2, 2, 2) { Id = new Guid("0B7C06C0-43F5-4E45-8C7F-68E0F1B41952"), Description = "Description", CategoryId = 10000, ImageId = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), CurrentPriceId = 10000, FurnitureUnitTypeId = type.Id, FurnitureUnitType = type });
            context.FurnitureUnits.Add(new FurnitureUnit("HA123458", 2, 2, 2) { Id = new Guid("D69CC81F-B318-4C06-99F5-991BB9386C91"), Description = "Description", CategoryId = 10000, ImageId = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), CurrentPriceId = 10001, FurnitureUnitTypeId = type.Id, FurnitureUnitType = type });
            context.FurnitureUnits.Add(new FurnitureUnit("HA1234158", 2, 2, 2) { Id = new Guid("20DD26B8-B89F-4471-B5CE-BE1103B724DC"), Description = "Description", CategoryId = 10000, ImageId = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), CurrentPriceId = 10002, FurnitureUnitTypeId = type.Id, FurnitureUnitType = type });
            context.FurnitureUnits.Add(new FurnitureUnit("HA1234258", 2, 2, 2) { Id = new Guid("3C14569A-BD79-49AA-B9AB-9A7C1482E8A1"), Description = "Description", CategoryId = 10000, ImageId = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), CurrentPriceId = 10003, FurnitureUnitTypeId = type.Id, FurnitureUnitType = type });
            context.FurnitureUnits.Add(new FurnitureUnit("Fairy Webshop Unit2", 80, 150, 25) { Id = new Guid("B50D5D13-3885-4584-82C7-C267D848B793"), CategoryId = 10002, ImageId = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E195"), Description = "Kücheregal", CurrentPriceId = 10002, FurnitureUnitTypeId = type.Id, FurnitureUnitType = type });
        }

        private static void PopulateFurnitureComponentData(IFPSSalesContext context)
        {
            context.Images.Add(new Image("furniturecomponent.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("84d41589-bfa5-42b9-93e2-7a2954cb199d") });
            context.Images.Add(new Image("accessorycomponent.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("10d55325-198e-4eb4-b923-67a83a5198b0") });

            context.FurnitureComponents.Add(new FurnitureComponent("COMP", 1, 1, 1) { Id = new Guid("a18b97d4-fe21-4f9e-ac3f-9888b8fb90a8"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"), BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), ImageId = new Guid("84d41589-bfa5-42b9-93e2-7a2954cb199d"), FurnitureUnitId = new Guid("3c14569a-bd79-49aa-b9ab-9a7c1482e8a1") });
            context.FurnitureComponents.Add(new FurnitureComponent("COMP", 1, 1, 1) { Id = new Guid("61ce580c-e65b-4282-a1d5-fca69c43fb0d"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"), BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), ImageId = new Guid("84d41589-bfa5-42b9-93e2-7a2954cb199d"), FurnitureUnitId = new Guid("3c14569a-bd79-49aa-b9ab-9a7c1482e8a1") });
            context.FurnitureComponents.Add(new FurnitureComponent("COMP", 1, 1, 1) { Id = new Guid("c371c8b1-ad09-441b-adc4-dfb0ad69a1d3"), Type = FurnitureComponentTypeEnum.Front, BoardMaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"), BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"), ImageId = new Guid("84d41589-bfa5-42b9-93e2-7a2954cb199d"), FurnitureUnitId = new Guid("3c14569a-bd79-49aa-b9ab-9a7c1482e8a1") });

            context.AccessoryMaterialFurnitureUnits.Add(new AccessoryMaterialFurnitureUnit("AMFU", 5) { Id = 10003, FurnitureUnitId = new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"), Accessory = new AccessoryMaterial(true, true, "HA123457", 10) { Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"), Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") } });
            context.AccessoryMaterialFurnitureUnits.Add(new AccessoryMaterialFurnitureUnit("AMFU", 5) { Id = 10004, FurnitureUnitId = new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"), Accessory = new AccessoryMaterial(true, true, "HA123457", 10) { Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"), Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") } });
            context.AccessoryMaterialFurnitureUnits.Add(new AccessoryMaterialFurnitureUnit("AMFU", 5) { Id = 10005, FurnitureUnitId = new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"), Accessory = new AccessoryMaterial(true, true, "HA123457", 10) { Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"), Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") } });
        }

        private static void PopulateAppointmentData(IFPSSalesContext context)
        {
            context.Appointments.Add(new Appointment(new DateRange(new DateTime(2019, 06, 05), new DateTime(2019, 06, 06)), 10000, "Hétfő reggel megbeszélés") { Id = 10000, Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1), CustomerId = 10000, CategoryId = 10000 });
            context.Appointments.Add(new Appointment(new DateRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)), 10000, "Hétfő reggel megbeszélés") { Id = 10001, Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1), CustomerId = 10000, CategoryId = 10000 });
            context.Appointments.Add(new Appointment(new DateRange(new DateTime(2019, 06, 07), new DateTime(2019, 06, 08)), 10000, "Péntek reggel megbeszélés") { Id = 10002, Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1), CustomerId = 10000, CategoryId = 10000 });
        }

        private static void PopulateTicketData(IFPSSalesContext context)
        {
            var user = new User("enco7@enco.hu") { Id = 10101 };
            var customer = new Customer(user.Id, new DateTime(2019, 12, 31)) { Id = 10240, User = user };
            user.AddVersion(new UserData("enco", "00000", Clock.Now)
            {
                Id = 10100,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });
            context.Users.Add(user);

            var order = new Order("Teszt order", user.Id, 10000, Clock.Now, new Price(0.0, 1))
            {
                Id = new Guid("FC2FFEB7-58FB-4DBE-AC8C-85918E940B01"),
                Customer = customer,
                CustomerId = customer.Id,
                WorkingNumberSerial = 11111,
                WorkingNumberYear = 3000,
                ShippingAddress = new Address(6344, "Hajós", "Petőfi utca 52.", 1)
            };
            order.FirstPayment = new OrderPrice() { Id = 10000, Price = new Price(10, 1) };
            order.SecondPayment = new OrderPrice() { Id = 10001, Price = new Price(10, 1) };
            //var orderState = new OrderState(OrderStateEnum.WaitingForOffer) { Id = 10000 };
            //orderState.AddTranslation(new OrderStateTranslation(coreId: 3, "WaitingForOffer", LanguageTypeEnum.EN) { Id = 10000 });
            order.AddTicket(new Ticket(3, user.Id, 2) { Id = 10000 });
            context.Orders.Add(order);
            //context.OrderStates.Add(orderState);
        }

        private static void PopulateMessagesData(IFPSSalesContext context)
        {
            context.Messages.Add(new Message(10001, 10001, "First message") { Id = 10001 });
            context.Messages.Add(new Message(10001, 10001, "Second message") { Id = 10002 });
            context.Messages.Add(new Message(10001, 10001, "Third message") { Id = 10003 });
            context.Messages.Add(new Message(10001, 10001, "Fourth message") { Id = 10004 });
            context.Messages.Add(new Message(10001, 10001, "Fifth message") { Id = 10005 });
            context.Messages.Add(new Message(10002, 10002, "First message") { Id = 10006 });

            context.ParticipantMessages.Add(new ParticipantMessage(10001, 10003) { Id = 10001 });
            context.ParticipantMessages.Add(new ParticipantMessage(10002, 10003) { Id = 10002 });
            context.ParticipantMessages.Add(new ParticipantMessage(10003, 10003) { Id = 10003 });
            context.ParticipantMessages.Add(new ParticipantMessage(10004, 10003) { Id = 10004 });
            context.ParticipantMessages.Add(new ParticipantMessage(10005, 10003) { Id = 10005 });
            context.ParticipantMessages.Add(new ParticipantMessage(10006, 10003) { Id = 10006 });

            context.MessageChannels.Add(new MessageChannel(new Guid("fc2ffeb7-58fb-4dbe-ac8c-85918e940b01")) { Id = 10001 });
            context.MessageChannels.Add(new MessageChannel(new Guid("fc2ffeb7-58fb-4dbe-ac8c-85918e940b01")) { Id = 10002 });

            context.MessageChannelParticipants.Add(new MessageChannelParticipant(10001, 10000) { Id = 10001 });
            context.MessageChannelParticipants.Add(new MessageChannelParticipant(10001, 10001) { Id = 10002 });
            context.MessageChannelParticipants.Add(new MessageChannelParticipant(10002, 10000) { Id = 10003 });
            context.MessageChannelParticipants.Add(new MessageChannelParticipant(10002, 10001) { Id = 10004 });
        }

        private static void PopulateVenuesData(IFPSSalesContext context)
        {
#if !DEBUG
            var v = new Venue("Iroda (seed)", "", "test@test.com", 1, null) { Id = 1 };
            v.OfficeAddress = new Address(1113, "Budapest", "H-1113 Budapest, Bocskai út 77-79. B. épület 2. em", 1);
            context.Venues.Add(v);
            var v2 = new Venue("Székhely (seed)", "", "test@test.com",1, null) { Id = 2};
            v2.OfficeAddress = new Address(6724, "Szeged", "H-6724 Szeged, Kossuth Lajos sugárút 72/B", 1);
            context.Venues.Add(v2);
#endif
            var venue = new Venue("Super Venue") { Id = 10042, Email = "test@test.com", PhoneNumber = "+838483288", CompanyId = 10000 };
            venue.OfficeAddress = new Address(1234, "SuperCity", "Super street 42.", 1);

            var dayTypeForVenue = new DayType(DayTypeEnum.Wednesday, 4) { Id = 10007 };
            context.DayTypes.Add(dayTypeForVenue);

            var meetingRoomForVenue = new MeetingRoom("Pici szoba", "Picike", venue.Id) { Id = 10020 };
            context.MeetingRooms.Add(meetingRoomForVenue);

            var venueDateRangeListForVenue = new List<VenueDateRange>();
            var venueDateRangeForVenue = new VenueDateRange(venue.Id, dayTypeForVenue.Id, new DateRange(new DateTime(2019, 7, 25), new DateTime(2019, 7, 31)));
            venueDateRangeListForVenue.Add(venueDateRangeForVenue);

            venue.AddOpeningHour(venueDateRangeForVenue);
            context.VenueDateRanges.Add(venueDateRangeForVenue);

            venue.MeetingRooms = new List<MeetingRoom>()
            {
                meetingRoomForVenue
            };

            var notExistVenue = new Venue("Venue the Brave") { Id = 10052, CompanyId = 10000 };
            notExistVenue.OfficeAddress = new Address(5678, "Brave City", "Bravery Avenue 12", 1);
            var notExistMeetingRoom = new MeetingRoom("Csodaszoba", "Csodák csodája", notExistVenue.Id) { Id = 10013 };
            var MeetingRoomForDelete = new MeetingRoom("Csodaszoba", "Csodák csodája", notExistVenue.Id) { Id = 10014 };
            context.MeetingRooms.Add(notExistMeetingRoom);
            context.MeetingRooms.Add(MeetingRoomForDelete);

            var dayType = new DayType(DayTypeEnum.Friday, 8) { Id = 10023 };
            context.DayTypes.Add(dayType);

            var venueDateRangeList = new List<VenueDateRange>();
            var venueDateRange = new VenueDateRange(notExistVenue.Id, dayType.Id, new DateRange(new DateTime(2019, 7, 10), new DateTime(2019, 7, 20)));
            venueDateRangeList.Add(venueDateRange);

            notExistVenue.AddOpeningHour(venueDateRange);
            context.VenueDateRanges.Add(venueDateRange);

            var secondVenue = new Venue("Test V2") { Id = 10001, Email = "test@test.com", PhoneNumber = "+873483234", OfficeAddress = new Address(3455, "City", "Schön street 23.", 1), CompanyId = 10000 };
            context.Venues.Add(secondVenue);

            notExistVenue.MeetingRooms = new List<MeetingRoom>()
            {
                notExistMeetingRoom,
                MeetingRoomForDelete
            };
            context.Venues.Add(venue);
            context.Venues.Add(notExistVenue);
        }

        private static void PopulateOrderData(IFPSSalesContext context)
        {
            // Countries

            // Currencies
            var currency = new Currency("HUF") { Id = 10900 };
            context.Currencies.Add(currency);

            // Images
            var image = new Image("image", ".jpg", "folder") { Id = new Guid("864DD88B-FC83-43A9-99F9-6E6B1951D779") };

            // Addresses
            //var address = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1);

            // Categories
            var category = new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) { Id = 10700 };
            context.GroupingCategories.Add(category);

            // Category Translations
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation("Unicorn", LanguageTypeEnum.EN) { Id = 10700, CoreId = 10700, Core = category });
            context.GroupingCategoryTranslations.Add(new GroupingCategoryTranslation("Unikornis", LanguageTypeEnum.HU) { Id = 10701, CoreId = 10700, Core = category });

            // UserDatas
            var userData = new UserData("John Doe", "45348976", Clock.Now) { Id = 10720, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };

            var thirdUser = new User("contact@contact.de", LanguageTypeEnum.HU) { Id = 10702, CurrentVersionId = userData.Id, CurrentVersion = userData };
            context.Users.Add(thirdUser);

            // CompanyDatas
            var companyData = new CompanyData("Tax-150", "Reg-762", new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), null, Clock.Now)
            {
                Id = 10780,
                ContactPersonId = thirdUser.Id,
                ContactPerson = thirdUser
            };
            context.CompanyData.Add(companyData);

            // CompanyTypes
            var firstCompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10700 };
            context.CompanyTypes.Add(firstCompanyType);

            // Companies
            var firstCompany = new Company("Bayerische Motor Werk", firstCompanyType.Id)
            {
                Id = 10700,
                CurrentVersionId = companyData.Id,
                CurrentVersion = companyData
            };
            context.Companies.Add(firstCompany);

            // Users
            var firstUser = new User("kunden@kunden.de", LanguageTypeEnum.HU)
            {
                Id = 10700,
                UserName = "enco",
                PasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw ==",
                SecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR",
                EmailConfirmed = true,
                ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),
                CompanyId = firstCompany.Id,
                Company = firstCompany,
                CurrentVersionId = userData.Id,
                CurrentVersion = userData
            };
            var secondUser = new User("sales@sales.de", LanguageTypeEnum.HU)
            {
                Id = 10701,
                CompanyId = firstCompany.Id,
                Company = firstCompany,
                CurrentVersionId = userData.Id,
                CurrentVersion = userData
            };
            context.Users.Add(firstUser);
            context.Users.Add(secondUser);

            // UserDatas
            var firstUserData = new UserData("Heinrich Kunden", "47626234", Clock.Now)
            {
                Id = 10700,
                CoreId = 10700,
                Core = firstUser,
                ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1)
            };
            var secondUserData = new UserData("Joe SalesPerson", "72361645", Clock.Now)
            {
                Id = 10701,
                CoreId = 10701,
                Core = secondUser,
                ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1)
            };
            context.UserData.Add(firstUserData);
            context.UserData.Add(secondUserData);

            // Customers
            var firstCustomer = new Customer(firstUser.Id, Clock.Now)
            {
                Id = 10700,
                User = firstUser
            };
            context.Customers.Add(firstCustomer);
            context.UserRoles.Add(new UserRole(userId: firstCustomer.Id, roleId: 10007));
            context.UserClaims.Add(new UserClaim(firstCustomer.Id, claimId: 40));

            // SalesPersons
            var firstSales = new SalesPerson(secondUser.Id, Clock.Now) { Id = 10700 };
            context.SalesPersons.Add(firstSales);

            // OrderPrices
            var orderPrice = new OrderPrice() { Id = 10000 };
            var orderPrice2 = new OrderPrice() { Id = 10001 };

            //OfferInformations
            var offerInformation = new OfferInformation()
            {
                Id = 10901,
                ProductsPrice = new Price(30000.0, currency.Id),
                ServicesPrice = new Price(25000.0, currency.Id)
            };

            var firstPayment = new OrderPrice() { Id = 10002, Price = new Price(42000.0, currency.Id), Deadline = Clock.Now };
            var secondPayment = new OrderPrice() { Id = 10003, Price = new Price(42000.0, currency.Id), Deadline = Clock.Now };

            var firstPaymentForCreateContract = new OrderPrice() { Id = 10004, Price = new Price(0.0, currency.Id), Deadline = Clock.Now };
            var secondPaymentForCreateContract = new OrderPrice() { Id = 10005, Price = new Price(0.0, currency.Id), Deadline = Clock.Now };

            var firstpayment_no_deadline = new OrderPrice() { Id = 10006, Price = new Price(42000.0, currency.Id), Deadline = DateTime.MinValue };
            var secondpayment_no_deadline = new OrderPrice() { Id = 10007, Price = new Price(42000.0, currency.Id), Deadline = DateTime.MinValue };


            var orderState_waitingForOffer = new OrderState(OrderStateEnum.WaitingForOffer);
            var currentTicket = new Ticket(orderState_waitingForOffer, firstUser, 2) { Id = 10000 };

            #region Orders
            // Orders
            var firstOrder = new Order("Test Bestellung", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("FBBA3CE4-F622-4500-9206-AE8BA8AA6CE7"),
                CurrentTicketId = currentTicket.Id,
                CurrentTicket = currentTicket,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 1010,
                WorkingNumberYear = 2021,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                IsPrivatePerson = false,
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2
            };

            var secondOrder = new Order("Bestellung für Löschung", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("AF02841C-F7EA-408B-9573-59965562AAB8"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 2642,
                WorkingNumberYear = 2027,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her nam was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var thirdOrder = new Order("Add new OFU to Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("10CB8429-4034-4E39-833C-20CA74488166"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 453,
                WorkingNumberYear = 3210,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };
            var fourthOrder = new Order("Update OFU by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("f9057613-1520-4063-b78a-e6bac33b9c57"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 456, WorkingNumberYear = 3211, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            var fifthOrder = new Order("Add Appliance by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("07be975f-f2b8-451b-a99b-3e97c146f067"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 734, WorkingNumberYear = 3212, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2, OfferInformation = offerInformation };
            var sixthOrder = new Order("Update Appliance by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("b931b0bc-08dd-47b6-a17f-7fc70d5ceded"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 003, WorkingNumberYear = 3213, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2, OfferInformation = offerInformation };

            var seventhOrder = new Order("Delete from appliance list", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("EF34A34F-A2DB-4A29-A847-9775640D68DC"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 123,
                WorkingNumberYear = 3214,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };
            var eighthOrder = new Order("Get appliance with quantity", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("135d6a97-1b25-4e3b-827e-a3a02d7b63db"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 444, WorkingNumberYear = 3215, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            var ninthOrder = new Order("Get orderedFurnitureUnit", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("eae1e5f6-21ea-49cf-a068-3e56a475e89b"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 555, WorkingNumberYear = 3216, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            var tenthOrder = new Order("Offer Preview", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("8A2333ED-9A68-4769-86CB-7028DA7BC7AC"),
                OrderName = "Unicorn Offer Test Name",
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 666,
                WorkingNumberYear = 3216,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };
            var eleventhOrder = new Order("Create New Furniture Unit", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("5d007976-d454-4b31-a757-2cbef1ddf7e9"), OrderName = "Magical Unicorn", CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 777, WorkingNumberYear = 3217, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            var twelfthOrder = new Order("Add Service by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("1FD1D5E6-69F1-4B04-8AE0-7CC002EFA7E5"),
                OrderName = "Magnificent Order Service",
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 888,
                WorkingNumberYear = 3218,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var thirteenthOrder = new Order("Add another Appliance by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("91547633-BD5A-491E-B712-6056E1771D91"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 744,
                WorkingNumberYear = 3222,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var fourteenthOrder = new Order("Add another Service by Order", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("6E197CE1-42B4-4EDB-887E-813A43BB2374"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 745,
                WorkingNumberYear = 3223,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var order_for_appliance_to_delete = new Order("Order for appliance to delete", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("CE108CCC-04ED-4930-8FB2-2700478312D3"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 746,
                WorkingNumberYear = 3224,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var order_for_appliance_to_find = new Order("Order for appliance to find", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("AFFC1AC6-CE3E-4ACF-9D26-CE07F7DD133B"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 747,
                WorkingNumberYear = 3225,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var order_to_add_payment_for = new Order("Order for adding payment", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("62e0c6e7-9568-414b-ad67-f17f70978dbe"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                WorkingNumberSerial = 748,
                WorkingNumberYear = 3226,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                FirstPayment = orderPrice,
                SecondPayment = orderPrice2,
                OfferInformation = offerInformation
            };

            var order_to_get_contract = new Order("Order for getting contract", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency })
            {
                Id = new Guid("5C383D29-11C8-4592-B155-2C9C8EBAFC3D"),
                CurrentTicketId = currentTicket.Id,
                ContractDate = DateTime.MinValue,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                Budget = new Price(0.0, currency.Id) { Currency = currency },
                WorkingNumberSerial = 749,
                WorkingNumberYear = 3227,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                FirstPayment = firstpayment_no_deadline,
                SecondPayment = secondpayment_no_deadline,
                OfferInformation = offerInformation
            };

            var getContractOrder = new Order("Get Contract Order", firstCustomer.Id, firstSales.Id, Clock.Now, null)
            {
                Id = new Guid("545C7D12-36AF-4792-9DDE-BA5179A1705F"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                Budget = new Price(0.0, currency.Id) { Currency = currency },
                WorkingNumberSerial = 900,
                WorkingNumberYear = 3230,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                FirstPayment = firstPayment,
                SecondPayment = secondPayment,
                OfferInformation = offerInformation
            };

            var createContractOrder = new Order("Create Contract Order", firstCustomer.Id, firstSales.Id, Clock.Now, null)
            {
                Id = new Guid("04CD23D7-3EC1-4354-9512-C4C239F47260"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                Budget = new Price(0.0, currency.Id) { Currency = currency },
                WorkingNumberSerial = 910,
                WorkingNumberYear = 3231,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                FirstPayment = firstPaymentForCreateContract,
                SecondPayment = secondPaymentForCreateContract
            };

            var order_add_furnitureUnit = new Order("Add furnitureunit to Order", firstCustomer.Id, firstSales.Id, Clock.Now, null)
            {
                Id = new Guid("FC7DA6DC-9959-4C40-ACD8-A02422FEC76E"),
                CurrentTicketId = currentTicket.Id,
                Customer = firstCustomer,
                SalesPerson = firstSales,
                Budget = new Price(0.0, currency.Id) { Currency = currency },
                WorkingNumberSerial = 911,
                WorkingNumberYear = 3232,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                FirstPayment = firstPaymentForCreateContract,
                SecondPayment = secondPaymentForCreateContract
            };

            context.Orders.Add(firstOrder);
            context.Orders.Add(secondOrder);
            context.Orders.Add(thirdOrder);
            context.Orders.Add(fourthOrder);
            context.Orders.Add(fifthOrder);
            context.Orders.Add(sixthOrder);
            context.Orders.Add(seventhOrder);
            context.Orders.Add(eighthOrder);
            context.Orders.Add(ninthOrder);
            context.Orders.Add(tenthOrder);
            context.Orders.Add(eleventhOrder);
            context.Orders.Add(twelfthOrder);
            context.Orders.Add(thirteenthOrder);
            context.Orders.Add(fourteenthOrder);
            context.Orders.Add(order_for_appliance_to_delete);
            context.Orders.Add(order_for_appliance_to_find);
            context.Orders.Add(order_to_add_payment_for);
            context.Orders.Add(order_to_get_contract);
            context.Orders.Add(order_add_furnitureUnit);

            #endregion Orders

            context.Orders.Add(getContractOrder);
            context.Orders.Add(createContractOrder);

            // CabinetMaterials
            var topCabinet = new CabinetMaterial
            {
                Id = 10700,
                OrderId = firstOrder.Id,
                Order = firstOrder,
                Height = 197,
                Depth = 87,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Ich geh' heut' nicht mehr tanzen"
            };

            var baseCabinet = new CabinetMaterial
            {
                Id = 10701,
                OrderId = firstOrder.Id,
                Order = firstOrder,
                Height = 200,
                Depth = 99,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Die Welt geht runter"
            };

            var tallCabinet = new CabinetMaterial
            {
                Id = 10702,
                OrderId = firstOrder.Id,
                Order = firstOrder,
                Height = 200,
                Depth = 87,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Ich liebe auto fahre"
            };

            context.CabinetMaterials.Add(topCabinet);
            context.CabinetMaterials.Add(baseCabinet);
            context.CabinetMaterials.Add(tallCabinet);


            // FurnitureUnitPrices
            var firstFurnitureUnitPrice = new FurnitureUnitPrice
            {
                Id = 10600,
                Price = new Price(12000.0, 10900) { Currency = currency },
                MaterialCost = new Price(12000.0, 10900) { Currency = currency }
            };
            context.FurnitureUnitPrices.Add(firstFurnitureUnitPrice);

            // FurnitureUnitTypes
            var topType = new FurnitureUnitType(FurnitureUnitTypeEnum.Top) { Id = 10400 };
            context.FurnitureUnitTypes.Add(topType);
            var topTranslationEN = new FurnitureUnitTypeTranslation(topType.Id, "Top", LanguageTypeEnum.EN) { Id = 10200 };
            var topTranslationHU = new FurnitureUnitTypeTranslation(topType.Id, "Felső", LanguageTypeEnum.HU) { Id = 10210 };
            context.FurnitureUnitTypeTranslations.Add(topTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(topTranslationHU);

            var tallType = new FurnitureUnitType(FurnitureUnitTypeEnum.Tall) { Id = 10410 };
            context.FurnitureUnitTypes.Add(tallType);
            var tallTranslationEN = new FurnitureUnitTypeTranslation(tallType.Id, "Tall", LanguageTypeEnum.EN) { Id = 10220 };
            var tallTranslationHU = new FurnitureUnitTypeTranslation(tallType.Id, "Magas", LanguageTypeEnum.HU) { Id = 10230 };
            context.FurnitureUnitTypeTranslations.Add(tallTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(tallTranslationHU);

            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10420 };
            context.FurnitureUnitTypes.Add(baseType);
            var baseTranslationEN = new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10240 };
            var baseTranslationHU = new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10250 };
            context.FurnitureUnitTypeTranslations.Add(baseTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(baseTranslationHU);

            // FurnitureUnits
            var firstFurnitureUnit = new FurnitureUnit("Test-FU-503", 84, 192, 46) { Id = new Guid("53eeeb26-5eb4-487e-9f69-89211fb59e48"), Description = "Unicorn Power", FurnitureUnitTypeId = topType.Id, FurnitureUnitType = topType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice };
            var secondFurnitureUnit = new FurnitureUnit("Test-FU-880", 75, 120, 50) { Id = new Guid("f2ace9bf-e9b7-42e6-91fe-75387690708b"), Description = "Unicorn Strong Power", FurnitureUnitTypeId = tallType.Id, FurnitureUnitType = tallType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice, CategoryId = category.Id, Category = category };
            var thirdFurnitureUnit = new FurnitureUnit("Delete-FU-000", 75, 120, 50) { Id = new Guid("205de084-7a56-4c2b-881c-26859b5bff7d"), Description = "DELETE", FurnitureUnitTypeId = tallType.Id, FurnitureUnitType = tallType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice };
            var fourthFurnitureUnit = new FurnitureUnit("Preview-FU-188", 75, 120, 50) { Id = new Guid("4bb65c4f-eae3-422f-8ce8-1d2f4336d501"), Description = "preview vol.1", FurnitureUnitTypeId = tallType.Id, FurnitureUnitType = tallType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice };
            var fifthFurnitureUnit = new FurnitureUnit("Preview-FU-230", 75, 120, 50) { Id = new Guid("93ff53f8-194c-45c4-94ad-2f1428f9bd2b"), Description = "preview vol.2", FurnitureUnitTypeId = topType.Id, FurnitureUnitType = topType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice };
            var sixthFurnitureUnit = new FurnitureUnit("Create New FU-420", 75, 120, 50) { Id = new Guid("bec81e89-8b3e-4460-ba7d-86e9c9331cf5"), Description = "hurray, it is new!", FurnitureUnitTypeId = topType.Id, FurnitureUnitType = topType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice, CategoryId = category.Id, Category = category };
            var sevethFurnitureUnit = new FurnitureUnit("Create New FU-660", 75, 120, 50) { Id = new Guid("74564600-8BD1-4E67-82CC-ED510C974A82"), Description = "hurray, it is new!", FurnitureUnitTypeId = topType.Id, FurnitureUnitType = topType, ImageId = image.Id, Image = image, CurrentPriceId = firstFurnitureUnitPrice.Id, CurrentPrice = firstFurnitureUnitPrice, CategoryId = category.Id, Category = category };

            context.FurnitureUnits.Add(firstFurnitureUnit);
            context.FurnitureUnits.Add(secondFurnitureUnit);
            context.FurnitureUnits.Add(thirdFurnitureUnit);
            context.FurnitureUnits.Add(fourthFurnitureUnit);
            context.FurnitureUnits.Add(fifthFurnitureUnit);
            context.FurnitureUnits.Add(sixthFurnitureUnit);
            context.FurnitureUnits.Add(sevethFurnitureUnit);

            // FoilMaterials
            var foilMaterial = new FoilMaterial("Test-FM-1001") { Id = new Guid("D2CBCC5E-2136-432F-98C2-B37553EFC555"), Description = "Foil unicorn" };

            // AccessoryMaterials
            var materialPrice = new MaterialPrice(new Price(1000.0, 1) { CurrencyId = currency.Id, Currency = currency });
            context.MaterialPrices.Add(materialPrice);
            var firstAccessoryMaterial = new AccessoryMaterial { Id = new Guid("AED9A385-2D35-4C87-83D2-2B18AC4FFCA2"), Code = "Test-AM-Code-14", CurrentPriceId = materialPrice.Id };
            var secondAccessoryMaterial = new AccessoryMaterial { Id = new Guid("500D30DA-56DF-468E-9973-6DFBB1C0C68E"), Code = "Test-AM-Code-222" };
            context.Materials.Add(firstAccessoryMaterial);
            context.Materials.Add(secondAccessoryMaterial);

            // AccessoryMaterialFurnitureUnits
            var firstAMFU = new AccessoryMaterialFurnitureUnit() { Id = 10700, FurnitureUnitId = firstFurnitureUnit.Id, FurnitureUnit = firstFurnitureUnit, AccessoryId = firstAccessoryMaterial.Id, Accessory = firstAccessoryMaterial };
            var secondAMFU = new AccessoryMaterialFurnitureUnit() { Id = 10701, FurnitureUnitId = secondFurnitureUnit.Id, FurnitureUnit = secondFurnitureUnit, AccessoryId = secondAccessoryMaterial.Id, Accessory = secondAccessoryMaterial };
            context.AccessoryMaterialFurnitureUnits.Add(firstAMFU);
            context.AccessoryMaterialFurnitureUnits.Add(secondAMFU);

            // FurnitureComponents
            var firstFurnitureComponent = new FurnitureComponent("Test-FC-1298", 2) { Id = new Guid("80AB53A1-EA7A-4693-A00E-E2EC7D30B04D"), FurnitureUnitId = firstFurnitureUnit.Id, FurnitureUnit = firstFurnitureUnit, ImageId = image.Id };
            var secondFurnitureComponent = new FurnitureComponent("Test-FC-0009", 2) { Id = new Guid("00156A43-DD90-4484-9976-A491403C651B"), FurnitureUnitId = firstFurnitureUnit.Id, FurnitureUnit = firstFurnitureUnit, ImageId = image.Id };
            var thirdFurnitureComponent = new FurnitureComponent("Test-FC-5067", 2)
            {
                Id = new Guid("286F4C7B-F4E4-4337-96E8-7B079FDCF578"),
                Type = FurnitureComponentTypeEnum.Front,
                FurnitureUnitId = secondFurnitureUnit.Id,
                FurnitureUnit = secondFurnitureUnit,
                ImageId = image.Id,
                Image = image,
                BottomFoilId = foilMaterial.Id,
                BottomFoil = foilMaterial,
                TopFoilId = foilMaterial.Id,
                TopFoil = foilMaterial,
                RightFoilId = foilMaterial.Id,
                RightFoil = foilMaterial,
                LeftFoilId = foilMaterial.Id,
                LeftFoil = foilMaterial
            };
            var fourthFurnitureComponent = new FurnitureComponent("Test-FC-2000", 2)
            {
                Id = new Guid("95B7887F-65F9-4B91-A676-62E1F8BBBDCE"),
                Type = FurnitureComponentTypeEnum.Corpus,
                FurnitureUnitId = secondFurnitureUnit.Id,
                FurnitureUnit = secondFurnitureUnit,
                ImageId = image.Id,
                Image = image,
                BottomFoilId = foilMaterial.Id,
                BottomFoil = foilMaterial,
                TopFoilId = foilMaterial.Id,
                TopFoil = foilMaterial,
                RightFoilId = foilMaterial.Id,
                RightFoil = foilMaterial,
                LeftFoilId = foilMaterial.Id,
                LeftFoil = foilMaterial
            };
            var fifthFurnitureComponent = new FurnitureComponent("Test-FC-2000", 2) { Id = new Guid("E696CF9E-9997-4A6A-B88D-9BDF0A0ECB22"), FurnitureUnitId = thirdFurnitureUnit.Id, FurnitureUnit = thirdFurnitureUnit, ImageId = image.Id };
            var sixthFurnitureComponent = new FurnitureComponent("Test-FC-2680", 4)
            {
                Id = new Guid("948B8E46-B039-4178-895D-67156108FE61"),
                Type = FurnitureComponentTypeEnum.Front,
                FurnitureUnitId = secondFurnitureUnit.Id,
                FurnitureUnit = secondFurnitureUnit,
                ImageId = image.Id,
                Image = image,
                BottomFoilId = foilMaterial.Id,
                BottomFoil = foilMaterial,
                TopFoilId = foilMaterial.Id,
                TopFoil = foilMaterial,
                RightFoilId = foilMaterial.Id,
                RightFoil = foilMaterial,
                LeftFoilId = foilMaterial.Id,
                LeftFoil = foilMaterial
            };
            var seventhFurnitureComponent = new FurnitureComponent("Test-FC-3120", 7)
            {
                Id = new Guid("56A1EB6F-3B04-4928-8551-53677639C6A8"),
                Type = FurnitureComponentTypeEnum.Corpus,
                FurnitureUnitId = secondFurnitureUnit.Id,
                FurnitureUnit = secondFurnitureUnit,
                ImageId = image.Id,
                Image = image,
                BottomFoilId = foilMaterial.Id,
                BottomFoil = foilMaterial,
                TopFoilId = foilMaterial.Id,
                TopFoil = foilMaterial,
                RightFoilId = foilMaterial.Id,
                RightFoil = foilMaterial,
                LeftFoilId = foilMaterial.Id,
                LeftFoil = foilMaterial
            };
            context.FurnitureComponents.Add(firstFurnitureComponent);
            context.FurnitureComponents.Add(secondFurnitureComponent);
            context.FurnitureComponents.Add(thirdFurnitureComponent);
            context.FurnitureComponents.Add(fourthFurnitureComponent);
            context.FurnitureComponents.Add(fifthFurnitureComponent);
            context.FurnitureComponents.Add(sixthFurnitureComponent);
            context.FurnitureComponents.Add(seventhFurnitureComponent);

            // Add values to lists in FurnitureUnit
            firstFurnitureUnit.AddFurnitureComponent(firstFurnitureComponent);
            secondFurnitureUnit.AddFurnitureComponent(secondFurnitureComponent);
            thirdFurnitureUnit.AddFurnitureComponent(thirdFurnitureComponent);
            fourthFurnitureUnit.AddFurnitureComponent(fourthFurnitureComponent);
            fifthFurnitureUnit.AddFurnitureComponent(fifthFurnitureComponent);
            sixthFurnitureUnit.AddFurnitureComponent(sixthFurnitureComponent);
            sevethFurnitureUnit.AddFurnitureComponent(seventhFurnitureComponent);

            firstFurnitureUnit.AddAccessory(firstAMFU);
            secondFurnitureUnit.AddAccessory(secondAMFU);

            // OrderedFurnitureUnits
            var firstOrderedFurnitureUnit = new OrderedFurnitureUnit(firstOrder.Id, firstFurnitureUnit.Id, 1) { Id = 10700, Order = firstOrder, FurnitureUnit = firstFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var secondOrderedFurnitureUnit = new OrderedFurnitureUnit(firstOrder.Id, secondFurnitureUnit.Id, 1) { Id = 10701, Order = firstOrder, FurnitureUnit = secondFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var thirdOrderedFurnitureUnit = new OrderedFurnitureUnit(secondOrder.Id, thirdFurnitureUnit.Id, 1) { Id = 10702, Order = secondOrder, FurnitureUnit = thirdFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var fourthOrderedFurnitureUnit = new OrderedFurnitureUnit(fourthOrder.Id, thirdFurnitureUnit.Id, 2) { Id = 10703, Order = fourthOrder, FurnitureUnit = thirdFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var fifthOrderedFurnitureUnit = new OrderedFurnitureUnit(ninthOrder.Id, secondFurnitureUnit.Id, 9) { Id = 10704, Order = ninthOrder, FurnitureUnit = secondFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var sixthOrderedFurnitureUnit = new OrderedFurnitureUnit(tenthOrder.Id, fourthFurnitureUnit.Id, 12) { Id = 10705, Order = tenthOrder, FurnitureUnit = fourthFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var seventhOrderedFurnitureUnit = new OrderedFurnitureUnit(tenthOrder.Id, fifthFurnitureUnit.Id, 7) { Id = 10706, Order = tenthOrder, FurnitureUnit = fifthFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var eighthOrderedFurnitureUnit = new OrderedFurnitureUnit(eleventhOrder.Id, sixthFurnitureUnit.Id, 30) { Id = 10707, Order = eleventhOrder, FurnitureUnit = sixthFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };
            var ninethOrderedFurnitureUnit = new OrderedFurnitureUnit(order_add_furnitureUnit.Id, sevethFurnitureUnit.Id, 30) { Id = 10708, Order = order_add_furnitureUnit, FurnitureUnit = sevethFurnitureUnit, UnitPrice = new Price(2000.0, 10900) };

            context.OrderedFurnitureUnits.Add(firstOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(secondOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(thirdOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(fourthOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(fifthOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(sixthOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(seventhOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(eighthOrderedFurnitureUnit);
            context.OrderedFurnitureUnits.Add(ninethOrderedFurnitureUnit);

            // ApplianceMaterials
            var firstApplianceMaterial = new ApplianceMaterial { Id = new Guid("D0D94BA5-E70C-45FD-BD15-4E9095B12DAE"), Code = "App-Mat-001", SellPrice = new Price(1000.0, 1), BrandId = firstCompany.Id, Brand = firstCompany };
            var secondApplianceMaterial = new ApplianceMaterial { Id = new Guid("75E3A976-429F-4F1C-B965-FCCF0522040E"), Code = "App-Mat-033", SellPrice = new Price(1000.0, 1), BrandId = firstCompany.Id, Brand = firstCompany };
            var thirdApplianceMaterial = new ApplianceMaterial { Id = new Guid("805E17D5-C617-4A11-BF2F-3A0C6CA0FE1E"), Code = "App-Mat-003", SellPrice = new Price(1000.0, 1), BrandId = firstCompany.Id, Brand = firstCompany };
            var fourthApplianceMaterial = new ApplianceMaterial
            {
                Id = new Guid("04CD23D7-3EC1-4354-9512-C4C239F47260"),
                Code = "App-Mat-042",
                SellPrice = new Price(1000.0, 1) { CurrencyId = currency.Id, Currency = currency },
                BrandId = firstCompany.Id,
                Brand = firstCompany,
                ImageId = image.Id,
                Image = image
            };

            var fifthApplianceMaterial = new ApplianceMaterial
            {
                Id = new Guid("C898F5E9-78C9-4A9A-9A06-F444113221D1"),
                Code = "App-Mat-043",
                SellPrice = new Price(1000.0, 1) { CurrencyId = currency.Id, Currency = currency },
                BrandId = firstCompany.Id,
                Brand = firstCompany,
                ImageId = image.Id,
                Image = image
            };

            var sixthApplianceMaterial = new ApplianceMaterial
            {
                Id = new Guid("851F7783-CC9D-418D-A725-64F5512C390E"),
                Code = "App-Mat-044",
                SellPrice = new Price(1000.0, 1) { CurrencyId = currency.Id, Currency = currency },
                BrandId = firstCompany.Id,
                Brand = firstCompany,
                ImageId = image.Id,
                Image = image
            };

            var seventhApplianceMaterial = new ApplianceMaterial
            {
                Id = new Guid("A2B08927-08EE-4B3A-B4F3-0B0129DDD085"),
                Code = "App-Mat-045",
                SellPrice = new Price(1000.0, 1) { CurrencyId = currency.Id, Currency = currency },
                BrandId = firstCompany.Id,
                Brand = firstCompany,
                ImageId = image.Id,
                Image = image
            };

            context.Materials.Add(firstApplianceMaterial);
            context.Materials.Add(secondApplianceMaterial);
            context.Materials.Add(thirdApplianceMaterial);
            context.Materials.Add(fourthApplianceMaterial);
            context.Materials.Add(fifthApplianceMaterial);
            context.Materials.Add(sixthApplianceMaterial);
            context.Materials.Add(seventhApplianceMaterial);

            // OrderedApplianceMaterial
            var firstOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10700, OrderId = new Guid("FBBA3CE4-F622-4500-9206-AE8BA8AA6CE7"), Order = firstOrder, ApplianceMaterialId = firstApplianceMaterial.Id, ApplianceMaterial = firstApplianceMaterial };
            var secondOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10701, OrderId = secondOrder.Id, Order = firstOrder, ApplianceMaterialId = secondApplianceMaterial.Id, ApplianceMaterial = secondApplianceMaterial };
            var thirdOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10702, OrderId = new Guid("B931B0BC-08DD-47B6-A17F-7FC70D5CEDED"), Order = sixthOrder, ApplianceMaterialId = secondApplianceMaterial.Id, ApplianceMaterial = secondApplianceMaterial, Quantity = 7 };
            var fourthOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10703, OrderId = new Guid("EF34A34F-A2DB-4A29-A847-9775640D68DC"), Order = seventhOrder, ApplianceMaterialId = thirdApplianceMaterial.Id, ApplianceMaterial = thirdApplianceMaterial, Quantity = 10 };
            var fifthOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10704, OrderId = eighthOrder.Id, Order = eighthOrder, ApplianceMaterialId = thirdApplianceMaterial.Id, ApplianceMaterial = thirdApplianceMaterial, Quantity = 14 };
            var sixthOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10705, OrderId = tenthOrder.Id, Order = tenthOrder, ApplianceMaterialId = fourthApplianceMaterial.Id, ApplianceMaterial = fourthApplianceMaterial, Quantity = 21 };
            var seventhOrderedApplianceMaterial = new OrderedApplianceMaterial { Id = 10706, OrderId = thirteenthOrder.Id, Order = thirteenthOrder, ApplianceMaterialId = fifthApplianceMaterial.Id, ApplianceMaterial = fifthApplianceMaterial, Quantity = 10 };
            var appliance_to_delete = new OrderedApplianceMaterial { Id = 10707, OrderId = order_for_appliance_to_delete.Id, Order = order_for_appliance_to_delete, ApplianceMaterialId = sixthApplianceMaterial.Id, ApplianceMaterial = sixthApplianceMaterial, Quantity = 10 };
            var appliance_to_find = new OrderedApplianceMaterial { Id = 10708, OrderId = order_for_appliance_to_find.Id, Order = order_for_appliance_to_find, ApplianceMaterialId = seventhApplianceMaterial.Id, ApplianceMaterial = seventhApplianceMaterial, Quantity = 20 };

            context.OrderedApplianceMaterials.Add(firstOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(secondOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(thirdOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(fourthOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(fifthOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(sixthOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(seventhOrderedApplianceMaterial);
            context.OrderedApplianceMaterials.Add(appliance_to_delete);
            context.OrderedApplianceMaterials.Add(appliance_to_find);

            // ServiceTypes
            var firstServiceType = new ServiceType(ServiceTypeEnum.Shipping) { Id = 10700 };
            context.ServiceTypes.Add(firstServiceType);

            // ServicePrices
            var firstServicePrice = new ServicePrice() { Id = 10300, Price = new Price(2000.0, 10900) { Currency = currency } };
            var secondServicePrice = new ServicePrice() { Id = 10301, Price = new Price(4000.0, 10900) { Currency = currency } };

            // Services
            var firstService = new Service("50-100 km") { Id = 10700, ServiceTypeId = 10700, ServiceType = firstServiceType, CurrentPriceId = firstServicePrice.Id, CurrentPrice = firstServicePrice };
            var fourthService = new Service("45-55 km") { Id = 10703, ServiceTypeId = 10700, ServiceType = firstServiceType, CurrentPriceId = secondServicePrice.Id, CurrentPrice = secondServicePrice };
            var seventhService = new Service("60-65 km") { Id = 10706, ServiceTypeId = 10700, ServiceType = firstServiceType, CurrentPriceId = secondServicePrice.Id, CurrentPrice = secondServicePrice };
            var tenthService = new Service("10-40 km") { Id = 10709, ServiceTypeId = 10701, ServiceType = firstServiceType, CurrentPriceId = secondServicePrice.Id, CurrentPrice = secondServicePrice };

            context.Services.Add(firstService);
            context.Services.Add(fourthService);
            context.Services.Add(seventhService);
            context.Services.Add(tenthService);

            //Add values to lists in order
            firstOrder.AddOrderedFurnitureUnit(firstOrderedFurnitureUnit);
            firstOrder.AddOrderedFurnitureUnit(secondOrderedFurnitureUnit);

            secondOrder.AddOrderedFurnitureUnit(thirdOrderedFurnitureUnit);

            tenthOrder.AddOrderedFurnitureUnit(sixthOrderedFurnitureUnit);
            tenthOrder.AddOrderedFurnitureUnit(seventhOrderedFurnitureUnit);

            firstOrder.AddAppliance(firstOrderedApplianceMaterial);
            firstOrder.AddAppliance(secondOrderedApplianceMaterial);

            tenthOrder.AddAppliance(sixthOrderedApplianceMaterial);
            thirteenthOrder.AddAppliance(seventhOrderedApplianceMaterial);


            var orderToUpdate = new Order("orderToUpdate", firstCustomer.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("1bca75cd-6105-42c2-97b7-af2239c987a2"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 1110, WorkingNumberYear = 2021, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", IsPrivatePerson = false, FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            context.Orders.Add(orderToUpdate);
            var documentGroup1 = DocumentGroup.FromSeedData(orderToUpdate.Id, 1, true, 10001);
            var documentGroup2 = DocumentGroup.FromSeedData(orderToUpdate.Id, 2, true, 10002);
            var documentGroup3 = DocumentGroup.FromSeedData(orderToUpdate.Id, 3, true, 10003);
            var documentGroup4 = DocumentGroup.FromSeedData(orderToUpdate.Id, 4, false, 10004);
            var documentGroup5 = DocumentGroup.FromSeedData(orderToUpdate.Id, 5, false, 10005);
            var documentGroupVersion1 = DocumentGroupVersion.FromSeedData(10001, 5, Clock.Now, null, 10001);
            var documentGroupVersion2 = DocumentGroupVersion.FromSeedData(10002, 5, Clock.Now, null, 10002);
            var documentGroupVersion3 = DocumentGroupVersion.FromSeedData(10003, 5, Clock.Now, null, 10003);
            var documentGroupVersion4 = DocumentGroupVersion.FromSeedData(10004, 5, Clock.Now, null, 10004);
            var documentGroupVersion5 = DocumentGroupVersion.FromSeedData(10005, 5, Clock.Now, null, 10005);
            documentGroup1.AddNewVersion(documentGroupVersion1);
            documentGroup2.AddNewVersion(documentGroupVersion2);
            documentGroup3.AddNewVersion(documentGroupVersion3);
            documentGroup4.AddNewVersion(documentGroupVersion4);
            documentGroup5.AddNewVersion(documentGroupVersion5);
            context.DocumentGroups.Add(documentGroup1);
            context.DocumentGroups.Add(documentGroup2);
            context.DocumentGroups.Add(documentGroup3);
            context.DocumentGroups.Add(documentGroup4);
            context.DocumentGroups.Add(documentGroup5);


            var userData4 = new UserData("Nagy Gertrúd", "45348976", Clock.Now) { Id = 10800, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
            var user4 = new User("beviz.elek@envotest.hu", LanguageTypeEnum.HU)
            {
                Id = 10800,
                UserName = "beviz elek",
                PasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==",
                SecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR",
                EmailConfirmed = true,
                ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),
                CompanyId = firstCompany.Id,
                Company = firstCompany,
                CurrentVersionId = userData4.Id,
                CurrentVersion = userData4
            };
            context.Users.Add(user4);
            context.UserRoles.Add(new UserRole(userId: user4.Id, roleId: 10007));
            context.UserClaims.Add(new UserClaim(user4.Id, claimId: 40));

            var userData5 = new UserData("Approve Document User Data", "45348976", Clock.Now)
            {
                Id = 10850,
                ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1)
            };
            var user5 = new User("approve@encotest.hu", LanguageTypeEnum.HU)
            {
                Id = 10850,
                UserName = "Approve Document",
                PasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==",
                SecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR",
                EmailConfirmed = true,
                ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),
                CompanyId = firstCompany.Id,
                Company = firstCompany,
                CurrentVersionId = userData5.Id,
                CurrentVersion = userData5
            };
            context.Users.Add(user5);
            context.UserRoles.Add(new UserRole(userId: user5.Id, roleId: 10007));
            context.UserClaims.Add(new UserClaim(user5.Id, claimId: 40));

            var userDataDeleteOrder = new UserData("Delete Order User Data", "45348976", Clock.Now)
            {
                Id = 10890,
                ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1)
            };
            var userDeleteOrder = new User("userdeleteorder@encotest.hu", LanguageTypeEnum.HU)
            {
                Id = 10890,
                UserName = "Delete Order",
                PasswordHash = "AQAAAAEAACcQAAAAEFI3pBoZU+sYQqse9aPfNrXVlPnDNcVmrUtIUIQ6hGmSiu6MHG5pUMJfPB0yggStgw==",
                SecurityStamp = "ZCRQFCSL4ABNPPR5SY3FRTGDHTKZZ7GR",
                EmailConfirmed = true,
                ImageId = new Guid("D9BD4A0D-47B9-4188-90C7-BEAE54626523"),
                CompanyId = firstCompany.Id,
                Company = firstCompany,
                CurrentVersionId = userDataDeleteOrder.Id,
                CurrentVersion = userDataDeleteOrder
            };
            context.Users.Add(userDeleteOrder);
            context.UserRoles.Add(new UserRole(userId: userDeleteOrder.Id, roleId: 10007));
            context.UserClaims.Add(new UserClaim(userDeleteOrder.Id, claimId: 40));

            var customer2 = new Customer(user4.Id, Clock.Now) { Id = 10800 };
            context.Customers.Add(customer2);
            var customer3 = new Customer(user5.Id, Clock.Now) { Id = 10850 };
            context.Customers.Add(customer3);

            var customerOrderDeleted = new Customer(userDeleteOrder.Id, Clock.Now) { Id = 10890 };
            context.Customers.Add(customerOrderDeleted);

            var orderWithDocuments = new Order("orderWithDocuments", customer2.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("43fe7efe-714b-4811-9872-395046428a7f"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 1111, WorkingNumberYear = 2022, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", IsPrivatePerson = false, FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            context.Orders.Add(orderWithDocuments);
            var documentGroup6 = DocumentGroup.FromSeedData(orderWithDocuments, 1, true, 10006);
            var documentGroup7 = DocumentGroup.FromSeedData(orderWithDocuments, 2, true, 10007);
            var documentGroup8 = DocumentGroup.FromSeedData(orderWithDocuments, 3, true, 10008);
            var documentGroup9 = DocumentGroup.FromSeedData(orderWithDocuments, 4, false, 10009);
            var documentGroup10 = DocumentGroup.FromSeedData(orderWithDocuments, 5, false, 10010);
            var documentGroupVersion6 = DocumentGroupVersion.FromSeedData(documentGroup6, 2, Clock.Now, null, 10006);
            var documentGroupVersion7 = DocumentGroupVersion.FromSeedData(documentGroup7, 5, Clock.Now, null, 10007);
            var documentGroupVersion8 = DocumentGroupVersion.FromSeedData(documentGroup8, 5, Clock.Now, null, 10008);
            var documentGroupVersion9 = DocumentGroupVersion.FromSeedData(documentGroup9, 5, Clock.Now, null, 10009);
            var documentGroupVersion10 = DocumentGroupVersion.FromSeedData(documentGroup10, 5, Clock.Now, null, 10010);
            var document1 = Document.FromSeedData("8c6c9f0f-0d73-4359-b082-6a411b030e73.png", ".png", "OrderTests", "Test_Img_1.pmg", FileExtensionTypeEnum.Picture, 1, null, documentGroupVersion6, new Guid("59dd3012-0d93-4af6-b2a6-0c8650bcbd80"));
            var document2 = Document.FromSeedData("9dbefc91-a054-4b86-986d-f649e1a4881f.png", ".png", "OrderTests", "Test_Img_2.pmg", FileExtensionTypeEnum.Picture, 1, null, documentGroupVersion6, new Guid("e67bf787-f066-4458-b67b-4e7be17ce24a"));
            documentGroupVersion6.AddDocument(document1);
            documentGroupVersion6.AddDocument(document2);
            documentGroup6.AddNewVersion(documentGroupVersion6);
            documentGroup7.AddNewVersion(documentGroupVersion7);
            documentGroup8.AddNewVersion(documentGroupVersion8);
            documentGroup9.AddNewVersion(documentGroupVersion9);
            documentGroup10.AddNewVersion(documentGroupVersion10);
            context.DocumentGroups.Add(documentGroup6);
            context.DocumentGroups.Add(documentGroup7);
            context.DocumentGroups.Add(documentGroup8);
            context.DocumentGroups.Add(documentGroup9);
            context.DocumentGroups.Add(documentGroup10);

            var orderToDelete = new Order("orderWithDocuments", customerOrderDeleted.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("4aa425db-791f-4a09-ae70-e409287210eb"), CurrentTicketId = currentTicket.Id, Customer = customerOrderDeleted, SalesPerson = firstSales, WorkingNumberSerial = 1112, WorkingNumberYear = 2022, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", IsPrivatePerson = false, FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            context.Orders.Add(orderToDelete);

            var orderToDocumentService = new Order("orderToDocumentService", customer2.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("228aac26-1066-4d33-af02-986eba4ef15b"), CurrentTicketId = currentTicket.Id, Customer = firstCustomer, SalesPerson = firstSales, WorkingNumberSerial = 11121, WorkingNumberYear = 2023, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", IsPrivatePerson = false, FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            context.Orders.Add(orderToDocumentService);
            var documentGroup11 = DocumentGroup.FromSeedData(orderToDocumentService, 1, true, 10011);
            var documentGroup12 = DocumentGroup.FromSeedData(orderToDocumentService, 2, true, 10012);
            var documentGroup13 = DocumentGroup.FromSeedData(orderToDocumentService, 3, true, 10013);
            var documentGroup14 = DocumentGroup.FromSeedData(orderToDocumentService, 4, false, 10014);
            var documentGroup15 = DocumentGroup.FromSeedData(orderToDocumentService, 5, false, 10015);
            var documentGroupVersion11 = DocumentGroupVersion.FromSeedData(documentGroup11, 2, Clock.Now, null, 10011);
            var documentGroupVersion12 = DocumentGroupVersion.FromSeedData(documentGroup12, 3, Clock.Now, null, 10012);
            var documentGroupVersion13 = DocumentGroupVersion.FromSeedData(documentGroup13, 4, Clock.Now, null, 10013);
            var documentGroupVersion14 = DocumentGroupVersion.FromSeedData(documentGroup14, 5, Clock.Now, null, 10014);
            var documentGroupVersion15 = DocumentGroupVersion.FromSeedData(documentGroup15, 5, Clock.Now, null, 10015);
            documentGroup11.AddNewVersion(documentGroupVersion11);
            documentGroup12.AddNewVersion(documentGroupVersion12);
            documentGroup13.AddNewVersion(documentGroupVersion13);
            documentGroup14.AddNewVersion(documentGroupVersion14);
            documentGroup15.AddNewVersion(documentGroupVersion15);
            context.DocumentGroups.Add(documentGroup11);
            context.DocumentGroups.Add(documentGroup12);
            context.DocumentGroups.Add(documentGroup13);
            context.DocumentGroups.Add(documentGroup14);
            context.DocumentGroups.Add(documentGroup15);


            var documentToAccept = new Order("documentToAccept", customer3.Id, firstSales.Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = currency }) { Id = new Guid("8687d8ae-ce64-4d0d-bbf9-598e5aa40bf2"), CurrentTicketId = currentTicket.Id, SalesPerson = firstSales, WorkingNumberSerial = 1113, WorkingNumberYear = 2022, ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), DescriptionByOffer = "Her name was Lola, she was a showgirl", IsPrivatePerson = false, FirstPayment = orderPrice, SecondPayment = orderPrice2 };
            context.Orders.Add(documentToAccept);

            var documentGroup16 = DocumentGroup.FromSeedData(documentToAccept, 1, true, 10016);
            var documentGroup17 = DocumentGroup.FromSeedData(documentToAccept, 2, true, 10017);
            var documentGroup18 = DocumentGroup.FromSeedData(documentToAccept, 3, true, 10018);
            var documentGroup19 = DocumentGroup.FromSeedData(documentToAccept, 4, false, 10019);
            var documentGroup20 = DocumentGroup.FromSeedData(documentToAccept, 5, false, 10020);
            var documentGroupVersion16 = DocumentGroupVersion.FromSeedData(documentGroup16, 2, Clock.Now, null, 10016);
            var documentGroupVersion17 = DocumentGroupVersion.FromSeedData(documentGroup17, 2, Clock.Now, null, 10017);
            var documentGroupVersion18 = DocumentGroupVersion.FromSeedData(documentGroup18, 4, Clock.Now, null, 10018);
            var documentGroupVersion19 = DocumentGroupVersion.FromSeedData(documentGroup19, 1, Clock.Now, null, 10019);
            var documentGroupVersion20 = DocumentGroupVersion.FromSeedData(documentGroup20, 5, Clock.Now, null, 10020);
            documentGroup16.AddNewVersion(documentGroupVersion16);
            documentGroup17.AddNewVersion(documentGroupVersion17);
            documentGroup18.AddNewVersion(documentGroupVersion18);
            documentGroup19.AddNewVersion(documentGroupVersion19);
            documentGroup20.AddNewVersion(documentGroupVersion20);
            context.DocumentGroups.Add(documentGroup16);
            context.DocumentGroups.Add(documentGroup17);
            context.DocumentGroups.Add(documentGroup18);
            context.DocumentGroups.Add(documentGroup19);
            context.DocumentGroups.Add(documentGroup20);
        }

        private static void PopulateCustomerFurnitureUnitData(IFPSSalesContext context)
        {
            var customer = new Customer(10000, Clock.Now) { Id = 10900 };
            var currency = new Currency("HUF") { Id = 10850 };
            context.Currencies.Add(currency);

            var image = new Image("furnitureunit456.jpg", ".jpg", "MaterialTestImages")
            {
                Id = new Guid("E999E4FA-69B9-4633-98C8-F8E58A48E198"),
                ThumbnailName = "thumbnail_furnitureunit4545.jpg"
            };

            context.Images.Add(image);

            var image2 = new Image("test23.jpg", ".jpg", "MaterialTestImages")
            {
                Id = new Guid("515B4AB1-A087-4373-96F1-25E6C058D03C"),
                ThumbnailName = "test_thumbnail23.jpg"
            };
            context.Images.Add(image2);

            var image3 = new Image("d1v44.png", ".png", "FurnitureUnits")
            {
                Id = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0389"),
                ThumbnailName = "d1v44.png"
            };
            context.Images.Add(image3);

            var price = new FurnitureUnitPrice()
            {
                Id = 10532,
                Price = new Price(10, currency.Id) { Currency = currency },
                MaterialCost = new Price(10, currency.Id)
            };
            context.FurnitureUnitPrices.Add(price);

            // FurnitureUnitTypes
            var topType = new FurnitureUnitType(FurnitureUnitTypeEnum.Top) { Id = 10700 };
            context.FurnitureUnitTypes.Add(topType);
            var topTranslationEN = new FurnitureUnitTypeTranslation(topType.Id, "Top", LanguageTypeEnum.EN) { Id = 10450 };
            var topTranslationHU = new FurnitureUnitTypeTranslation(topType.Id, "Felső", LanguageTypeEnum.HU) { Id = 10451 };
            context.FurnitureUnitTypeTranslations.Add(topTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(topTranslationHU);

            var tallType = new FurnitureUnitType(FurnitureUnitTypeEnum.Tall) { Id = 10710 };
            context.FurnitureUnitTypes.Add(tallType);
            var tallTranslationEN = new FurnitureUnitTypeTranslation(tallType.Id, "Tall", LanguageTypeEnum.EN) { Id = 10452 };
            var tallTranslationHU = new FurnitureUnitTypeTranslation(tallType.Id, "Magas", LanguageTypeEnum.HU) { Id = 10453 };
            context.FurnitureUnitTypeTranslations.Add(tallTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(tallTranslationHU);

            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10720 };
            context.FurnitureUnitTypes.Add(baseType);
            var baseTranslationEN = new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10454 };
            var baseTranslationHU = new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10455 };
            context.FurnitureUnitTypeTranslations.Add(baseTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(baseTranslationHU);

            var f1 = new FurnitureUnit("Test Handle", 60, 200, 60)
            {
                Id = new Guid("44A6A5AC-2C90-49E2-8842-03132DB64232"),
                CategoryId = 10000,
                Trending = true,
                ImageId = image2.Id,
                Image = image2,
                Description = "Silver and metal",
                CurrentPriceId = price.Id,
                CurrentPrice = price,
                FurnitureUnitType = baseType,
                FurnitureUnitTypeId = baseType.Id
            };

            var wfu = new WebshopFurnitureUnit(f1.Id)
            {
                Id = 10011,
                Value = 30000.0,
                FurnitureUnit = f1,
                Price = new Price(30000, currency.Id) { Currency = currency }
            };
            context.WebshopFurnitureUnits.Add(wfu);

            var f2 = new FurnitureUnit("Test Base cabinet with shelves", 20, 40, 37)
            {
                Id = new Guid("9AEC5347-3E49-4493-9CCA-825363858168"),
                Trending = true,
                CategoryId = 10001,
                ImageId = image2.Id,
                Image = image2,
                Description = "Open storage solutions that complement your kitchen",
                CurrentPriceId = price.Id,
                CurrentPrice = price,
                FurnitureUnitType = topType,
                FurnitureUnitTypeId = topType.Id
            };

            var wfu2 = new WebshopFurnitureUnit(f2.Id)
            {
                Id = 10012,
                Value = 100000.0,
                FurnitureUnit = f2,
                Price = new Price(100000, currency.Id) { Currency = currency }
            };
            context.WebshopFurnitureUnits.Add(wfu2);

            var f3 = new FurnitureUnit("Test Unicorn Webshop Unit", 20, 34, 12)
            {
                Id = new Guid("BE472113-CE95-4C91-B5E4-6ACFEB54C638"),
                Trending = true,
                CategoryId = 10002,
                ImageId = image2.Id,
                Image = image2,
                Description = "Spülmaschine",
                CurrentPriceId = price.Id,
                CurrentPrice = price,
                FurnitureUnitType = tallType,
                FurnitureUnitTypeId = tallType.Id
            };

            var wfu3 = new WebshopFurnitureUnit(f3.Id)
            {
                Id = 10013,
                Value = 119000.0,
                FurnitureUnit = f3,
                Price = new Price(100000, currency.Id) { Currency = currency }
            };
            context.WebshopFurnitureUnits.Add(wfu3);

            var fu4 = new FurnitureUnit("Fairy Webshop Unit", 80, 150, 25)
            {
                Id = new Guid("B50D5D13-3885-4584-82C7-C267D848B894"),
                CategoryId = 25,
                ImageId = image3.Id,
                Image = image3,
                Description = "Kücheregal",
                CurrentPriceId = price.Id,
                CurrentPrice = price,
                FurnitureUnitType = topType,
                FurnitureUnitTypeId = topType.Id
            };

            var wfuCustomer = new WebshopFurnitureUnit(fu4.Id)
            {
                Id = 10014,
                Value = 54000.0,
                Price = new Price(54000.0, currency.Id) { Currency = currency },
                FurnitureUnit = fu4
            };
            context.WebshopFurnitureUnits.Add(wfuCustomer);

            var customerFurnitureUnit = new CustomerFurnitureUnit(customer.Id, wfuCustomer.Id) { Id = 10000 };
            context.CustomerFurnitureUnits.Add(customerFurnitureUnit);

            customer.AddRecommendation(customerFurnitureUnit);
            context.Customers.Add(customer);
        }

        private static void PopulateWebshopFurnitureUnitData(IFPSSalesContext context)
        {
            var currency = new Currency("HUF") { Id = 10910 };

            // FurnitureUnitType
            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10666 };
            context.FurnitureUnitTypes.Add(baseType);
            var baseTranslationEN = new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10555 };
            var baseTranslationHU = new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10556 };
            context.FurnitureUnitTypeTranslations.Add(baseTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(baseTranslationHU);

            var image = new Image("accessory4.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ab-4160-bcc5-9660402699da") };
            var image2 = new Image("accessory5.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("e3cfe1d4-fed7-4c72-91dd-e5c356f5d66f") };
            context.Images.Add(image);
            context.Images.Add(image2);

            var fuPrice = new FurnitureUnitPrice() { Id = 10970, Price = new Price(25000.0, currency.Id) { Currency = currency }, MaterialCost = new Price(25000.0, currency.Id) { Currency = currency }, ValidFrom = new DateTime(2019, 10, 01) };
            context.FurnitureUnitPrices.Add(fuPrice);

            var fu = new FurnitureUnit("Test Code for FU", 4700.0, 18.0, 1800.0) { Id = new Guid("a465b4f4-fc87-4785-95af-37acf421776b"), Image = image, ImageId = image.Id, FurnitureUnitType = baseType, FurnitureUnitTypeId = baseType.Id };
            var fu2 = new FurnitureUnit("Test Webshop Code for WFU", 4700.0, 18.0, 1800.0) { Id = new Guid("1b836bc7-0b44-4726-b26a-33056364294b"), Image = image, ImageId = image.Id, FurnitureUnitType = baseType, FurnitureUnitTypeId = baseType.Id };
            fu.AddPrice(fuPrice);
            fu2.AddPrice(fuPrice);
            context.FurnitureUnits.Add(fu);
            context.FurnitureUnits.Add(fu2);

            var wfu = new WebshopFurnitureUnit(fu.Id) { Id = 10000, Price = new Price(30000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu, FurnitureUnitId = fu.Id };
            var wfu2 = new WebshopFurnitureUnit(fu.Id) { Id = 10001, Price = new Price(45000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu, FurnitureUnitId = fu.Id };
            var wfu3 = new WebshopFurnitureUnit(fu.Id) { Id = 10002, Price = new Price(139000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu, FurnitureUnitId = fu.Id };
            var wfu4 = new WebshopFurnitureUnit(fu.Id) { Id = 10003, Price = new Price(145000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu, FurnitureUnitId = fu.Id };
            var wfu5 = new WebshopFurnitureUnit(fu.Id) { Id = 10004, Price = new Price(175000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu2, FurnitureUnitId = fu2.Id };
            var wfu6 = new WebshopFurnitureUnit(fu.Id) { Id = 10005, Price = new Price(189000.0, currency.Id) { Currency = currency }, FurnitureUnit = fu2, FurnitureUnitId = fu2.Id };

            var wfui = new WebshopFurnitureUnitImage() { Id = 10000, Image = image, ImageId = image.Id, WebshopFurnitureUnit = wfu2, WebshopFurnitureUnitId = wfu2.Id };
            var wfui2 = new WebshopFurnitureUnitImage() { Id = 10001, Image = image2, ImageId = image2.Id, WebshopFurnitureUnit = wfu2, WebshopFurnitureUnitId = wfu2.Id };
            var wfui3 = new WebshopFurnitureUnitImage() { Id = 10002, Image = image, ImageId = image.Id, WebshopFurnitureUnit = wfu3, WebshopFurnitureUnitId = wfu3.Id };
            var wfui4 = new WebshopFurnitureUnitImage() { Id = 10003, Image = image2, ImageId = image2.Id, WebshopFurnitureUnit = wfu3, WebshopFurnitureUnitId = wfu3.Id };

            context.WebshopFurnitureUnitImages.Add(wfui);
            context.WebshopFurnitureUnitImages.Add(wfui2);

            wfu2.AddWebshopFurnitureUnitImage(wfui);
            wfu2.AddWebshopFurnitureUnitImage(wfui2);

            wfu3.AddWebshopFurnitureUnitImage(wfui3);
            wfu3.AddWebshopFurnitureUnitImage(wfui4);

            context.WebshopFurnitureUnits.Add(wfu);
            context.WebshopFurnitureUnits.Add(wfu2);
            context.WebshopFurnitureUnits.Add(wfu3);
            context.WebshopFurnitureUnits.Add(wfu4);
            context.WebshopFurnitureUnits.Add(wfu5);
            context.WebshopFurnitureUnits.Add(wfu6);
        }

        private static void PopulateWebshopOrderData(IFPSSalesContext context)
        {
            // FurnitureUnitType
            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10850 };
            context.FurnitureUnitTypes.Add(baseType);
            var baseTranslationEN = new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10350 };
            var baseTranslationHU = new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10351 };
            context.FurnitureUnitTypeTranslations.Add(baseTranslationEN);
            context.FurnitureUnitTypeTranslations.Add(baseTranslationHU);

            var address1 = new Address(1117, "Budapest", "Fő utca 1.", 1);
            var address2 = new Address(1117, "Budapest", "Fő utca 2.", 1);

            var currency = new Currency("HUF") { Id = 10901 };
            context.Currencies.Add(currency);

            var neUser2 = new User("ne@enco.hu") { Id = 10070, CreationTime = new DateTime(2019, 06, 05), CompanyId = 10003 };
            neUser2.AddVersion(new UserData("Nemer Eszti2", "06701234567", Clock.Now,
                 new Address(5000, "Szolnok", "Seholse utca 1.", 1)));
            context.Users.Add(neUser2);

            var customer = new Customer(neUser2.Id, Clock.Now) { Id = 10909 };
            context.Customers.Add(customer);

            var basket = new Basket(10001) { Id = 10000, SubTotal = new Price(2000.0, 10901), DelieveryPrice = new Price(1000.0, 10901) };
            context.Baskets.Add(basket);

            // Get_webshop_orders_for_customer_works
            var worder1 = new WebshopOrder("Test webshop order 1", customer.Id, address1)
            {
                Id = new Guid("7947278c-1979-4776-98be-e2f90d802cf9"),
                CreationTime = Clock.Now,
                Basket = basket,
                BasketId = basket.Id,
                WorkingNumberSerial = 0,
                WorkingNumberYear = 0
            };
            context.WebshopOrders.Add(worder1);

            // Get_webshop_order_details_by_id            
            var img = new Image("furnitureunit002.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("2fad9825-e3e8-4a72-bdc6-68d911513e10"), ThumbnailName = "thumbnail_furnitureunit002.jpg" };
            context.Images.Add(img);

            var currPrice = new FurnitureUnitPrice() { Id = 10010, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id), ValidFrom = Clock.Now };
            context.FurnitureUnitPrices.Add(currPrice);

            var fu = new FurnitureUnit("TESTCODE0001", 200, 200, 200)
            {
                Id = new Guid("d84840a9-1dca-4424-9248-0b81b7e5a491"),
                Description = "Description lorem ipsum",
                CategoryId = 10000,
                ImageId = new Guid("2fad9825-e3e8-4a72-bdc6-68d911513e10"),
                Image = img,
                CurrentPriceId = currPrice.Id,
                CurrentPrice = currPrice,
                FurnitureUnitType = baseType,
                FurnitureUnitTypeId = baseType.Id
            };
            context.FurnitureUnits.Add(fu);

            var ofu = new OrderedFurnitureUnit(new Guid("d84840a9-1dca-4424-9248-0b81b7e5a491"), 2, basket.Id)
            {
                UnitPrice = new Price(12000.0, 10901),
                Basket = basket
            };
            context.OrderedFurnitureUnits.Add(ofu);

            var worder2 = new WebshopOrder("Test webshop order 2", customer.Id, address2)
            {
                Id = new Guid("7d94d9e1-6b2e-488f-b9a3-e85e60ea332b"),
                CreationTime = Clock.Now,
                Basket = basket,
                BasketId = basket.Id,
                WorkingNumberSerial = 1,
                WorkingNumberYear = 1,
                Customer = customer
            };
            worder2.AddOrderedFurnitureUnit(ofu);
            context.WebshopOrders.Add(worder2);
        }

        private static void PopulateUserTeamTypeData(IFPSSalesContext context)
        {
            var installationUserTeamType = new UserTeamType(UserTeamTypeEnum.InstallationGroup) { Id = 10001 };
            var installationUserTeamTypeTranslationEN = new UserTeamTypeTranslation("Installation group", installationUserTeamType.Id, LanguageTypeEnum.EN) { Id = 10002 };
            var installationUserTeamTypeTranslationHU = new UserTeamTypeTranslation("Beszerelőcsapat", installationUserTeamType.Id, LanguageTypeEnum.HU) { Id = 10003 };

            context.UserTeamTypeTranslations.Add(installationUserTeamTypeTranslationEN);
            context.UserTeamTypeTranslations.Add(installationUserTeamTypeTranslationHU);
            context.UserTeamTypes.Add(installationUserTeamType);

            var surveyUserTeamType = new UserTeamType(UserTeamTypeEnum.SurveyGroup) { Id = 10002 };
            var surveyUserTeamTypeTranslationEN = new UserTeamTypeTranslation("Survey group", surveyUserTeamType.Id, LanguageTypeEnum.EN) { Id = 10004 };
            var surveyUserTeamTypeTranslationHU = new UserTeamTypeTranslation("Helyszíni felmérőcsapat", surveyUserTeamType.Id, LanguageTypeEnum.HU) { Id = 10005 };

            context.UserTeamTypeTranslations.Add(surveyUserTeamTypeTranslationEN);
            context.UserTeamTypeTranslations.Add(surveyUserTeamTypeTranslationHU);
            context.UserTeamTypes.Add(surveyUserTeamType);
        }
    }
}