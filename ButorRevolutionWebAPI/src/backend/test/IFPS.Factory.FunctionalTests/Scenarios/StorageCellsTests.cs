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
    public class StorageCellCellsTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public StorageCellCellsTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_storage_cell_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storageCell = new StorageCell("column3 / row2", 10000) { Id = 10000, Metadata = "Desc1" };
            var expectedResult = new StorageCellDetailsDto(storageCell);

            // Act
            var resp = await client.GetAsync("api/storagecells/10000");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Update_storage_cell_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storageCell = new StorageCellUpdateDto
            {
                Name = "New storage cell",
                Description = "New desc",
                StorageId = 10000
            };
            var content = new StringContent(JsonConvert.SerializeObject(storageCell), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/storagecells/10001", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                var updatedStorageCell = await service.GetStorageCellAsync(10001);
                // Assert
                storageCell.Name.Should().BeEquivalentTo(updatedStorageCell.Name);
                storageCell.Description.Should().BeEquivalentTo(updatedStorageCell.Description);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_storage_cell_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/storagecells/10002");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                Func<Task> act = async () => { await service.GetStorageCellAsync(10002); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_storage_cell_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var storageCell = new StorageCellCreateDto
            {
                Name = "HA123456",
                StorageId = 10000,
                Description = "New desc"
            };
            var content = new StringContent(JsonConvert.SerializeObject(storageCell), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/storagecells", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_storage_cells_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<StorageCellListDto>();
            var storageCell = new StorageCellListDto() { Id = 10003, Name = "column3 / row8", Description = "Desc4", StorageName = "Storage Nr1" };

            expectedResult.Add(storageCell);
            IPagedList<StorageCellListDto> pagedList = new PagedList<StorageCellListDto>()
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
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                StorageCellFilterDto storageCellFilterDto = new StorageCellFilterDto() { Name = "column3 / row8", StorageName = "Storage Nr1", Description = "Desc4" };
                var storageCells = await service.GetStorageCellsAsync(storageCellFilterDto);

                // Assert
                result.Should().BeEquivalentTo(storageCells);
            }
        }

        [Fact]
        public async Task Get_storage_cells_filter_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                StorageCellFilterDto storageCellFilterDto = new StorageCellFilterDto() { Name = "Wrong name" };
                var storageCells = await service.GetStorageCellsAsync(storageCellFilterDto);

                // Assert
                storageCells.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_storage_cells_filter_storage_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                StorageCellFilterDto storageCellFilterDto = new StorageCellFilterDto() { StorageName = "Wrong storage name" };
                var storageCells = await service.GetStorageCellsAsync(storageCellFilterDto);

                // Assert
                storageCells.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_storage_cells_filter_description_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStorageCellAppService>();
                StorageCellFilterDto storageCellFilterDto = new StorageCellFilterDto() { Description = "Wrong description" };
                var storageCells = await service.GetStorageCellsAsync(storageCellFilterDto);

                // Assert
                storageCells.Data.Count().Should().Be(0);
            }
        }
    }
}