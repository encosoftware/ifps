using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class WebshopFurnitureUnitTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public WebshopFurnitureUnitTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        [Fact]
        public async Task Get_wfu_by_id_should_work()
        {
            // Arrange
            var client = factory.CreateClient();

            var currency = new Currency("HUF") { Id = 10910 };
            var image = new Image("accessory4.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ab-4160-bcc5-9660402699da") };
            var image2 = new Image("accessory5.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("e3cfe1d4-fed7-4c72-91dd-e5c356f5d66f") };

            var furnitureUnit = new FurnitureUnit("Test Code for FU", 4700.0, 18.0, 1800.0)
            {
                Id = new Guid("a465b4f4-fc87-4785-95af-37acf421776b"),
                Image = image,
                ImageId = image.Id
            };

            var wfu = new WebshopFurnitureUnit(furnitureUnit.Id)
            {
                Id = 10001,
                Price = new Price(45000.0, currency.Id) { Currency = currency },
                FurnitureUnit = furnitureUnit
            };

            var wfui = new WebshopFurnitureUnitImage() { Id = 10000, Image = image, ImageId = image.Id, WebshopFurnitureUnit = wfu, WebshopFurnitureUnitId = wfu.Id };
            var wfui2 = new WebshopFurnitureUnitImage() { Id = 10001, Image = image2, ImageId = image2.Id, WebshopFurnitureUnit = wfu, WebshopFurnitureUnitId = wfu.Id };
            wfu.AddWebshopFurnitureUnitImage(wfui);
            wfu.AddWebshopFurnitureUnitImage(wfui2);

            var expectedResult = new WebshopFurnitureUnitDetailsDto(wfu);
            // Act
            var resp = await client.GetAsync("api/webshopfurnitureunits/10001");
            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_webshop_furnitureunits_should_work()
        {
            // Assert
            var client = factory.CreateClient();

            var currency = new Currency("HUF") { Id = 10910 };
            var image = new Image("accessory4.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("86d4d57f-a6ab-4160-bcc5-9660402699da") };
            var image2 = new Image("accessory5.jpg", ".jpg", "MaterialTestImages") { Id = new Guid("e3cfe1d4-fed7-4c72-91dd-e5c356f5d66f") };

            var fu = new FurnitureUnit("Test Webshop Code for WFU", 4700.0, 18.0, 1800.0)
            {
                Id = new Guid("1b836bc7-0b44-4726-b26a-33056364294b"),
                Image = image,
                ImageId = image.Id
            };

            var wfu = new WebshopFurnitureUnit(fu.Id)
            {
                Id = 10004,
                Price = new Price(175000.0, currency.Id) { Currency = currency },
                FurnitureUnit = fu
            };

            var wfu2 = new WebshopFurnitureUnit(fu.Id)
            {
                Id = 10005,
                Price = new Price(189000.0, currency.Id) { Currency = currency },
                FurnitureUnit = fu
            };

            var wfui = new WebshopFurnitureUnitImage() { Id = 10000, Image = image, ImageId = image.Id, WebshopFurnitureUnit = wfu, WebshopFurnitureUnitId = wfu.Id };
            var wfui2 = new WebshopFurnitureUnitImage() { Id = 10001, Image = image2, ImageId = image2.Id, WebshopFurnitureUnit = wfu, WebshopFurnitureUnitId = wfu.Id };
            wfu.AddWebshopFurnitureUnitImage(wfui);
            wfu.AddWebshopFurnitureUnitImage(wfui2);

            var wfui3 = new WebshopFurnitureUnitImage() { Id = 10002, Image = image, ImageId = image.Id, WebshopFurnitureUnit = wfu2, WebshopFurnitureUnitId = wfu2.Id };
            var wfui4 = new WebshopFurnitureUnitImage() { Id = 10003, Image = image2, ImageId = image2.Id, WebshopFurnitureUnit = wfu2, WebshopFurnitureUnitId = wfu2.Id };
            wfu.AddWebshopFurnitureUnitImage(wfui2);
            wfu.AddWebshopFurnitureUnitImage(wfui2);

            var expectedResult = new List<WebshopFurnitureUnit>
            {
                wfu, wfu2
            };

            PagedList<WebshopFurnitureUnit> pagedList = new PagedList<WebshopFurnitureUnit>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 2
            };
            var result = pagedList.ToPagedList(WebshopFurnitureUnitListDto.FromEntity);
            WebshopFurnitureUnitFilterDto wfuFilterDto = new WebshopFurnitureUnitFilterDto() { Code = "WFU" };
            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWebshopFurnitureUnitsAppService>();
                var wfus = await service.GetWebshopFurnitureUnitsAsync(wfuFilterDto);

                // Assert
                result.Should().BeEquivalentTo(wfus);
                result.Data.Count.Should().Be(wfus.Data.Count);
            }
        }

        [Fact]
        public async Task Get_webshop_furniture_unit_filter_code_should_work()
        {
            // Arrange
            factory.CreateClient();
            WebshopFurnitureUnitFilterDto wfuFilterDto = new WebshopFurnitureUnitFilterDto() { Code = "Wrong code" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWebshopFurnitureUnitsAppService>();
                var wfus = await service.GetWebshopFurnitureUnitsAsync(wfuFilterDto);

                // Assert
                wfus.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_webshop_furniture_unit_filter_name_should_work()
        {
            // Arrange
            factory.CreateClient();
            WebshopFurnitureUnitFilterDto wfuFilterDto = new WebshopFurnitureUnitFilterDto() { Description = "Wrong name" };

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWebshopFurnitureUnitsAppService>();
                var wfus = await service.GetWebshopFurnitureUnitsAsync(wfuFilterDto);

                // Assert
                wfus.Data.Count().Should().Be(0);
            }
        }

        [Fact]
        public async Task Update_wfu_should_work()
        {
            // Arrange
            var client = factory.CreateClient();

            var existedImage = new ImageCreateDto() { FileName = "accessory4.jpg", ContainerName = "MaterialTestImages" };
            var existedImage2 = new ImageCreateDto() { FileName = "accessory5.jpg", ContainerName = "MaterialTestImages" };
            var newImage = new ImageCreateDto() { FileName = "accessory7.jpg", ContainerName = "MaterialTestImages" };
            var newImage2 = new ImageCreateDto() { FileName = "accessory8.jpg", ContainerName = "MaterialTestImages" };
            var imageList = new List<ImageCreateDto>();
            imageList.Add(newImage);
            imageList.Add(newImage2);
            imageList.Add(existedImage);
            imageList.Add(existedImage2);

            var wfu = new WebshopFurnitureUnitUpdateDto()
            {
                FurnitureUnitId = new Guid("a465b4f4-fc87-4785-95af-37acf421776b"),
                Price = new PriceCreateDto() { Value = 220000, CurrencyId = 10910 },
                Images = imageList
            };

            var content = new StringContent(JsonConvert.SerializeObject(wfu), Encoding.UTF8, "application/json");
            // Act
            var response = await client.PutAsync("api/webshopfurnitureunits/10002", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWebshopFurnitureUnitsAppService>();
                var updatedWfu = await service.GetWebshopFurnitureUnitByIdAsync(10002);
                // Assert
                wfu.Price.Should().Equals(updatedWfu.Price);
                wfu.Images.Count.Should().Be(4);
            }
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_wfu_by_id_should_work()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            var response = await client.DeleteAsync("api/webshopfurnitureunits/10000");
            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
