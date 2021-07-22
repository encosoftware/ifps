using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public class OrderStateTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public OrderStateTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_orderstates_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            // Act
            var resp = await client.GetAsync("api/orderstates");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrderStateAppService>();
                var result = await service.GetOrderStatesAsync();

                // Assert
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
                result.Where(ent => ent.Id == 3).Should().BeEquivalentTo(BuildOrderState());
            }
        }

        private List<OrderStateDto> BuildOrderState()
        {
            List<OrderStateDto> orderStates = new List<OrderStateDto>();
            var orderState = new OrderState(OrderStateEnum.WaitingForOffer) { Id = 3 };
            orderState.AddTranslation(new OrderStateTranslation(coreId: 3, "Várakozás árajánlatra", LanguageTypeEnum.HU) { Id = 10000 });
            orderStates.Add(new OrderStateDto(orderState));
            return orderStates;
        }
    }
}
