using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
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
    public class ApplianceMaterialsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public ApplianceMaterialsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_appliance_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var applianceMaterial = new ApplianceMaterial("HA124457")
            {
                Id = new Guid("c81a3824-1685-414e-9641-e3c916c27302"),
                Description = "Description",
                CategoryId = 10000,
                SellPrice = new Price(10, 1),
                BrandId = 10000,
                HanaCode = "Test",
                Image = new Image("appliance.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da") }
            };
            applianceMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) });
            var expectedResult = new ApplianceMaterialDetailsDto(applianceMaterial);

            // Act
            var resp = await client.GetAsync($"api/appliances/{new Guid("c81a3824-1685-414e-9641-e3c916c27302")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_appliance_materials_works()
        {
            // Arrange
            factory.CreateClient();

            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Teszt alapanyagok", LanguageTypeEnum.HU));

            var applianceMaterial = new ApplianceMaterial("HA124457")
            {
                Id = new Guid("C81A3824-1685-414E-9641-E3C916C27302"),
                Description = "Description",
                CategoryId = 10000,
                Category = category,
                SellPrice = new Price(10, 1) { Currency = new Currency("HUF") },
                Brand = new Company("Test EN-CO Software", 10000),
                HanaCode = "Test",
                Image = new Image("appliance.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_appliance.jpg" }
            };
            applianceMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) { Currency = new Currency("HUF") } });
            var expectedResult = new List<ApplianceMaterialListDto>
            {
                new ApplianceMaterialListDto(applianceMaterial)
            };

            PagedList<ApplianceMaterialListDto> pagedList = new PagedList<ApplianceMaterialListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto()
            {
                Code = "HA124457",
                Description = "Description",
                CategoryId = 10000,
                Brand = "Test",
                HanaCode = "Test"
            };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_appliance_material_filter_brand_works()
        {
            // Arrange
            factory.CreateClient();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto() { Brand = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_appliance_material_filter_hanacode_works()
        {
            // Arrange
            factory.CreateClient();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto() { HanaCode = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_appliance_material_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_appliance_material_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto() { Description = "Wrong description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_appliance_material_filter_category_works()
        {
            // Arrange
            factory.CreateClient();
            ApplianceMaterialFilterDto applianceMaterialFilterDto = new ApplianceMaterialFilterDto()
            {
                CategoryId = 10000654
            };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var applianceMaterials = await service.GetApplianceMaterialsAsync(applianceMaterialFilterDto);

                // Assert
                applianceMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_appliance_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            var applianceMaterial = new ApplianceMaterialCreateDto
            {
                Code = "HA123456",
                Description = "Desc",
                PurchasingPrice = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                SellPrice = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 1,
                CompanyId = 1,
                HanaCode = "TestHANACode",
                ImageCreateDto = new ImageCreateDto()
                {
                    ContainerName = "MaterialTestImages",
                    FileName = "appliance2.jpg"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(applianceMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/appliances", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_appliance_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var applianceMaterial = new ApplianceMaterialUpdateDto
            {
                Description = "Desc2",
                PurchasingPrice = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                SellPrice = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 1,
                CompanyId = 1,
                HanaCode = "TestHANACode",
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "appliance.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(applianceMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/appliances/{new Guid("012e2fd4-12de-4148-9187-90c321daac16")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                var updatedApplianceMaterial = await service.GetApplianceMaterialAsync(new Guid("012e2fd4-12de-4148-9187-90c321daac16"));
                // Assert
                applianceMaterial.Description.Should().BeEquivalentTo(updatedApplianceMaterial.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_appliance_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/appliances/{new Guid("bf79421c-a97b-4da2-8191-9cd19e328aea")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IApplianceMaterialAppService>();
                Func<Task> act = async () => { await service.GetApplianceMaterialAsync(new Guid("bf79421c-a97b-4da2-8191-9cd19e328aea")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}