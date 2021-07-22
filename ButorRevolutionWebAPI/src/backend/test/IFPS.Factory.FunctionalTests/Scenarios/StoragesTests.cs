using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
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
    public class StoragesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public StoragesTests(IFPSFactoryWebApplicationFactory factory)
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
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenDto>(stringresp);
            return model.AccessToken;
        }

        [Fact]
        public async Task Get_storage_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storage = new Storage("Storage Nr1", 10000, new Address(1064, "Budapest", "Szinyei Merse utca 13.", 1)) { Id = 10000, Company = new Company("Test EN-CO Software", 10000) };
            var expectedResult = new StorageDetailsDto(storage);

            // Act
            var resp = await client.GetAsync("api/storages/10000");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Update_storage_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storage = new StorageUpdateDto
            {
                Name = "New storage",
                Address = new AddressUpdateDto() { Address = "New Address", City = "Budapest", CountryId = 1, PostCode = 1212 }
            };
            var content = new StringContent(JsonConvert.SerializeObject(storage), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/storages/10001", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageAppService>();
                var updatedStorage = await service.GetStorageAsync(10001);
                // Assert
                storage.Name.Should().BeEquivalentTo(updatedStorage.Name);
                storage.Address.Should().BeEquivalentTo(updatedStorage.Address);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_storage_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/storages/10002");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageAppService>();
                Func<Task> act = async () => { await service.GetStorageAsync(10002); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_storage_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storage = new StorageCreateDto
            {
                Name = "HA123456",
                CompanyId = 10000,
                Address = new AddressCreateDto() { Address = "sdas", City = "Budapest", CountryId = 1, PostCode = 1212 }
            };
            var content = new StringContent(JsonConvert.SerializeObject(storage), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/storages", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_storages_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<StorageListDto>();
            var storage = new StorageListDto() { Id = 10000, Name = "Storage Nr1", Address = new AddressDetailsDto(new Address(1064, "Budapest", "Szinyei Merse utca 13.", 1)) };

            expectedResult.Add(storage);
            IPagedList<StorageListDto> pagedList = new PagedList<StorageListDto>()
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
                var service = scope.ServiceProvider.GetRequiredService<IStorageAppService>();
                StorageFilterDto storageFilterDto = new StorageFilterDto() { Name = "Storage", Address = "Szinyei" };
                var storages = await service.GetStoragesAsync(storageFilterDto);

                // Assert
                result.Should().BeEquivalentTo(storages);
            }
        }

        [Fact]
        public async Task Get_storages_filter_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageAppService>();
                StorageFilterDto storageFilterDto = new StorageFilterDto() { Name = "Wrong name" };
                var storages = await service.GetStoragesAsync(storageFilterDto);

                // Assert
                storages.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_storages_filter_address_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageAppService>();
                StorageFilterDto storageFilterDto = new StorageFilterDto() { Address = "Wrong address" };
                var storages = await service.GetStoragesAsync(storageFilterDto);

                // Assert
                storages.Data.Count().Should().Be(0);
            }
        }
    }
}