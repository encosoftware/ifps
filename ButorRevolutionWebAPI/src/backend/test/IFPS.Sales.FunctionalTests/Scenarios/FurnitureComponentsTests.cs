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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class FurnitureComponentsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public FurnitureComponentsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_furniture_component_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureComponent = new FurnitureComponent("COMP", 1, 1, 1)
            {
                Id = new Guid("a18b97d4-fe21-4f9e-ac3f-9888b8fb90a8"),
                Type = FurnitureComponentTypeEnum.Front,
                BoardMaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"),
                BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                Image = new Image("test.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da") },
                FurnitureUnitId = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952")
            };
            var expectedResult = new FurnitureComponentDetailsDto(furnitureComponent);

            // Act
            var resp = await client.GetAsync($"api/furniturecomponents/{new Guid("a18b97d4-fe21-4f9e-ac3f-9888b8fb90a8")}");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Create_furniture_component_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureComponent = new FurnitureComponentCreateDto
            {
                Name = "HA123456",
                FurnitureUnitId = new Guid("0b7c06c0-43f5-4e45-8c7f-68e0f1b41952"),
                Amount = 10,
                Width = 10,
                Height = 10,
                Type = FurnitureComponentTypeEnum.Front,
                MaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"),
                BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
            };
            var content = new StringContent(JsonConvert.SerializeObject(furnitureComponent), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/furniturecomponents", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsAsync(typeof(Guid));
            stringresp.Should().BeOfType(typeof(Guid));
        }

        [Fact]
        public async Task Update_furniture_component_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var furnitureComponent = new FurnitureComponentUpdateDto
            {
                Name = "New",
                Amount = 10,
                Width = 10,
                Height = 10,
                MaterialId = new Guid("2460fbb0-04f9-48cf-9ff4-cb4c7fe72abe"),
                BottomFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                TopFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                LeftFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                RightFoilId = new Guid("1e824fae-3432-40b8-a858-6eca4b20df0c"),
                ImageUpdateDto = new ImageUpdateDto() { ContainerName = "MaterialTestImages", FileName = "furniturecomponent.jpg" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(furnitureComponent), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/furniturecomponents/{new Guid("c371c8b1-ad09-441b-adc4-dfb0ad69a1d3")}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureComponentAppService>();
                var updatedFurnitureComponent = await service.GetFurnitureComponentDetailsAsync(new Guid("c371c8b1-ad09-441b-adc4-dfb0ad69a1d3"));
                // Assert
                furnitureComponent.Name.Should().BeEquivalentTo(updatedFurnitureComponent.Name);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_furniture_component_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync($"api/furniturecomponents/{new Guid("61ce580c-e65b-4282-a1d5-fca69c43fb0d")}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFurnitureComponentAppService>();
                Func<Task> act = async () => { await service.GetFurnitureComponentDetailsAsync(new Guid("61ce580c-e65b-4282-a1d5-fca69c43fb0d")); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}