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
    public class WorktopBoardMaterialsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public WorktopBoardMaterialsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_worktopboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var worktopBoardMaterial = new WorktopBoardMaterial("HA126457", 10)
            {
                Id = new Guid("b33fe42e-0f53-4e2e-acb2-68843b1dacee"),
                Description = "Description",
                CategoryId = 10000,
                Dimension = new Dimension(2, 2, 2),
                Image = new Image("worktop.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da") }
            };
            worktopBoardMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) });
            var expectedResult = new WorktopBoardMaterialDetailsDto(worktopBoardMaterial) { CategoryId = 10000 };

            // Act
            var resp = await client.GetAsync($"api/worktopboards/{new Guid("b33fe42e-0f53-4e2e-acb2-68843b1dacee")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_worktopboard_materials_works()
        {
            // Arrange
            factory.CreateClient();

            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(10000, "Teszt alapanyagok", LanguageTypeEnum.HU));

            var worktopBoardMaterial = new WorktopBoardMaterial("HA126457", 10)
            {
                Id = new Guid("b33fe42e-0f53-4e2e-acb2-68843b1dacee"),
                Description = "Description",
                CategoryId = 10000,
                Category = category,
                Dimension = new Dimension(2, 2, 2),
                Image = new Image("worktop.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("bbc06ed5-9673-45c6-bc85-2554c63787da"), ThumbnailName = "thumbnail_worktop.jpg" }
            };
            worktopBoardMaterial.AddPrice(new MaterialPrice() { Price = new Price(10, 1) { Currency = new Currency("HUF") } });
            var expectedResult = new List<WorktopBoardMaterialListDto>
            {
                new WorktopBoardMaterialListDto(worktopBoardMaterial)
            };

            PagedList<WorktopBoardMaterialListDto> pagedList = new PagedList<WorktopBoardMaterialListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();
            WorktopBoardMaterialFilterDto worktopBoardMaterialFilterDto = new WorktopBoardMaterialFilterDto() { Code = "HA126457", Description = "Description", CategoryId = 10000 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                var worktopBoardMaterials = await service.GetWorktopBoardMaterialsAsync(worktopBoardMaterialFilterDto);

                // Assert
                worktopBoardMaterials.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_worktopboard_material_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            WorktopBoardMaterialFilterDto worktopBoardMaterialFilterDto = new WorktopBoardMaterialFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                var worktopBoardMaterials = await service.GetWorktopBoardMaterialsAsync(worktopBoardMaterialFilterDto);

                // Assert
                worktopBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_worktopboard_material_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            WorktopBoardMaterialFilterDto worktopBoardMaterialFilterDto = new WorktopBoardMaterialFilterDto() { Description = "Wrong description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                var worktopBoardMaterials = await service.GetWorktopBoardMaterialsAsync(worktopBoardMaterialFilterDto);

                // Assert
                worktopBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_worktopboard_material_filter_category_works()
        {
            // Arrange
            factory.CreateClient();
            WorktopBoardMaterialFilterDto worktopBoardMaterialFilterDto = new WorktopBoardMaterialFilterDto() { CategoryId = 2 };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                var worktopBoardMaterials = await service.GetWorktopBoardMaterialsAsync(worktopBoardMaterialFilterDto);

                // Assert
                worktopBoardMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_worktopboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var worktopBoardMaterial = new WorktopBoardMaterialCreateDto
            {
                Code = "HA123456",
                Description = "Desc",
                Price = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 10000,
                TransactionMultiplier = 10,
                HasFiberDirection = false,
                Dimension = new DimensionCreateDto() { Length = 2, Thickness = 2, Width = 2 },
                ImageCreateDto = new ImageCreateDto() { ContainerName = "MaterialTestImages", FileName = "worktop2.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(worktopBoardMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/worktopboards", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_worktopboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var worktopBoardMaterial = new WorktopBoardMaterialUpdateDto
            {
                Description = "Desc2",
                PriceUpdateDto = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                CategoryId = 10000,
                TransactionMultiplier = 10,
                HasFiberDirection = false,
                Dimension = new DimensionUpdateDto() { Length = 2, Thickness = 2, Width = 2 },
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "worktop.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(worktopBoardMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/worktopboards/{new Guid("31a21d26-26a0-46a8-8092-ccb06bba12f1")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                var updatedWorktopBoardMaterial = await service.GetWorktopBoardMaterialAsync(new Guid("31a21d26-26a0-46a8-8092-ccb06bba12f1"));
                // Assert
                worktopBoardMaterial.Description.Should().BeEquivalentTo(updatedWorktopBoardMaterial.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_worktopboard_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/worktopboards/{new Guid("b8675809-1172-48de-8778-17710493eca6")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorktopBoardMaterialAppService>();
                Func<Task> act = async () => { await service.GetWorktopBoardMaterialAsync(new Guid("b8675809-1172-48de-8778-17710493eca6")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}