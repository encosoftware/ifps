using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class ServiceTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public ServiceTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private async Task<string> GetAccessToken()
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
        public async Task Get_services_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var currency = new Currency("HUF") { Id = 10850 };
            var firstServiceType = new ServiceType(Domain.Enums.ServiceTypeEnum.Shipping) { Id = 10700 };
            var serviceType = new ServiceType(Domain.Enums.ServiceTypeEnum.Shipping) { Id = 10700 };
            var currentPrice = new ServicePrice() { Id = 10300, Price = new Price(2000.0, 10900) { Currency = currency } };
            var secondServicePrice = new ServicePrice() { Id = 10301, Price = new Price(4000.0, 10900) { Currency = currency } };

            var service1 = new Service("50-100 km")
            {
                Id = 10700,
                ServiceTypeId = 10700,
                ServiceType = firstServiceType,
                CurrentPriceId = currentPrice.Id,
                CurrentPrice = currentPrice,
            };

            var service2 = new Service("45-55 km")
            {
                Id = 10703,
                ServiceTypeId = 10700,
                ServiceType = firstServiceType,
                CurrentPriceId = secondServicePrice.Id,
                CurrentPrice = secondServicePrice
            };

            var service3 = new Service("60-65 km")
            {
                Id = 10706,
                ServiceTypeId = 10700,
                ServiceType = firstServiceType,
                CurrentPriceId = secondServicePrice.Id,
                CurrentPrice = secondServicePrice
            };

            var service4 = new Service("10-40 km")
            {
                Id = 10709,
                ServiceTypeId = 10701,
                ServiceType = firstServiceType,
                CurrentPriceId = secondServicePrice.Id,
                CurrentPrice = secondServicePrice
            };

            var expectedResult = new List<ServiceListDto>();
            expectedResult.Add(new ServiceListDto(service1));
            expectedResult.Add(new ServiceListDto(service2));
            expectedResult.Add(new ServiceListDto(service3));
            expectedResult.Add(new ServiceListDto(service4));

            var resp = await client.GetAsync("api/services/dropdown");

            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IServiceAppService>();
                var result = await service.GetServicesForDropdownAsync();

                // Assert
                resp.EnsureSuccessStatusCode();
                result.Where(ent => ent.Id > 10000).Should().BeEquivalentTo(expectedResult);
            }
        }    
    }
}
