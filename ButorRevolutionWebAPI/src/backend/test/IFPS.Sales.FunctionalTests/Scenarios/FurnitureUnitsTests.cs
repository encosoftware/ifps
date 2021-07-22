using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class FurnitureUnitsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public FurnitureUnitsTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            CultureInfo.CurrentCulture = new CultureInfo("hu-HU");
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
        public async Task Get_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureUnit = new FurnitureUnit("HA123457", 2, 2, 2)
            {
                Id = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952"),
                Description = "Description",
                CategoryId = 10000,
                Image = new Image("furnitureunit.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_furnitureunit.jpg" }
            };
            furnitureUnit.AddPrice(new FurnitureUnitPrice() { CoreId = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952"), Price = new Price(10, 1), MaterialCost = new Price(10, 1) });
            var expectedResult = new FurnitureUnitDetailsDto(furnitureUnit);

            // Act
            var resp = await client.GetAsync($"api/furnitureunits/{new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_furniture_units_works()
        {
            // Arrange
            factory.CreateClient();

            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Teszt alapanyagok", LanguageTypeEnum.HU));

            var furnitureUnit = new FurnitureUnit("HA123457", 2, 2, 2)
            {
                Id = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952"),
                Description = "Description",
                CategoryId = 10000,
                Category = category,
                Image = new Image("furnitureunit.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("e999e4fa-69b9-4633-98c8-f8e58a48e195"), ThumbnailName = "thumbnail_furnitureunit.jpg" }
            };
            furnitureUnit.AddPrice(new FurnitureUnitPrice() { CoreId = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952"), Price = new Price(10, 10870) { Currency = new Currency("HUF") }, MaterialCost = new Price(10, 10870) { Currency = new Currency("HUF") } });
            var expectedResult = new List<FurnitureUnit>
            {
                furnitureUnit
            };

            PagedList<FurnitureUnit> pagedList = new PagedList<FurnitureUnit>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(FurnitureUnitListDto.FromEntity);
            FurnitureUnitFilterDto furnitureUnitFilterDto = new FurnitureUnitFilterDto() { Code = "HA123457", Description = "Description", CategoryId = 10000 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var furnitureUnits = await service.GetFurnitureUnitsAsync(furnitureUnitFilterDto);

                // Assert
                result.Should().BeEquivalentTo(furnitureUnits);
            }
        }

        [Fact]
        public async Task Get_furniture_unit_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            FurnitureUnitFilterDto furnitureUnitFilterDto = new FurnitureUnitFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var furnitureUnits = await service.GetFurnitureUnitsAsync(furnitureUnitFilterDto);

                // Assert
                furnitureUnits.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_furniture_unit_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            FurnitureUnitFilterDto furnitureUnitFilterDto = new FurnitureUnitFilterDto() { Description = "Wrong" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var furnitureUnits = await service.GetFurnitureUnitsAsync(furnitureUnitFilterDto);

                // Assert
                furnitureUnits.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_furniture_unit_filter_category_works()
        {
            // Arrange
            factory.CreateClient();
            FurnitureUnitFilterDto furnitureUnitFilterDto = new FurnitureUnitFilterDto() { CategoryId = 10000654 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var furnitureUnits = await service.GetFurnitureUnitsAsync(furnitureUnitFilterDto);

                // Assert
                furnitureUnits.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureUnit = new FurnitureUnitCreateDto
            {
                Code = "HA123456",
                Description = "Desc",
                Depth = 10,
                Width = 10,
                Height = 10,
                CategoryId = 10000,
                FurnitureUnitTypeId = 10200,
                ImageCreateDto = new ImageCreateDto() { ContainerName = "MaterialTestImages", FileName = "furnitureunit2.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(furnitureUnit), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/furnitureunits", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureUnit = new FurnitureUnitUpdateDto
            {
                Code = "CODE",
                Description = "Description",
                Depth = 10,
                Width = 10,
                Height = 10,
                CategoryId = 10000,
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "furnitureunit.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(furnitureUnit), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/furnitureunits/{new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                var updatedFurnitureUnit = await service.GetFurnitureUnitDetailsAsync(new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"));
                // Assert
                furnitureUnit.Description.Should().BeEquivalentTo(updatedFurnitureUnit.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/furnitureunits/{new Guid("d69cc81f-b318-4c06-99f5-991bb9386c91")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureUnitAppService>();
                Func<Task> act = async () => { await service.GetFurnitureUnitDetailsAsync(new Guid("d69cc81f-b318-4c06-99f5-991bb9386c91")); };

                // Assert
                await act.Should().ThrowAsync<NullReferenceException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddProductRecommendation_To_Customer_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            int customerId = 10006;
            List<Guid> Ids = new List<Guid> { new Guid("a465b4f4-fc87-4785-95af-37acf421776b") };
            FurnitureRecommendationDto dto = new FurnitureRecommendationDto(Ids);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var customerAppSevice = scope.ServiceProvider.GetRequiredService<ICustomerAppService>();

                //Act
                await customerAppSevice.UpdateProductRecommendationsAsync(customerId, dto);

                var customerWithRecommendedProduct = context.Customers
                    .AsNoTracking()
                    .Include(ent => ent.RecommendedProducts)
                    .Single(ent => ent.UserId == customerId);

                //Assert
                customerWithRecommendedProduct.RecommendedProducts.Where(x => x.WebshopFurnitureUnit.FurnitureUnitId == Ids.First()).Should().NotBeNull();
            }
        }

        [Fact]
        public async Task AddProductRecommendation_To_Non_Existing_Customer_Not_Works()
        {
            //Arrange
            factory.CreateClient();
            int customerId = 10;
            List<Guid> Ids = new List<Guid> { new Guid("4105025C-E947-4D82-8E72-216582EC6B94") };
            FurnitureRecommendationDto dto = new FurnitureRecommendationDto(Ids);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var customerAppSevice = scope.ServiceProvider.GetRequiredService<ICustomerAppService>();

                //Act & Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => customerAppSevice.UpdateProductRecommendationsAsync(customerId, dto));
            }
        }

        [Fact]
        public async Task Add_Non_Existing_ProductRecommendation_To_Customer_Not_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int customerId = 10006;
            List<Guid> Ids = new List<Guid> { new Guid("BE472113-CE95-DDDD-B5E4-6ACFAC54C639") };
            FurnitureRecommendationDto dto = new FurnitureRecommendationDto(Ids);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var customerAppSevice = scope.ServiceProvider.GetRequiredService<ICustomerAppService>();

                //Act & Assert
                await Assert.ThrowsAsync<NullReferenceException>(() => customerAppSevice.UpdateProductRecommendationsAsync(customerId, dto));
            }
        }

        [Fact]
        public async Task Add_Several_ProductRecommendation_To_Customer_Works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int customerId = 10006;

            List<Guid> Ids = new List<Guid>
            {
                new Guid("a465b4f4-fc87-4785-95af-37acf421776b"),
                new Guid("1b836bc7-0b44-4726-b26a-33056364294b")
            };

            int listCount = Ids.Count;

            FurnitureRecommendationDto dto = new FurnitureRecommendationDto(Ids);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();

                var customerAppServce = scope.ServiceProvider.GetRequiredService<ICustomerAppService>();

                //Act
                await customerAppServce.UpdateProductRecommendationsAsync(customerId, dto);

                //Asert
                var severalProductsAddedToCustomer = context.Customers
                    .AsNoTracking()
                    .Include(ent => ent.RecommendedProducts)
                    .Single(ent => ent.Id == customerId);

                Assert.Equal(severalProductsAddedToCustomer.RecommendedProducts.Count(), listCount);
            }
        }
    }
}