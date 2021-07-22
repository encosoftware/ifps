using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class CargoTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public CargoTests(IFPSFactoryWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private async Task<string> getAccessToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "enco@enco.hu",
                Password = "password",
                RememberMe = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var client = factory.CreateClient();
            var resp = await client.PostAsync("api/account/login/", content);
            var stringresp = await resp.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenDto>(stringresp);
            return model.AccessToken;
        }

        //private

        [Fact]
        public async Task Create_cargo_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var cargo = new CargoCreateDto
            {
                CargoName = "New Cargo",
                BookedById = 10000,
                SupplierId = 10001,
                Notes = "I am so happy when the tests are green",
                ShippingAddress = new AddressCreateDto()
                {
                    Address = "Sasadi út 13.",
                    City = "Budapest",
                    PostCode = 1118,
                    CountryId = 1
                },
                ShippingCost = new PriceCreateDto()
                {
                    Value = 4500.0,
                    CurrencyId = 1
                },
                //NetCost = new PriceCreateDto()
                //{
                //    Value = 5500.0,
                //    CurrencyId = 1
                //},
                Additionals = new List<OrderedMaterialPackageCreateDto>() { },
                Materials = new List<OrderedMaterialPackageCreateDto>()
                {
                    new OrderedMaterialPackageCreateDto()
                    {
                        OrderedPackageNum = 20,
                        PackageId = 10000
                    }
                }

            };
            
            var content = new StringContent(JsonConvert.SerializeObject(cargo), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/cargos", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_cargo_details_preview_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var contact = new User("contact@enco.test.hu", LanguageTypeEnum.HU) { Id = 10004 };
            contact.AddVersion(new UserData("Mihail Bulgakov", "+875345434", Clock.Now, null) { Id = 10002 });

            var companyData = new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), contact.Id, Clock.Now) { Id = 10000, ContactPerson = contact };
            var supplier = new Company("Test Super Supplier Company", 3) { Id = 10001, CurrentVersion = companyData, CurrentVersionId = 10000 };

            var status = new CargoStatusType(CargoStatusEnum.Ordered) { Id = 10000 };
            status.AddTranslation(new CargoStatusTypeTranslation(10000, "Megrendelve", LanguageTypeEnum.HU));
            status.AddTranslation(new CargoStatusTypeTranslation(10000, "Ordered", LanguageTypeEnum.EN));

            var ownCompany = new Company("Test EN-CO Software", 1) { Id = 10000, CurrentVersion = companyData, CurrentVersionId = 10000 };

            var bookedBy = new User("zelda@göttingsberg.com", LanguageTypeEnum.EN) { Id = 10001, Company = ownCompany, CompanyId = ownCompany.Id };
            bookedBy.AddVersion(new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now, new Address(1117, "Budapest", "Baranyai utca 7.", 1)));

            var currency = new Currency("HUF") { Id = 1 };

            var orderedPackages = new List<OrderedMaterialPackageListDto>();

            var cargo = new Cargo("Super Test Cargo Name", supplier.Id, status.Id, bookedBy.Id, new Price(10000.0, currency.Id) { Currency = currency }, new Price(1000.0, currency.Id) { Currency = currency }, new Address(1212, "Budapest", "Kuruclesi út 4.", 1), new Price(900.0, currency.Id) { Currency = currency }, "Tja!", new DateTime(2019, 8, 31), null, null)
            {
                Id = 10001,
                Supplier = supplier,
                Status = status,
                BookedBy = bookedBy 
            };

            var material = new Material("DEC") { Id = new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), Description = "beautiful dec" };

            var materialPackage = new MaterialPackage(material.Id, supplier.Id)
            {
                Material = material,
                Price = new Price(8000.0, currency.Id) { Currency = currency },
                PackageCode= "COD-748",
                Size = 1,
                PackageDescription = "Stuff",
            };

            var orderedPackage = new OrderedMaterialPackage()
            {
                MaterialPackage = materialPackage,
                MaterialPackageId = materialPackage.Id,
                OrderedAmount = 15,
                UnitPrice = new Price(9999.0, currency.Id) { Currency = currency }
            };

            cargo.AddOrderedPackage(orderedPackage);

            var expectedResult = new CargoDetailsPreviewDto(cargo);
            // Act
            var resp = await client.GetAsync("api/cargos/10001/preview");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        
        [Fact]
        public async Task Get_cargo_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var supplier = new Company("Test Super Supplier Company", 3) { Id = 10001 };

            var status = new CargoStatusType(CargoStatusEnum.Ordered) { Id = 10000 };
            status.AddTranslation(new CargoStatusTypeTranslation(10000, "Megrendelve", LanguageTypeEnum.HU));
            status.AddTranslation(new CargoStatusTypeTranslation(10000, "Ordered", LanguageTypeEnum.EN));

            var ownCompany = new Company("Test EN-CO Software", 1) { Id = 10000 };

            var bookedBy = new User("enco@enco.hu", LanguageTypeEnum.HU) { Id = 10000, Company = ownCompany, CompanyId = ownCompany.Id };
            bookedBy.AddVersion(new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now, new Address(1117, "Budapest", "Baranyai utca 7.", 1)));

            var currency = new Currency("HUF") { Id = 1 };

            var cargo = new Cargo("Test Cargo Name", supplier.Id, status.Id, bookedBy.Id,
                new Price(10000.0, currency.Id) { Currency = currency },
                new Price(1000.0, currency.Id) { Currency = currency },
                new Address(1064, "Budapest", "Szinyei Merse utca 13.", 1),
                new Price(1100.0, currency.Id) { Currency = currency },
                "Es gab eine Überraschung!", new DateTime(2019, 8, 31), null, null)
            { Id = 10000, Supplier = supplier, Status = status, BookedBy = bookedBy };
            var expectedResult = new CargoDetailsDto(cargo);

            // Act
            var resp = await client.GetAsync("api/cargos/10000");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Update_cargo_with_missing_and_refused_amount_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var products = new List<ProductsUpdateDto>()
            {
                new ProductsUpdateDto() { Id = 10004, Missing = 2, Refused = 3 },
                new ProductsUpdateDto() { Id = 10005, Missing = 1, Refused = 4 }
            };
            var cargo = new CargoUpdateDto
            {
                Products = products
            };

            var content = new StringContent(JsonConvert.SerializeObject(cargo), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/cargos/10004", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICargoAppService>();
                var updatedCargo = await service.CargoDetailsAsync(10004);
                // Assert
                foreach (var orderedPackage in updatedCargo.ProductList)
                {
                    var product = cargo.Products.Single(ent => ent.Id == orderedPackage.Id);

                    product.Missing.Should().Be(orderedPackage.Missing);
                    product.Refused.Should().Be(orderedPackage.Refused);
                }
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_cargo_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/cargos/10003");

            // Assert
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_cargos_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<CargoListDto>();

            var currency = new Currency("HUF") { Id = 1 };
            var companyType = new CompanyType(CompanyTypeEnum.SupplierCompany);

            var currentVersion = new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now);
            var statusType = new CargoStatusType(CargoStatusEnum.Ordered) { Id = 10000 };
            var translation = new CargoStatusTypeTranslation(10000, "Megrendelve", LanguageTypeEnum.HU);
            statusType.AddTranslation(translation);

            var cargo = new CargoListDto()
            {
                Id = 10004,
                CargoName = "Super Super Cargo",
                CreatedOn = new DateTime(2019, 10, 31),
                BookedByUser = new CargoUserListDto(new User("zelda@göttingsberg.com", LanguageTypeEnum.EN) { CurrentVersion = currentVersion }) { Id = 10001, Name = currentVersion.Name },
                SupplierName = new SupplierCompanyListDto(new Company("Test Super Supplier Company", 1) { Id = 10001, CompanyType = companyType }),
                Status = new CargoStatusTypeListDto(statusType),
                TotalCost = new Price(999999.0, 1) { Currency = currency }
            };

            expectedResult.Add(cargo);
            IPagedList<CargoListDto> pagedList = new PagedList<CargoListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICargoAppService>();
                var cargoFilterDto = new CargoFilterDto() { CargoName = "Super Super Cargo" };
                var cargos = await service.ListCargos(cargoFilterDto);

                // Assert
                result.Should().BeEquivalentTo(cargos);
            }
        }

        [Fact]
        public async Task Get_cargos_filter_statustype_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICargoAppService>();
                CargoFilterDto cargoFilterDto = new CargoFilterDto() { CargoStatusTypeId = 7 };
                var cargos = await service.ListCargos(cargoFilterDto);

                // Assert
                cargos.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_cargos_filter_cargo_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICargoAppService>();
                CargoFilterDto cargoFilterDto = new CargoFilterDto() { CargoName = "I cannot believe that name of cargo" };
                var cargos = await service.ListCargos(cargoFilterDto);

                // Assert
                cargos.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_cargos_by_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<CargoListByStockDto>();

            var currency = new Currency("HUF") { Id = 1 };
            var companyType = new CompanyType(CompanyTypeEnum.SupplierCompany);

            var currentVersion = new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now);
            var statusType = new CargoStatusType(CargoStatusEnum.WaitingForStocking) { Id = 10001 };
            var translation = new CargoStatusTypeTranslation(10000, "Várakozás raktározásra", LanguageTypeEnum.HU);
            statusType.AddTranslation(translation);

            var cargo = new CargoListByStockDto()
            {
                Id = 10005,
                CargoName = "Super Stock Cargo",
                BookedByUser = new CargoUserListDto(new User("zelda@göttingsberg.com", LanguageTypeEnum.EN) { CurrentVersion = currentVersion }) { Id = 10001, Name = currentVersion.Name },
                SupplierName = new SupplierCompanyListDto(new Company("Test Super Supplier Company", 1) { Id = 10001, CompanyType = companyType }),
                Status = new CargoStatusTypeListDto(statusType),
            };

            expectedResult.Add(cargo);
            IPagedList<CargoListByStockDto> pagedList = new PagedList<CargoListByStockDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICargoAppService>();
                var cargoFilterDto = new CargoFilterDto() { CargoName = "Super Stock Cargo" };
                var cargos = await service.ListCargosByStock(cargoFilterDto);

                // Assert
                result.Should().BeEquivalentTo(cargos);
            }
        }

        [Fact]
        public async Task Get_cargo_details_by_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var contactData = new UserData("Mihail Bulgakov", "+875345434", new DateTime(2019, 12, 6));
            var contact = new User("contact@enco.test.hu", LanguageTypeEnum.HU) { Id = 1, CurrentVersion = contactData, CurrentVersionId = contactData.Id };

            var companyDataForSupplier = new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), 1, new DateTime(2019, 11, 30)) { Id = 10001, ContactPerson = contact, ContactPersonId = contact.Id };
            var supplier = new Company("Test Super Supplier Company", 3) { Id = 10001, CurrentVersion = companyDataForSupplier, CurrentVersionId = companyDataForSupplier.Id };

            var status = new CargoStatusType(CargoStatusEnum.WaitingForStocking) { Id = 10001 };
            status.AddTranslation(new CargoStatusTypeTranslation(10001, "Várakozás raktározásra", LanguageTypeEnum.HU));
            status.AddTranslation(new CargoStatusTypeTranslation(10001, "Waiting for stocking", LanguageTypeEnum.EN));

            var companyData = new CompanyData("1111", "1111", new Address(6344, "Hajós", "Petőfi utca 52.", 1), 1, new DateTime(2019, 11, 30)) { Id = 10000, ContactPerson = contact, ContactPersonId = contact.Id };
            var ownCompany = new Company("Test EN-CO Software", 1) { Id = 10000, CurrentVersion = companyData, CurrentVersionId = companyData.Id };

            var bookedBy = new User("zelda@göttingsberg.com", LanguageTypeEnum.HU) { Id = 10001, Company = ownCompany, CompanyId = ownCompany.Id };
            bookedBy.AddVersion(new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now, new Address(6344, "Hajós", "Petőfi utca 52.", 1)));

            var currency = new Currency("HUF") { Id = 1 };

            var cargo = new Cargo("Excellent Stock Cargo", supplier.Id, status.Id, bookedBy.Id,
                new Price(87345.0, currency.Id) { Currency = currency },
                new Price(87345.0, currency.Id) { Currency = currency },
                new Address(8000, "Székesfehérvár", "III. Béla király tér 5.", 1),
                new Price(87345.0, currency.Id) { Currency = currency },
                "Welcome to Fabulous Stock City!", new DateTime(2019, 12, 31), null, null)
            { Id = 10006, Supplier = supplier, Status = status, BookedBy = bookedBy };
            var expectedResult = new CargoStockDetailsDto(cargo);

            // Act
            var resp = await client.GetAsync("api/cargos/bystock/details/10006");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }
    }
}
