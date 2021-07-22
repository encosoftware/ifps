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
    public class DecorBoardMaterialsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public DecorBoardMaterialsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_decorboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var decorBoardMaterial = new DecorBoardMaterial("HA125457", 10)
            {
                Id = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"),
                Description = "Description",
                CategoryId = 10000,
                Dimension = new Dimension(2, 2, 2),
                Image = new Image("decorboard.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da") }
            };
            decorBoardMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) });
            var expectedResult = new DecorBoardMaterialDetailsDto(decorBoardMaterial) { CategoryId = 10000 };

            // Act
            var resp = await client.GetAsync($"api/decorboards/{new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_decorboard_materials_works()
        {
            // Arrange
            factory.CreateClient();

            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Teszt alapanyagok", LanguageTypeEnum.HU));

            var decorBoardMaterial = new DecorBoardMaterial("HA125457", 10)
            {
                Id = new Guid("2460FBB0-04F9-48CF-9FF4-CB4C7FE72ABE"),
                Description = "Description",
                CategoryId = 10000,
                Category = category,
                Dimension = new Dimension(2, 2, 2),
                Image = new Image("decorboard.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_decorboard.jpg" }
            };
            decorBoardMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) { Currency = new Currency("HUF") } });
            var expectedResult = new List<DecorBoardMaterialListDto>
            {
                new DecorBoardMaterialListDto(decorBoardMaterial)
            };

            PagedList<DecorBoardMaterialListDto> pagedList = new PagedList<DecorBoardMaterialListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();
            DecorBoardMaterialFilterDto decorBoardMaterialFilterDto = new DecorBoardMaterialFilterDto() { Code = "HA125457", Description = "Description", CategoryId = 10000 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                var decorBoardMaterials = await service.GetDecorBoardMaterialsAsync(decorBoardMaterialFilterDto);

                // Assert
                decorBoardMaterials.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_decorboard_material_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            DecorBoardMaterialFilterDto decorBoardMaterialFilterDto = new DecorBoardMaterialFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                var decorBoardMaterials = await service.GetDecorBoardMaterialsAsync(decorBoardMaterialFilterDto);

                // Assert
                decorBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_decorboard_material_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            DecorBoardMaterialFilterDto decorBoardMaterialFilterDto = new DecorBoardMaterialFilterDto() { Description = "Wrong description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                var decorBoardMaterials = await service.GetDecorBoardMaterialsAsync(decorBoardMaterialFilterDto);

                // Assert
                decorBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_decorboard_material_filter_category_works()
        {
            // Arrange
            factory.CreateClient();
            DecorBoardMaterialFilterDto decorBoardMaterialFilterDto = new DecorBoardMaterialFilterDto() { CategoryId = 10000654 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                var decorBoardMaterials = await service.GetDecorBoardMaterialsAsync(decorBoardMaterialFilterDto);

                // Assert
                decorBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_decorboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var decorBoardMaterial = new DecorBoardMaterialCreateDto
            {
                Code = "HA123456",
                Description = "Desc",
                Price = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 10000,
                TransactionMultiplier = 10,
                HasFiberDirection = false,
                Dimension = new DimensionCreateDto() { Length = 2, Thickness = 2, Width = 2 },
                ImageCreateDto = new ImageCreateDto() { ContainerName = "MaterialTestImages", FileName = "decorboard2.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(decorBoardMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/decorboards", content);
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_decorboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var decorBoardMaterial = new DecorBoardMaterialUpdateDto
            {
                Description = "Desc2",
                PriceUpdateDto = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 10000,
                TransactionMultiplier = 10,
                HasFiberDirection = false,
                Dimension = new DimensionUpdateDto() { Length = 2, Thickness = 2, Width = 2 },
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "decorboard.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(decorBoardMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/decorboards/{new Guid("d0022a4e-3339-4ee7-8988-f4af75d22d1f")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                var updatedDecorBoardMaterial = await service.GetDecorBoardMaterialAsync(new Guid("d0022a4e-3339-4ee7-8988-f4af75d22d1f"));
                // Assert
                decorBoardMaterial.Description.Should().BeEquivalentTo(updatedDecorBoardMaterial.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_decorboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/decorboards/{new Guid("5e05089f-1dac-4883-9f62-6d16bf682dcd")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IDecorBoardMaterialAppService>();
                Func<Task> act = async () => { await service.GetDecorBoardMaterialAsync(new Guid("5e05089f-1dac-4883-9f62-6d16bf682dcd")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}