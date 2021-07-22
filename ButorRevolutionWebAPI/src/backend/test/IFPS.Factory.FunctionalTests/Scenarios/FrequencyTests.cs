using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
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
    public class FrequencyTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public FrequencyTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_frequencies_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = new List<FrequencyTypeListDto>();
            var frequencyType1 = new FrequencyType(FrequencyTypeEnum.Day) { Id = 1 };
            var frequencyType2 = new FrequencyType(FrequencyTypeEnum.Week) { Id = 2 };
            var frequencyType3 = new FrequencyType(FrequencyTypeEnum.Month) { Id = 3 };
            var frequencyType4 = new FrequencyType(FrequencyTypeEnum.Year) { Id = 4 };

            //translation error Param name
            frequencyType1.AddTranslation(new FrequencyTypeTranslation("Nap", 1, LanguageTypeEnum.HU) { Id = 1 });
            frequencyType2.AddTranslation(new FrequencyTypeTranslation("Hét", 1, LanguageTypeEnum.HU) { Id = 2 });
            frequencyType3.AddTranslation(new FrequencyTypeTranslation("Hónap", 1, LanguageTypeEnum.HU) { Id = 3 });
            frequencyType4.AddTranslation(new FrequencyTypeTranslation("Év", 1, LanguageTypeEnum.HU) { Id = 4 });

            var frequency1 = new FrequencyTypeListDto(frequencyType1);
            var frequency2 = new FrequencyTypeListDto(frequencyType2);
            var frequency3 = new FrequencyTypeListDto(frequencyType3);
            var frequency4 = new FrequencyTypeListDto(frequencyType4);

            expectedResult.Add(frequency1);
            expectedResult.Add(frequency2);
            expectedResult.Add(frequency3);
            expectedResult.Add(frequency4);

            //Act
            var resp = await client.GetAsync("api/frequencytypes");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IFrequencyTypeAppService>();
                var result = await service.GetFrequencyTypesAsync();

                //Assert
                var stringresponse = await resp.Content.ReadAsStringAsync();
                jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                stringresponse.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));

                resp.StatusCode.Should().Be(HttpStatusCode.OK);
                result.Should().BeEquivalentTo(expectedResult);

            }

            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            //Assert
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }
    }
}
