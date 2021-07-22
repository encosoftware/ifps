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
    public class StocksTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public StocksTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var stock = new Stock(10000, 5, 10000) { Id = 10000 };
            var expectedResult = new StockDetailsDto(stock);

            // Act
            var resp = await client.GetAsync("api/stocks/10000");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Update_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var stock = new StockUpdateDto
            {
                PackageId = 10000,
                Quantity = 10,
                StorageCellId = 10000
            };
            var content = new StringContent(JsonConvert.SerializeObject(stock), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/stocks/10001", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                var updatedStock = await service.GetStockAsync(10001);
                // Assert
                stock.PackageId.Should().Be(updatedStock.PackageId);
                stock.Quantity.Should().Be(updatedStock.Quantity);
                stock.StorageCellId.Should().Be(updatedStock.StorageCellId);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.DeleteAsync("api/stocks/10002");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                Func<Task> act = async () => { await service.GetStockAsync(10002); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_stock_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var stock = new StockCreateDto
            {
                PackageId = 10000,
                Quantity = 10,
                StorageCellId = 10000
            };
            var content = new StringContent(JsonConvert.SerializeObject(stock), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PostAsync("api/stocks", content);

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_stocks_works()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<StockListDto>();
            var stock = new StockListDto() { Id = 10003, PackageDescription = "Stuff", PackageCode = "COD-748", OrderName = null, Quantity = 11, StorageCellMetadata = "Desc1", StorageCellName = "column3 / row2" };

            expectedResult.Add(stock);
            IPagedList<StockListDto> pagedList = new PagedList<StockListDto>()
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
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { PackageCode = "COD-748", PackageDescription = "Stuff", Quantity = 11, StorageCellMetadata = "Desc1", StorageCellName = "column3 / row2" };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                result.Should().BeEquivalentTo(stocks);
            }
        }

        [Fact]
        public async Task Get_stocks_filter_package_code_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { PackageCode = "Wrong package code" };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                stocks.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_stocks_filter_package_description_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { PackageDescription = "Wrong package description" };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                stocks.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_stocks_filter_quantity_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { Quantity = 1115 };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                stocks.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_stocks_filter_cell_name_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { StorageCellName = "Wrong storage cell name" };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                stocks.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_stocks_filter_cell_metadata_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IStockAppService>();
                StockFilterDto stockFilterDto = new StockFilterDto() { StorageCellMetadata = "Wrong storage cell description" };
                var stocks = await service.GetStocksAsync(stockFilterDto);

                // Assert
                stocks.Data.Count().Should().Be(0);
            }
        }
    }
}