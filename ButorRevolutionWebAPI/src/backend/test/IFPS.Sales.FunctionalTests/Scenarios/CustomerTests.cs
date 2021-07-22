using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class CustomerTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public CustomerTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_RecommendedProduct_For_Customer_Works()
        {
            //Arrange            
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
            int customerId = 10900;

            var currency = new Currency("HUF") { Id = 10850 };
         
            var image = new Image("d1v44.png", ".png", "FurnitureUnits")
            {
                Id = new Guid("CD8ADF9E-8413-4B77-A0BA-9719FF9D0389"),
                ThumbnailName = "d1v44.png"
            };

            var fu4 = new FurnitureUnit("Fairy Webshop Unit", 80, 150, 25)
            {
                Id = new Guid("B50D5D13-3885-4584-82C7-C267D848B894"),
                CategoryId = 25,
                ImageId = image.Id,
                Image = image,
                Description = "Kücheregal",
                CurrentPriceId = 2,
                FurnitureUnitType = BuildTopType(),
                FurnitureUnitTypeId = BuildTopType().Id
            };

            var wfuCustomer = new WebshopFurnitureUnit(fu4.Id)
            {
                Id = 10014,
                Value = 54000.0,
                Price = new Price(54000.0, currency.Id) { Currency = currency },
                FurnitureUnit = fu4
            };

            var expectedResult = new List<WebshopFurnitureUnitListByWebshopCategoryDto>
            {
                new WebshopFurnitureUnitListByWebshopCategoryDto(wfuCustomer)
            };

            //Act
            var response = await client.GetAsync($"api/customers/{customerId}");

            //Assert
            response.EnsureSuccessStatusCode();

            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var stringResponse = await response.Content.ReadAsStringAsync();
            var responseContent =
                JsonConvert.DeserializeObject<List<WebshopFurnitureUnitListByWebshopCategoryDto>>(stringResponse, jsonSerializerSettings);
            //var responseContent = JsonConvert.DeserializeObject<List<WebshopFurnitureUnitDetailsDto>>(stringResponse, jsonSerializerSettings);
            responseContent.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Get_TrendingRecommendations_For_Non_Customers_Works()
        {
            //Arrange            
            var client = factory.CreateClient();
            int customerId = 0;

            var currency = new Currency("HUF") { Id = 10850 };
            var image2 = new Image("test23.jpg", ".jpg", "MaterialTestImages")
            {
                Id = new Guid("515B4AB1-A087-4373-96F1-25E6C058D03C"),
                ThumbnailName = "test_thumbnail23.jpg"
            };

            var price = new FurnitureUnitPrice()
            {
                Id = 10532,
                Price = new Price(10, currency.Id) { Currency = currency },
                MaterialCost = new Price(10, currency.Id)
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

            var f1 = new FurnitureUnit("Test Handle", 60, 200, 60)
            {
                Id = new Guid("44A6A5AC-2C90-49E2-8842-03132DB64232"),
                CategoryId = 10000,
                Trending = true,
                ImageId = image2.Id,
                Image = image2,
                Description = "Silver and metal",
                CurrentPriceId = price.Id,
                CurrentPrice = price,
                FurnitureUnitType = BuildBaseType(),
                FurnitureUnitTypeId = BuildBaseType().Id
            };

            var f1wfu = new WebshopFurnitureUnit(f1.Id)
            {
                Id = 10011,
                Value = 30000.0,
                FurnitureUnit = f1,
                Price = new Price(30000, currency.Id) { Currency = currency }
            };

            var f2 = new FurnitureUnit("Test Base cabinet with shelves", 20, 40, 37)
            {
                Id = new Guid("9AEC5347-3E49-4493-9CCA-825363858168"),
                Trending = true,
                CategoryId = 10001,
                ImageId = image2.Id,
                Image = image2,
                Description = "Open storage solutions that complement your kitchen",
                CurrentPriceId = 1,
                FurnitureUnitType = BuildTopType(),
                FurnitureUnitTypeId = BuildTopType().Id
            };

            var f2wfu = new WebshopFurnitureUnit(f2.Id)
            {
                Id = 10012,
                Value = 100000.0,
                FurnitureUnit = f2,
                Price = new Price(100000, currency.Id) { Currency = currency }
            };

            var f3 = new FurnitureUnit("Test Unicorn Webshop Unit", 20, 34, 12)
            {
                Id = new Guid("BE472113-CE95-4C91-B5E4-6ACFEB54C638"),
                Trending = true,
                CategoryId = 10002,
                ImageId = image2.Id,
                Image = image2,
                Description = "Spülmaschine",
                CurrentPriceId = 1,
                FurnitureUnitType = BuildTallType(),
                FurnitureUnitTypeId = BuildTallType().Id
            };

            var f3wfu = new WebshopFurnitureUnit(f3.Id)
            {
                Id = 10013,
                Value = 119000.0,
                FurnitureUnit = f3,
                Price = new Price(100000, currency.Id) { Currency = currency }
            };

         
            var expectedResult = new List<WebshopFurnitureUnitListByWebshopCategoryDto>
           {
                new WebshopFurnitureUnitListByWebshopCategoryDto(f1wfu),
                new WebshopFurnitureUnitListByWebshopCategoryDto(f2wfu),
                new WebshopFurnitureUnitListByWebshopCategoryDto(f3wfu),              
           };

            //Act
            var response = await client.GetAsync($"api/customers/{customerId}");

            //Assert
            response.EnsureSuccessStatusCode();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var stringResponse = await response.Content.ReadAsStringAsync();
            var responseContent =
                JsonConvert.DeserializeObject<List<WebshopFurnitureUnitListByWebshopCategoryDto>>(stringResponse, jsonSerializerSettings);

            responseContent.Where(ent => ent.Code.Contains("Test")).ToList().Should().BeEquivalentTo(expectedResult);
        }

        private async Task<string> GetAccessTokenAsync()
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

        private FurnitureUnitType BuildTopType()
        {
            var topType = new FurnitureUnitType(FurnitureUnitTypeEnum.Top) { Id = 10700 };
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Top", LanguageTypeEnum.EN) { Id = 10200 });
            topType.AddTranslation(new FurnitureUnitTypeTranslation(topType.Id, "Felső", LanguageTypeEnum.HU) { Id = 10210 });
            return topType;
        }

        private FurnitureUnitType BuildTallType()
        {
            var tallType = new FurnitureUnitType(FurnitureUnitTypeEnum.Tall) { Id = 10710 };
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Tall", LanguageTypeEnum.EN) { Id = 10220 });
            tallType.AddTranslation(new FurnitureUnitTypeTranslation(tallType.Id, "Magas", LanguageTypeEnum.HU) { Id = 10230 });
            return tallType;
        }

        private FurnitureUnitType BuildBaseType()
        {
            var baseType = new FurnitureUnitType(FurnitureUnitTypeEnum.Base) { Id = 10720 };
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Base", LanguageTypeEnum.EN) { Id = 10240 });
            baseType.AddTranslation(new FurnitureUnitTypeTranslation(baseType.Id, "Alsó", LanguageTypeEnum.HU) { Id = 10250 });
            return baseType;
        }
    }
}