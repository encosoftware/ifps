using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class CountriesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public CountriesTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_countries_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new List<CountryListDto>();
            var country1 = new Country(code: "HU") { Id = 1 };
            country1.AddTranslation(new CountryTranslation(1, "Magyarország", LanguageTypeEnum.HU) { Id = 1 });

            var country2 = new Country(code: "SK") { Id = 2 };
            country2.AddTranslation(new CountryTranslation(2, "Szlovákia", LanguageTypeEnum.HU) { Id = 2 });
            expectedResult.Add(new CountryListDto(country1));
            expectedResult.Add(new CountryListDto(country2));

            //Act
            var resp = await client.GetAsync("api/countries");

            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            //Assert
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }
    }
}