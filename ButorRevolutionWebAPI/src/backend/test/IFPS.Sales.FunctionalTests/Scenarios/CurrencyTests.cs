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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class CurrencyTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public CurrencyTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_currencies_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var currency2 = new Currency("HUF") { Id = 10850 };
            var currency1 = new Currency("HUF") { Id = 10900 };
            var currency3 = new Currency("HUF") { Id = 10901 };
            var currency4 = new Currency("HUF") { Id = 10910 };
            var currency5 = new Currency("HUF") { Id = 10870 };

            var resultDto = new List<CurrencyDto>();
            resultDto.Add(new CurrencyDto(currency1));
            resultDto.Add(new CurrencyDto(currency2));
            resultDto.Add(new CurrencyDto(currency3));
            resultDto.Add(new CurrencyDto(currency4));
            resultDto.Add(new CurrencyDto(currency5));

            //Act
            var resp = await client.GetAsync("api/currencies");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICurrencyAppService>();
                var expectedResult = await service.GetCurrenciesListAsync();

                //Assert
                resp.EnsureSuccessStatusCode();
                expectedResult.Where(ent => ent.Id > 10000).Should().BeEquivalentTo(resultDto);
            }
        }
    }
}