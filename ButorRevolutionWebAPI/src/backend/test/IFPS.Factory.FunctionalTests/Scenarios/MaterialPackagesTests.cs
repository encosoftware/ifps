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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class MaterialPackagesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public MaterialPackagesTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_MaterialPackages_Works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var expectedResult = new List<MaterialPackage>();

            var package = new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), 10001)
            {
                Id = 10000,
                Supplier = BuildSupplier(),
                PackageDescription = "Package with stuff",
                Size = 1,
                PackageCode = "PAC-591",
                Price = new Price(10000.0, 1) { Currency = BuildCurrency() }
            };

            expectedResult.Add(package);

            PagedList<MaterialPackage> pagedList = new PagedList<MaterialPackage>
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };

            var result = pagedList.ToPagedList(MaterialPackageListDto.FromEntity);

            //Assert
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMaterialPackageAppService>();
                MaterialPackageFilterDto materialPackageFilterDto = new MaterialPackageFilterDto
                {
                    Code = "PAC-591",
                    Description = "Package with stuff",
                    Size = 1,
                    SupplierName = "Test Super Supplier Company"
                };
                var packages = await service.GetMaterialPackagesAsync(materialPackageFilterDto);

                //Assert
                result.Should().BeEquivalentTo(packages);
            }
        }

        [Fact]
        public async Task Create_MaterialPackage_Works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var materialPackage = new MaterialPackageCreateDto
            {
                MaterialId = new Guid("b2e0b4a3-8327-4836-fff5-deaec8b3c93c"),
                Code = "PAC-591",
                Description = "Package with stuff",
                Price = new PriceCreateDto { CurrencyId = 1, Value = 10000.0 },
                Size = 1,
                SupplierId = 10009
            };

            var content = new StringContent(JsonConvert.SerializeObject(materialPackage), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PostAsync("api/materialpackages", content);
            resp.EnsureSuccessStatusCode();

            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_MaterialPackage_Works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var materialPackage = new MaterialPackage(new Guid("b2e0b4a3-8327-4836-a4b3-deaec8b3c83b"), supplierId: 10001,new Price(10000.0, 1) { Currency = new Currency("HUF") });

            var expectedResult = new MaterialPackageDetailsDto(materialPackage)
            {
                Code = "PAC-591",
                Description = "Package with stuff",
                Size = 1,
            };

            //Act
            var resp = await client.GetAsync("api/materialpackages/10000");
            resp.EnsureSuccessStatusCode();

            //Assert
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Delete_MaterialPackage_Works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var resp = await client.DeleteAsync("api/materialpackages/10000");

            //Assert
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMaterialPackageAppService>();
                Func<Task> act = async () => { await service.GeMaterialPackageAsync(10000); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_MaterialPackage_Cannot_Work_bad_orderId()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var resp = await client.DeleteAsync("api/materialpackages/10099");

            //Assert
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMaterialPackageAppService>();
                Func<Task> act = async () => { await service.GeMaterialPackageAsync(10099); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        private static Currency BuildCurrency()
        {
            return new Currency("HUF") { Id = 1 };
        }

        private static Company BuildSupplier()
        {
            return new Company("Test Super Supplier Company", 10002) { Id = 10001, CurrentVersionId = 10000 };
        }
    }
}
