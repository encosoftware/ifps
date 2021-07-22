using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
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
    public class AccessoryFurnitureUnitsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public AccessoryFurnitureUnitsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_accessory_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var accessoryFurnitureUnit = new AccessoryMaterialFurnitureUnit("AMFU", 5)
            {
                Id = 10003,
                FurnitureUnitId = new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"),
                AccessoryId = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
                Accessory = new AccessoryMaterial(true, true, "HA123457", 10)
                {
                    Id = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
                    Image = new Image("accessory.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ad-4160-bcc5-9660402699da"), ThumbnailName = "thumbnail_accessory.jpg" }
                }
            };
            var expectedResult = new AccessoryFurnitureUnitDetailsDto(accessoryFurnitureUnit);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryFurnitureUnitAppService>();
                var updatedaccessoryFurnitureUnit = await service.GetAccessoryFurnitureUnitDetailsAsync(10004);
                // Assert
                accessoryFurnitureUnit.Name.Should().BeEquivalentTo(updatedaccessoryFurnitureUnit.Name);
            }
            // Act
            var resp2 = await client.GetAsync("api/accessoryfurnitureunits/10003");
            // Assert
            resp2.EnsureSuccessStatusCode();
            var stringresp2 = await resp2.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp2.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Create_accessory_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var accessoryFurnitureUnit = new AccessoryFurnitureUnitCreateDto
            {
                Name = "HA123456",
                FurnitureUnitId = new Guid("20dd26b8-b89f-4471-b5ce-be1103b724dc"),
                Amount = 10,
                MaterialId = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
            };
            var content = new StringContent(JsonConvert.SerializeObject(accessoryFurnitureUnit), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/accessoryfurnitureunits", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Update_accessory_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var accessoryFurnitureUnit = new AccessoryFurnitureUnitUpdateDto
            {
                Name = "New",
                Amount = 10,
                MaterialId = new Guid("ef3bcc95-18df-463d-ae18-f6550d45d2b1"),
                ImageUpdateDto = new ImageUpdateDto()
                {
                    ContainerName = "MaterialTestImages",
                    FileName = "accessorycomponent.jpg"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(accessoryFurnitureUnit), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/accessoryfurnitureunits/10004", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryFurnitureUnitAppService>();
                var updatedaccessoryFurnitureUnit = await service.GetAccessoryFurnitureUnitDetailsAsync(10004);
                // Assert
                accessoryFurnitureUnit.Name.Should().BeEquivalentTo(updatedaccessoryFurnitureUnit.Name);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_accessory_furniture_unit_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/accessoryfurnitureunits/10005");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAccessoryFurnitureUnitAppService>();
                Func<Task> act = async () => { await service.GetAccessoryFurnitureUnitDetailsAsync(10005); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }
    }
}