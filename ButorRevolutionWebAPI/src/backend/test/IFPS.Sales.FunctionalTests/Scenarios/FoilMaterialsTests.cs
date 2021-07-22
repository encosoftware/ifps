using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
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
    public class FoilMaterialsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public FoilMaterialsTests(IFPSSalesWebApplicationFactory factory)
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

        [Fact]
        public async Task Get_foil_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var foilMaterial = new FoilMaterial("HA127457", 10)
            {
                Id = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                Description = "Description",
                Thickness = 2,
                Image = new Image("foil2.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51") }
            };
            foilMaterial.AddPrice(new MaterialPrice(foilMaterial.Id, new Price(10, 1)));
            var expectedResult = new FoilMaterialDetailsDto(foilMaterial);

            // Act
            var resp = await client.GetAsync($"api/foils/{new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_foil_materials_works()
        {
            // Arrange
            factory.CreateClient();
            var foilMaterial = new FoilMaterial("HA127457", 10)
            {
                Id = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                Description = "Description",
                Thickness = 2,
                Image = new Image("foil2.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("ca660b04-9db1-462c-b36e-f2d4c6821f51"), ThumbnailName = "thumbnail_foil2.jpg" }
            };
            foilMaterial.AddPrice(new MaterialPrice(foilMaterial.Id, new Price(10, 1) { Currency = new Currency("HUF") }));
            var expectedResult = new List<FoilMaterialListDto>
            {
                new FoilMaterialListDto(foilMaterial)
            };

            PagedList<FoilMaterialListDto> pagedList = new PagedList<FoilMaterialListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList();
            FoilMaterialFilterDto foilMaterialFilterDto = new FoilMaterialFilterDto() { Code = "HA127457", Description = "Description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFoilMaterialAppService>();
                var foilMaterials = await service.GetFoilMaterialsAsync(foilMaterialFilterDto);

                // Assert
                foilMaterials.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Get_foil_material_filter_code_works()
        {
            // Arrange
            factory.CreateClient();
            FoilMaterialFilterDto foilMaterialFilterDto = new FoilMaterialFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFoilMaterialAppService>();
                var foilMaterials = await service.GetFoilMaterialsAsync(foilMaterialFilterDto);

                // Assert
                foilMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_foil_material_filter_description_works()
        {
            // Arrange
            factory.CreateClient();
            FoilMaterialFilterDto foilMaterialFilterDto = new FoilMaterialFilterDto() { Description = "Wrong description" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFoilMaterialAppService>();
                var foilMaterials = await service.GetFoilMaterialsAsync(foilMaterialFilterDto);

                // Assert
                foilMaterials.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Create_foil_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var foilMaterial = new FoilMaterialCreateDto
            {
                Code = "HA123456",
                Description = "Desc",
                Price = new PriceCreateDto() { Value = 10, CurrencyId = 1 },
                Thickness = 2,
                TransactionMultiplier = 10,
                ImageCreateDto = new ImageCreateDto() { ContainerName = "MaterialTestImages", FileName = "foil3.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(foilMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/foils", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_foil_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var foilMaterial = new FoilMaterialUpdateDto
            {
                Description = "Desc2",
                PriceUpdateDto = new PriceUpdateDto() { Value = 10, CurrencyId = 1 },
                Thickness = 2,
                TransactionMultiplier = 10,
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "foil2.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(foilMaterial), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/foils/{new Guid("4b2334b4-4436-4085-bcd7-c5cb939c7e97")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFoilMaterialAppService>();
                var updatedFoilMaterial = await service.GetFoilMaterialAsync(new Guid("4b2334b4-4436-4085-bcd7-c5cb939c7e97"));
                // Assert
                foilMaterial.Description.Should().BeEquivalentTo(updatedFoilMaterial.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_foil_material_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/foils/{new Guid("9e0a2348-f1e2-4d09-9594-37c0e63c4fed")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFoilMaterialAppService>();
                Func<Task> act = async () => { await service.GetFoilMaterialAsync(new Guid("9e0a2348-f1e2-4d09-9594-37c0e63c4fed")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}