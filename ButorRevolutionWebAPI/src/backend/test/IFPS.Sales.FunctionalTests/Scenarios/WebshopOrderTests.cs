using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.EF;
using Microsoft.EntityFrameworkCore;
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
    public class WebshopOrderTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public WebshopOrderTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_webshop_orders_for_customer_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var currency = new Currency("HUF") { Id = 10901 };
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified); //hack to workaround unspecified Kind which comes from the test seed
            var worder1 = new WebshopOrder("Test webshop order 1", 10909, new Address(1117, "Budapest", "Fő utca 1.", 1))
            {
                Id = new Guid("7947278c-1979-4776-98be-e2f90d802cf9"),
                CreationTime = date,
                Basket = new Basket(10001) { Id = 10000, SubTotal = new Price(2000.0, 10901) { Currency = currency }, DelieveryPrice = new Price(1000.0, 10901) { Currency = currency} }                
            };
            var worder2 = new WebshopOrder("Test webshop order 2", 10909, new Address(1117, "Budapest", "Fő utca 2.", 1))
            {
                Id = new Guid("7d94d9e1-6b2e-488f-b9a3-e85e60ea332b"),
                CreationTime = date,
                Basket = new Basket(10001) { Id = 10000, SubTotal = new Price(2000.0, 10901) { Currency = currency }, DelieveryPrice = new Price(1000.0, 10901) { Currency = currency } }
            };

            var expectedResult = new List<WebshopOrdersListDto>()
            {
                new WebshopOrdersListDto(worder2)
                {
                    SubTotal = new PriceListDto(worder2.Basket.SubTotal),
                    DelieveryPrice = new PriceListDto(worder2.Basket.DelieveryPrice)
                },
                new WebshopOrdersListDto(worder1) { 
                    SubTotal = new PriceListDto(worder1.Basket.SubTotal), 
                    DelieveryPrice = new PriceListDto(worder1.Basket.DelieveryPrice)
                }
            };

            // Act
            var resp = await client.GetAsync($"api/webshoporders/10909");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var responseContent = JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings);
            stringresp.Should().Be(responseContent);
        }

        [Fact]
        public async Task Get_webshop_order_details_by_id()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            var currency = new Currency("HUF") { Id = 10901 };

            var img = new Image("furnitureunit002.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("2fad9825-e3e8-4a72-bdc6-68d911513e10"), ThumbnailName = "thumbnail_furnitureunit002.jpg" };           
            var fu = new FurnitureUnit("TESTCODE0001", 200, 200, 200)
            {
                Id = new Guid("d84840a9-1dca-4424-9248-0b81b7e5a491"),
                Description = "Description lorem ipsum",
                CategoryId = 10000,
                ImageId = img.Id,
                Image = img,
                CurrentPriceId = 10010
            };
            fu.AddPrice(new FurnitureUnitPrice() { Id = 10010, Price = new Price(10, currency.Id), MaterialCost = new Price(10, currency.Id) });

            var ofu = new OrderedFurnitureUnit(new Guid("2fad9825-e3e8-4a72-bdc6-68d911513e10"), 2, 10001)
            {
                UnitPrice = new Price(12000.0, 10901) { Currency = currency },
                FurnitureUnit = fu
            };

            var worder2 = new WebshopOrder("Test webshop order 2", 10909, new Address(1117, "Budapest", "Fő utca 2.", 1))
            {
                Id = new Guid("7d94d9e1-6b2e-488f-b9a3-e85e60ea332b"),
                CreationTime = Clock.Now,
                Basket = new Basket(10001) { Id = 10000, SubTotal = new Price(2000.0, 10901) { Currency = currency }, DelieveryPrice = new Price(1000.0, 10901) { Currency = currency } }
            };
            worder2.AddOrderedFurnitureUnit(ofu);

            var expectedResult = new WebshopOrdersDetailsDto(worder2);            

            // Act
            var resp = await client.GetAsync($"api/webshoporders/{ new Guid("7d94d9e1-6b2e-488f-b9a3-e85e60ea332b") }/orderedFurnitureUnits");

            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }
    }
}
