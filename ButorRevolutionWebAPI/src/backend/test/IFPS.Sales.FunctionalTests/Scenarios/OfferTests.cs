using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
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

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    /*
     These tests check an offer by an order (indeed it is OrderTest instead of OfferTest)
         */

    public class OfferTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public OfferTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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

        [Fact]
        public async Task Save_offer_by_general_informations_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var offerCreateDto = BuildOffer();

            var content = new StringContent(JsonConvert.SerializeObject(offerCreateDto), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"api/orders/{new Guid("FBBA3CE4-F622-4500-9206-AE8BA8AA6CE7")}/offers", content);
            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Appliance_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            var applianceCreateDto = BuildOrderedAppliance(id: 10708, quantity: 20);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                //Act
                await service.GetApplianceAsync(new Guid("AFFC1AC6-CE3E-4ACF-9D26-CE07F7DD133B"), applianceCreateDto.Id);

                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("AFFC1AC6-CE3E-4ACF-9D26-CE07F7DD133B"), x => x.OrderedApplianceMaterials);
                //Assert
                modifiedOrder.OrderedApplianceMaterials.Count().Should().Be(1);
                modifiedOrder.OrderedApplianceMaterials.First().Quantity.Should().Be(20);
            }
        }

        [Fact]
        public async Task Get_Appliance_by_companyId_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            var applialnceByCompanyId = BuildApplianceWithCompanyId();

            PagedList<Order> pagedList = new PagedList<Order>()
            {
                Items = new List<Order> { applialnceByCompanyId },
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var expectedResult = pagedList.ToPagedList(OrderFinanceListDto.FromEntity);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                //Act
                var ordersByCompanyId = await service.GetOrdersByCompanyAsync(10700, new OrderFinanceFilterDto { OrderId = applialnceByCompanyId.OrderName });

                //Assert
                // The next two lines: we must compare just the dates beacuse due to domain events there are some seconds delay
                expectedResult.Data.First().StatusDeadline = expectedResult.Data.First().StatusDeadline.Value.Date;
                ordersByCompanyId.Data.First().StatusDeadline = ordersByCompanyId.Data.First().StatusDeadline.Value.Date;

                ordersByCompanyId.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task Put_Orderpayment_by_orderId_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var orderFinanceDto = new OrderFinanceCreateDto()
            {
                PaymentDate = new DateTime(2020, 1, 1),
                PaymentIndex = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(orderFinanceDto), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/orders/62e0c6e7-9568-414b-ad67-f17f70978dbe/finances", content);

            //Assert
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Save_Appliance_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            var applianceCreateDto = BuildAppliance(new Guid("C898F5E9-78C9-4A9A-9A06-F444113221D1"), 10);
            var content = new StringContent(JsonConvert.SerializeObject(applianceCreateDto), Encoding.UTF8, "application/json");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                //Act
                await service.AddApplianceAsync(new Guid("91547633-BD5A-491E-B712-6056E1771D91"), applianceCreateDto);

                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("91547633-BD5A-491E-B712-6056E1771D91"), x => x.OrderedApplianceMaterials);
                //Assert
                modifiedOrder.OrderedApplianceMaterials.Count().Should().Be(2);
                modifiedOrder.OrderedApplianceMaterials.First().Quantity.Should().Be(10);
            }
        }

        [Fact]
        public async Task Delete_appliance_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                //Act
                await service.DeleteApplianceFromAplliancesListAsync(new Guid("CE108CCC-04ED-4930-8FB2-2700478312D3"), 10707);

                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("CE108CCC-04ED-4930-8FB2-2700478312D3"), x => x.OrderedApplianceMaterials);
                //Assert
                //TODO: assert deletion success
                modifiedOrder.OrderedApplianceMaterials.Count().Should().Be(0);
                modifiedOrder.OrderedApplianceMaterials.Where(x => x.Id == 10707).Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public async Task Get_orderedFurnitureUnit_by_order_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var order = BuildOrder();
            var furnitureUnit = BuildFurnitureUnit();
            furnitureUnit.AddFurnitureComponent(BuildThirdFurnitureComponent());
            furnitureUnit.AddFurnitureComponent(BuildFourthFurnitureComponent());

            var orderedFurnitureUnit = new OrderedFurnitureUnit(order.Id, furnitureUnit.Id, 9)
            {
                Id = 10704,
                Order = order,
                FurnitureUnit = furnitureUnit
            };

            order.AddOrderedFurnitureUnit(orderedFurnitureUnit);

            var expectedResult = new FurnitureUnitDetailsByOfferDto(orderedFurnitureUnit);
            // Act
            var response = await client.GetAsync($"api/orders/{order.Id}/offers/orderedfurnitureunits/10704");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task Update_order_delete_orderedFurnitureUnit_from_list_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var offerDetailsDto = BuildOfferDetailsForDeleteOrderedFurnitureUnit();

            //Act
            var response = await client.DeleteAsync($"api/orders/{new Guid("AF02841C-F7EA-408B-9573-59965562AAB8")}/offers/orderedfurnitureunits/{new Guid("205DE084-7A56-4C2B-881C-26859B5BFF7D")}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                var modifiedOrder = await service.GetFurnitureUnitsListByOfferAsync(new Guid("AF02841C-F7EA-408B-9573-59965562AAB8"));
                //Assert
                offerDetailsDto.Products.TallCabinets.Count().Should().Be(modifiedOrder.Products.TallCabinets.Count());
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_order_add_orderedFurnitureUnit_to_list_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            Guid orderId = new Guid("10CB8429-4034-4E39-833C-20CA74488166");

            var fuCreateDto = new FurnitureUnitCreateByOfferDto() { FurnitureUnitId = new Guid("53EEEB26-5EB4-487E-9F69-89211FB59E48"), Quantity = 5 };

            var content = new StringContent(JsonConvert.SerializeObject(fuCreateDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"api/orders/{orderId}/offers/orderedfurnitureunits", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == orderId, x => x.OrderedFurnitureUnits);
                //Assert
                modifiedOrder.OrderedFurnitureUnits.Count().Should().Be(1);
                modifiedOrder.OrderedFurnitureUnits.First().Quantity.Should().Be(5);
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_order_update_orderedFurnitureUnit_with_quantity_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var updateOFUWithQuantityDto = new UpdateOrderedFurnitureUnitQuantityByOfferDto() { Quantity = 11 };

            var content = new StringContent(JsonConvert.SerializeObject(updateOFUWithQuantityDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"api/orders/{new Guid("f9057613-1520-4063-b78a-e6bac33b9c57")}/offers/orderedfurnitureunits/10703", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("f9057613-1520-4063-b78a-e6bac33b9c57"), x => x.OrderedFurnitureUnits);
                //Assert
                modifiedOrder.OrderedFurnitureUnits.First().Quantity.Should().Be(11);
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_order_add_appliance_to_list_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var applianceCreateDto = new ApplianceCreateByOfferDto() { ApplianceMaterialId = new Guid("D0D94BA5-E70C-45FD-BD15-4E9095B12DAE"), Quantity = 8 };
            var content = new StringContent(JsonConvert.SerializeObject(applianceCreateDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"api/orders/{new Guid("07BE975F-F2B8-451B-A99B-3E97C146F067")}/offers/appliances", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("07BE975F-F2B8-451B-A99B-3E97C146F067"), x => x.OrderedApplianceMaterials);
                //Assert
                modifiedOrder.OrderedApplianceMaterials.Count().Should().Be(1);
                modifiedOrder.OrderedApplianceMaterials.First().Quantity.Should().Be(8);
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_appliance_by_order_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var order = new Order("Get appliance with quantity", BuildCustomer().Id, BuildSalesPerson().Id, new DateTime(2020, 1, 31), new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() })
            {
                Id = new Guid("135d6a97-1b25-4e3b-827e-a3a02d7b63db"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                WorkingNumberSerial = 444,
                WorkingNumberYear = 3215,
                DescriptionByOffer = "Her name was Lola, she was a showgirl"
            };

            var applianceMaterial = new ApplianceMaterial()
            {
                Id = new Guid("805e17d5-c617-4a11-bf2f-3a0c6ca0fe1e"),
                Code = "App-Mat-003",
                SellPrice = new Price(1000.0, 1),
                BrandId = BuildCompany().Id,
                Brand = BuildCompany()
            };

            var orderedApplianceMaterial = new OrderedApplianceMaterial()
            {
                Id = 10704,
                OrderId = order.Id,
                Order = order,
                ApplianceMaterialId = applianceMaterial.Id,
                ApplianceMaterial = applianceMaterial,
                Quantity = 14
            };

            order.AddAppliance(orderedApplianceMaterial);

            var expectedResult = new ApplianceDetailsByOfferDto(orderedApplianceMaterial);
            // Act
            var response = await client.GetAsync($"api/orders/{order.Id}/offers/appliances/10704");
            // Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task Update_order_update_appliance_with_quantity_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var applianceUpdateByOfferDto = new ApplianceUpdateByOfferDto() { Quantity = 20 };

            var content = new StringContent(JsonConvert.SerializeObject(applianceUpdateByOfferDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"api/orders/{new Guid("B931B0BC-08DD-47B6-A17F-7FC70D5CEDED")}/offers/appliances/10702", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("B931B0BC-08DD-47B6-A17F-7FC70D5CEDED"), x => x.OrderedApplianceMaterials);
                //Assert
                modifiedOrder.OrderedApplianceMaterials.First().Quantity.Should().Be(20);
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_order_delete_appliance_from_list_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var offerDetailsDto = BuildOfferDetailsForDeleteAppliance();

            //Act
            var response = await client.DeleteAsync($"api/orders/{new Guid("EF34A34F-A2DB-4A29-A847-9775640D68DC")}/offers/appliances/10703");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("EF34A34F-A2DB-4A29-A847-9775640D68DC"), x => x.OrderedApplianceMaterials);
                //Assert
                modifiedOrder.OrderedApplianceMaterials.Count().Should().Be(0);
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_offer_preview_by_order_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var order = new Order("Offer Preview", BuildCustomer().Id, BuildSalesPerson().Id, new DateTime(2020, 1, 31), new Price(0.0, 10900) { Currency = BuildCurrency() })
            {
                Id = new Guid("8A2333ED-9A68-4769-86CB-7028DA7BC7AC"),
                OrderName = "Unicorn Offer Test Name",
                CurrentTicketId = 1,
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                WorkingNumberSerial = 666,
                WorkingNumberYear = 3216,
                ShippingAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1),
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                OfferInformation = BuildOfferInformation(10000),
                OfferInformationId = BuildOfferInformation(10000).Id
            };

            var firstFU = new FurnitureUnit("Preview-FU-188", 75, 120, 50)
            {
                Id = new Guid("4BB65C4F-EAE3-422F-8CE8-1D2F4336D501"),
                Description = "preview vol.1",
                FurnitureUnitType = BuildTallType(),
                FurnitureUnitTypeId = BuildTallType().Id,
                ImageId = BuildImage().Id,
                Image = BuildImage(),
                CurrentPriceId = BuildFirstFurniturUnitPrice().Id,
                CurrentPrice = BuildFirstFurniturUnitPrice()
            };

            var secondFU = new FurnitureUnit("Preview-FU-230", 75, 120, 5)
            {
                Id = new Guid("93FF53F8-194C-45C4-94AD-2F1428F9BD2B"),
                Description = "preview vol.2",
                FurnitureUnitType = BuildTopType(),
                FurnitureUnitTypeId = BuildTopType().Id,
                ImageId = BuildImage().Id,
                Image = BuildImage(),
                CurrentPriceId = BuildFirstFurniturUnitPrice().Id,
                CurrentPrice = BuildFirstFurniturUnitPrice()
            };

            var firstOFU = new OrderedFurnitureUnit(order.Id, firstFU.Id, 12)
            {
                Id = 10705,
                FurnitureUnit = firstFU,
                Order = order
            };

            var secondOFU = new OrderedFurnitureUnit(order.Id, secondFU.Id, 7)
            {
                Id = 10706,
                FurnitureUnit = secondFU,
                Order = order
            };

            order.AddOrderedFurnitureUnit(firstOFU);
            order.AddOrderedFurnitureUnit(secondOFU);

            var applianceMaterial = new ApplianceMaterial()
            {
                Id = new Guid("04CD23D7-3EC1-4354-9512-C4C239F47260"),
                Code = "App-Mat-042",
                SellPrice = new Price(1000.0, BuildCurrency().Id) { Currency = BuildCurrency() },
                BrandId = BuildCompany().Id,
                Brand = BuildCompany(),
                ImageId = BuildImage().Id,
                Image = BuildImage()
            };

            var orderedApplianceMaterial = new OrderedApplianceMaterial()
            {
                Id = 10705,
                OrderId = order.Id,
                Order = order,
                ApplianceMaterialId = applianceMaterial.Id,
                ApplianceMaterial = applianceMaterial,
                Quantity = 21
            };

            order.AddAppliance(orderedApplianceMaterial);

            var expectedResult = new OfferPreviewDto(order);
            // Act
            var response = await client.GetAsync($"api/orders/{order.Id}/offers/previews");
            // Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        //[Fact]
        //public async Task Create_new_furniture_unit_with_base_furniture_unit_works()
        //{
        //    // Arrange
        //    var client = factory.CreateClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

        //    var content = new StringContent(JsonConvert.SerializeObject(BuildNewFurnitureUnit()), Encoding.UTF8, "application/json");
        //    // Act
        //    var response = await client.PostAsync($"api/orders/{new Guid("5d007976-d454-4b31-a757-2cbef1ddf7e9")}/offers/furnitureunits/{new Guid("74564600-8BD1-4E67-82CC-ED510C974A82")}", content);
        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    var stringresp = await response.Content.ReadAsAsync(typeof(Guid));
        //    stringresp.Should().BeOfType(typeof(Guid));
        //}

        [Fact]
        public async Task Add_service_to_new_order_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var serviceCreateByOfferDto = new ServiceCreateByOfferDto() { IsAdded = true, ServiceType = ServiceTypeEnum.Assembly };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
                //Act
                await service.AddServiceAsync(new Guid("6E197CE1-42B4-4EDB-887E-813A43BB2374"), serviceCreateByOfferDto);

                var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var modifiedOrder = await repository.SingleIncludingAsync(ent => ent.Id == new Guid("6E197CE1-42B4-4EDB-887E-813A43BB2374"), x => x.Services);
                //Assert
                modifiedOrder.Services.Count().Should().BeGreaterOrEqualTo(1);
            }
        }

        #region BuildEntities

        private FurnitureUnitType BuildTopType()
        {
            var topType = new FurnitureUnitType(FurnitureUnitTypeEnum.Top) { Id = 10400 };
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Top", LanguageTypeEnum.EN) { Id = 10200 });
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Felső", LanguageTypeEnum.HU) { Id = 10210 });
            return topType;
        }

        private FurnitureUnitType BuildTallType()
        {
            var tallType = new FurnitureUnitType(FurnitureUnitTypeEnum.Tall) { Id = 10410 };
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Tall", LanguageTypeEnum.EN) { Id = 10220 });
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Magas", LanguageTypeEnum.HU) { Id = 10230 });
            return tallType;
        }

        private FurnitureUnitType BuildBaseType()
        {
            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10420 };
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10240 });
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10250 });
            return baseType;
        }

        private ServiceCreateByOfferDto BuildService(List<int> ids) => new ServiceCreateByOfferDto { /*Ids = ids*/ };

        private UserData BuildUserData()
        {
            return new UserData("John Doe", "45348976", Clock.Now) { Id = 10720, ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1) };
        }

        private Company BuildCompany()
        {
            var firstCompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10700 };
            var thirdUser = new User("contact@contact.de", LanguageTypeEnum.HU) { Id = 10702, CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
            var companyData = new CompanyData("Tax-150", "Reg-762", new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), null, Clock.Now) { Id = 10780, ContactPerson = thirdUser, ContactPersonId = thirdUser.Id };
            return new Company("Bayerische Motor Werk", firstCompanyType.Id) { Id = 10700, CurrentVersionId = companyData.Id, CurrentVersion = companyData };
        }

        private User BuildFirstUserForCustomer()
        {
            return new User("kunden@kunden.de", LanguageTypeEnum.HU) { Id = 10700, CompanyId = BuildCompany().Id, Company = BuildCompany(), CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
        }

        private User BuildSecondUserForSalesPerson()
        {
            return new User("sales@sales.de", LanguageTypeEnum.HU) { Id = 10701, CompanyId = BuildCompany().Id, Company = BuildCompany(), CurrentVersionId = BuildUserData().Id, CurrentVersion = BuildUserData() };
        }

        private OfferInformation BuildOfferInformation(int id)
        {
            return new OfferInformation()
            {
                Id = id,
                ProductsPrice = new Price(30000.0, BuildCurrency().Id) { Currency = BuildCurrency() },
                ServicesPrice = new Price(25000.0, BuildCurrency().Id) { Currency = BuildCurrency() }
            };
        }

        private Customer BuildCustomer()
        {
            return new Customer(BuildFirstUserForCustomer().Id, Clock.Now) { Id = 10700, User = BuildFirstUserForCustomer() };
        }

        private SalesPerson BuildSalesPerson()
        {
            return new SalesPerson(BuildSecondUserForSalesPerson().Id, Clock.Now) { Id = 10700, User = BuildSecondUserForSalesPerson() };
        }

        private Currency BuildCurrency()
        {
            return new Currency("HUF") { Id = 10900 };
        }

        private FoilMaterial BuildFoilMaterial()
        {
            return new FoilMaterial() { Id = new Guid("d2cbcc5e-2136-432f-98c2-b37553efc555"), Description = "Foil unicorn" };
        }

        private GroupingCategory BuildCategory()
        {
            var category = new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) { Id = 10700 };

            category.AddTranslation(new GroupingCategoryTranslation("Unicorn", LanguageTypeEnum.EN) { Id = 10700, CoreId = 10700, Core = category });
            category.AddTranslation(new GroupingCategoryTranslation("Unikornis", LanguageTypeEnum.HU) { Id = 10701, CoreId = 10700, Core = category });

            return category;
        }

        private Image BuildImage()
        {
            return new Image("image", ".jpg", "folder") { Id = new Guid("864dd88b-fc83-43a9-99f9-6e6b1951d779") };
        }

        private FurnitureUnitPrice BuildFirstFurniturUnitPrice()
        {
            return new FurnitureUnitPrice() { Id = 10600, Price = new Price(12000.0, BuildCurrency().Id) { Currency = BuildCurrency() }, MaterialCost = new Price(12000.0, BuildCurrency().Id) { Currency = BuildCurrency() } };
        }

        private OfferDetailsDto BuildOfferDetailsForDeleteAppliance()
        {
            var order = new Order("Delete from appliance list", BuildCustomer().Id, BuildSalesPerson().Id, new DateTime(2020, 1, 31), new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() })
            {
                Id = new Guid("EF34A34F-A2DB-4A29-A847-9775640D68DC"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                WorkingNumberSerial = 123,
                WorkingNumberYear = 3214,
                DescriptionByOffer = "Her name was Lola, she was a showgirl",
                OfferInformation = BuildOfferInformation(10000)
            };

            var thirdApplianceMaterial = BuildApplianceMaterial();

            var fourthOrderedApplianceMaterial = new OrderedApplianceMaterial()
            {
                Id = 10703,
                OrderId = order.Id,
                Order = order,
                ApplianceMaterialId = thirdApplianceMaterial.Id,
                ApplianceMaterial = thirdApplianceMaterial,
                Quantity = 10
            };
            order.AddAppliance(fourthOrderedApplianceMaterial);

            return new OfferDetailsDto(order, 0.0, 0.0);
        }

        private OfferDetailsDto BuildOfferDetailsForDeleteOrderedFurnitureUnit()
        {
            var order = new Order("Bestellung für Löschung", BuildCustomer().Id, BuildSalesPerson().Id, new DateTime(2020, 1, 31), new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() })
            {
                Id = new Guid("AF02841C-F7EA-408B-9573-59965562AAB8"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                WorkingNumberSerial = 2642,
                WorkingNumberYear = 2027,
                DescriptionByOffer = "Her nam was Lola, she was a showgirl",
                OfferInformation = BuildOfferInformation(10000)
            };

            var thirdFurnitureUnit = new FurnitureUnit("Delete-FU-000", 75, 120, 50)
            {
                Id = new Guid("205DE084-7A56-4C2B-881C-26859B5BFF7D"),
                Description = "DELETE",
                FurnitureUnitType = BuildTallType(),
                FurnitureUnitTypeId = BuildTallType().Id,
                CurrentPriceId = BuildFirstFurniturUnitPrice().Id,
                CurrentPrice = BuildFirstFurniturUnitPrice(),
                ImageId = BuildImage().Id,
                Image = BuildImage()
            };

            var fifthFurnitureComponent = new FurnitureComponent("Test-FC-2000", 2) { Id = new Guid("769F31C3-AAA5-4857-8A51-9196532551D4"), FurnitureUnitId = thirdFurnitureUnit.Id, FurnitureUnit = thirdFurnitureUnit, ImageId = BuildImage().Id, Image = BuildImage() };
            thirdFurnitureUnit.AddFurnitureComponent(fifthFurnitureComponent);

            var thirdOrderedFurnitureUnit = new OrderedFurnitureUnit(order.Id, thirdFurnitureUnit.Id, 1) { Id = 10702, Order = order, FurnitureUnit = thirdFurnitureUnit };
            order.AddOrderedFurnitureUnit(thirdOrderedFurnitureUnit);

            var offerDetailsDto = new OfferDetailsDto(order, 0.0, 0.0);
            return offerDetailsDto;
        }

        private FurnitureUnitCreateWithQuantityByOfferDto BuildNewFurnitureUnit()
        {
            var createFU = new FurnitureUnitCreateWithQuantityByOfferDto()
            {
                Code = "Modified-FU-Code_435",
                Description = "Rainbow, you magnificent",
                CategoryId = BuildCategory().Id,
                Height = 135,
                Width = 69,
                Depth = 34,
                Quantity = 9
            };

            var front = BuildNewFrontFC();
            createFU.Fronts = new List<FurnitureComponentsCreateByOfferDto>();
            createFU.Fronts.Add(front);
            createFU.Corpuses = new List<FurnitureComponentsCreateByOfferDto>();
            createFU.Corpuses.Add(BuildNewCorpusFC());

            return createFU;
        }

        private FurnitureComponentsCreateByOfferDto BuildNewFrontFC()
        {
            var createFC = new FurnitureComponentsCreateByOfferDto()
            {
                Id = new Guid("948B8E46-B039-4178-895D-67156108FE61"),
                Name = "Front Name",
                Height = 200,
                Width = 150,
                Amount = 5
            };

            return createFC;
        }

        private ApplianceMaterial BuildApplianceMaterial()
        {
            return new ApplianceMaterial()
            {
                Id = new Guid("805e17d5-c617-4a11-bf2f-3a0c6ca0fe1e"),
                Code = "App-Mat-003",
                SellPrice = new Price(1000.0, 1),
                BrandId = BuildCompany().Id,
                Brand = BuildCompany()
            };
        }

        private FurnitureComponentsCreateByOfferDto BuildNewCorpusFC()
        {
            var createFC = new FurnitureComponentsCreateByOfferDto()
            {
                Id = new Guid("56A1EB6F-3B04-4928-8551-53677639C6A8"),
                Name = "Corpus Name",
                Height = 220,
                Width = 110,
                Amount = 14
            };

            return createFC;
        }     

        private OrderPrice BuildPayment(int id, double price)
        {
            return new OrderPrice() { Id = id, Deadline = DateTime.MinValue, Price = new Price(price, BuildCurrency().Id) { Currency = BuildCurrency() } };
        }
        private OfferCreateDto BuildOffer()
        {
            var offerCreateDto = new OfferCreateDto();
            offerCreateDto.Requires = new RequiresCreateDto()
            {
                IsPrivatePerson = false,
                Budget = new PriceCreateDto() { Value = (decimal)0.0, CurrencyId = BuildCurrency().Id },
                Description = "Her name was Lola, she was a showgirl"
            };

            offerCreateDto.TopCabinet = new CabinetMaterialCreateDto
            {
                Height = 197,
                Width = 87,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Ich geh' heut' nicht mehr tanzen"
            };

            offerCreateDto.BaseCabinet = new CabinetMaterialCreateDto
            {
                Height = 200,
                Width = 99,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Die Welt geht runter"
            };

            offerCreateDto.TallCabinet = new CabinetMaterialCreateDto
            {
                Height = 200,
                Width = 87,
                BackPanelMaterialId = 10000,
                DoorMaterialId = 10000,
                InnerMaterialId = 10000,
                OuterMaterialId = 10000,
                Description = "Ich liebe auto fahre"
            };

            return offerCreateDto;
        }

        private ApplianceCreateByOfferDto BuildAppliance(Guid id, int quantity) => new ApplianceCreateByOfferDto
        {
            ApplianceMaterialId = id,
            Quantity = quantity
        };

        private OrderedApplianceMaterial BuildOrderedAppliance(int id, int quantity) => new OrderedApplianceMaterial
        {
            Id = id,
            Quantity = quantity
        };

        private Order BuildApplianceWithCompanyId()
        {
            var userData = new UserData("John Doe", "45348976", Clock.Now)
            {
                Id = 10720,
                ContactAddress = new Address(1357, "Budapest", "Kossuth tér 1-3.", 1)
            };

            var thirdUser = new User("contact@contact.de", LanguageTypeEnum.HU) { Id = 10702, CurrentVersionId = userData.Id, CurrentVersion = userData };

            var companyData = new CompanyData("Tax-150", "Reg-762", new Address(1357, "Budapest", "Kossuth tér 1-3.", 1), null, Clock.Now)
            {
                Id = 10780,
                ContactPersonId = thirdUser.Id,
                ContactPerson = thirdUser
            };

            var firstCompanyType = new CompanyType(CompanyTypeEnum.MyCompany) { Id = 10700 };

            var firstCompany = new Company("Bayerische Motor Werk", firstCompanyType.Id)
            {
                Id = 10700,
                CurrentVersionId = companyData.Id,
                CurrentVersion = companyData
            };

            var firstUser = new User("kunden@kunden.de", LanguageTypeEnum.HU)
            {
                Id = 10700,
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

            var firstSales = new SalesPerson(secondUser.Id, Clock.Now) { Id = 10700, User = firstUser };
            var firstCustomer = new Customer(firstUser.Id, Clock.Now) { Id = 10700, User = firstUser };
            var orderPrice = new OrderPrice() { Id = 10000 };
            var orderPrice2 = new OrderPrice() { Id = 10001 };
            var currency = new Currency("HUF") { Id = 10900 };

            var orderState_waitingForDocuments = new OrderState(OrderStateEnum.WaitingForOffer);
            var currentTicket = new Ticket(orderState_waitingForDocuments, firstUser, 2) { Id = 10000 };

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

            var firstApplianceMaterial = new ApplianceMaterial
            {
                Id = new Guid("D0D94BA5-E70C-45FD-BD15-4E9095B12DAE"),
                Code = "App-Mat-001",
                SellPrice = new Price(1000.0, 1),
                BrandId = firstCompany.Id,
                Brand = firstCompany
            };

            var firstOrderedApplianceMaterial = new OrderedApplianceMaterial
            {
                Id = 10700,
                OrderId = new Guid("FBBA3CE4-F622-4500-9206-AE8BA8AA6CE7"),
                Order = firstOrder,
                ApplianceMaterialId = firstApplianceMaterial.Id,
                ApplianceMaterial = firstApplianceMaterial
            };

            firstOrder.AddAppliance(firstOrderedApplianceMaterial);
            return firstOrder;
        }


        private FurnitureComponent BuildFourthFurnitureComponent()
        {
            return new FurnitureComponent("Test-FC-2000", 2)
            {
                Id = new Guid("95b7887f-65f9-4b91-a676-62e1f8bbbdce"),
                Type = FurnitureComponentTypeEnum.Corpus,
                FurnitureUnitId = BuildFurnitureUnit().Id,
                FurnitureUnit = BuildFurnitureUnit(),
                ImageId = BuildImage().Id,
                Image = BuildImage(),
                BottomFoilId = BuildFoilMaterial().Id,
                BottomFoil = BuildFoilMaterial(),
                TopFoilId = BuildFoilMaterial().Id,
                TopFoil = BuildFoilMaterial(),
                RightFoilId = BuildFoilMaterial().Id,
                RightFoil = BuildFoilMaterial(),
                LeftFoilId = BuildFoilMaterial().Id,
                LeftFoil = BuildFoilMaterial()
            };
        }

        private FurnitureUnit BuildFurnitureUnit()
        {
            return new FurnitureUnit("Test-FU-880", 75, 120, 50)
            {
                Id = new Guid("f2ace9bf-e9b7-42e6-91fe-75387690708b"),
                Description = "Unicorn Strong Power",
                FurnitureUnitType = BuildTallType(),
                FurnitureUnitTypeId = BuildTallType().Id,
                ImageId = BuildImage().Id,
                Image = BuildImage(),
                CurrentPriceId = BuildFirstFurniturUnitPrice().Id,
                CurrentPrice = BuildFirstFurniturUnitPrice(),
                CategoryId = BuildCategory().Id,
                Category = BuildCategory()
            };
        }

        private FurnitureComponent BuildThirdFurnitureComponent()
        {
            return new FurnitureComponent("Test-FC-5067", 2)
            {
                Id = new Guid("286f4c7b-f4e4-4337-96e8-7b079fdcf578"),
                Type = FurnitureComponentTypeEnum.Front,
                FurnitureUnitId = BuildFurnitureUnit().Id,
                FurnitureUnit = BuildFurnitureUnit(),
                ImageId = BuildImage().Id,
                Image = BuildImage(),
                BottomFoilId = BuildFoilMaterial().Id,
                BottomFoil = BuildFoilMaterial(),
                TopFoilId = BuildFoilMaterial().Id,
                TopFoil = BuildFoilMaterial(),
                RightFoilId = BuildFoilMaterial().Id,
                RightFoil = BuildFoilMaterial(),
                LeftFoilId = BuildFoilMaterial().Id,
                LeftFoil = BuildFoilMaterial()
            };
        }

        private Order BuildOrder()
        {
            return new Order("Get orderedFurnitureUnit", BuildCustomer().Id, BuildSalesPerson().Id, new DateTime(2020, 1, 31), new Price(0.0, BuildCurrency().Id) { Currency = BuildCurrency() })
            {
                Id = new Guid("eae1e5f6-21ea-49cf-a068-3e56a475e89b"),
                Customer = BuildCustomer(),
                SalesPerson = BuildSalesPerson(),
                WorkingNumberSerial = 555,
                WorkingNumberYear = 3216,
                DescriptionByOffer = "Her name was Lola, she was a showgirl"
            };
        }

        #endregion
    }
}