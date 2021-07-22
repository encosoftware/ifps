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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class CountriesTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public CountriesTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_countries_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new List<CountryListDto>();
            var country1 = new Country(code: "HU") { Id = 10000 };
            country1.AddTranslation(new CountryTranslation(1, "Magyarország", ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU) { Id = 1 });

            var country2 = new Country(code: "SK") { Id = 10001 };
            country2.AddTranslation(new CountryTranslation(2, "Szlovákia", ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU) { Id = 2 });
            expectedResult.Add(new CountryListDto(country1));
            expectedResult.Add(new CountryListDto(country2));

            //Act
            var resp = await client.GetAsync("api/countries");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICountryAppService>();
                var result = await service.GetCountriesAsync();

                //Assert
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
                result.Where(ent => ent.Id > 9000).Should().BeEquivalentTo(expectedResult);
                result.Where(ent => ent.Id > 9000).Count().Should().Be(expectedResult.Count());
            }
        }
    }
}