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
    public class AccessoryMaterialsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public AccessoryMaterialsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_accessory_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var accessoryMaterial = new AccessoryMaterial(true, true, "HA123457", 10)
            {
                Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
                Description = "Description",
                CategoryId = 10000,
                Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_accessory.jpg" }
            };
            accessoryMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) });
            var expectedResult = new AccessoryMaterialDetailsDto(accessoryMaterial);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.GetAsync($"api/accessories/{new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_accessory_materials_works()
        {
            // Arrange
            factory.CreateClient();

            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Teszt alapanyagok", LanguageTypeEnum.HU));

            var accessoryMaterial = new AccessoryMaterial(true, true, "HA123457", 10)
            {
                Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
                Description = "Description",
                CategoryId = 10000,
                Category = category,
                TransactionMultiplier = 10,
                Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_accessory.jpg" }
            };
            accessoryMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) { Currency = new Currency("HUF") } });

            var expectedResult = new List<AccessoryMaterialListDto>
            {
                new AccessoryMaterialListDto(accessoryMaterial)
            };

            PagedList<AccessoryMaterialListDto> pagedList = new PagedList<AccessoryMaterialListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();
            AccessoryMaterialFilterDto accessoryMaterialFilterDto = new AccessoryMaterialFilterDto() { Code = "HA123457", Description = "Description", CategoryId = 10000 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                var accessoryMaterials = await service.GetAccessoryMaterialsAsync(accessoryMaterialFilterDto);

                // Assert
                accessoryMaterials.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_accessory_material_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            AccessoryMaterialFilterDto accessoryMaterialFilterDto = new AccessoryMaterialFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                var accessoryMaterials = await service.GetAccessoryMaterialsAsync(accessoryMaterialFilterDto);

                // Assert
                accessoryMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_accessory_material_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            AccessoryMaterialFilterDto accessoryMaterialFilterDto = new AccessoryMaterialFilterDto() { Description = "Wrong description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                var accessoryMaterials = await service.GetAccessoryMaterialsAsync(accessoryMaterialFilterDto);

                // Assert
                accessoryMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_accessory_material_filter_category_works()
        {
            // Arrange
            factory.CreateClient();
            AccessoryMaterialFilterDto accessoryMaterialFilterDto = new AccessoryMaterialFilterDto() { CategoryId = 10000654 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                var accessoryMaterials = await service.GetAccessoryMaterialsAsync(accessoryMaterialFilterDto);

                // Assert
                accessoryMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_accessory_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var accessoryMaterial = new AccessoryMaterialCreateDto
            {
                Code = "HA123456",
                IsOptional = true,
                IsRequiredForAssembly = false,
                Description = "Desc",
                Price = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 1,
                TransactionMultiplier = 50,
                ImageCreateDto = new ImageCreateDto() { ContainerName = "MaterialTestImages", FileName = "accessorycomponent2.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(accessoryMaterial), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.PostAsync("api/accessories", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_accessory_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var accessoryMaterial = new AccessoryMaterialUpdateDto
            {
                Description = "Desc2",
                PriceUpdateDto = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 1,
                TransactionMultiplier = 10,
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "accessory.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(accessoryMaterial), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.PutAsync($"api/accessories/{new Guid("91a00f8b-f350-4778-abd3-2123a707693f")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                var updatedAccessoryMaterial = await service.GetAccessoryMaterialAsync(new Guid("91a00f8b-f350-4778-abd3-2123a707693f"));
                // Assert
                accessoryMaterial.Description.Should().BeEquivalentTo(updatedAccessoryMaterial.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_accessory_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/accessories/{new Guid("92aa6e7c-a524-45ec-8d67-5bcfa2393f21")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryMaterialAppService>();
                Func<Task> act = async () => { await service.GetAccessoryMaterialAsync(new Guid("92aa6e7c-a524-45ec-8d67-5bcfa2393f21")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}