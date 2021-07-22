using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class TicketTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public TicketTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_tickets_by_orderid_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var expectedResult = new System.Collections.Generic.List<TicketByOrderListDto>();
            var orderState = new OrderState(Domain.Enums.OrderStateEnum.UnderProduction);
            var translation = new OrderStateTranslation(10004, "Várakozás gyártásra", ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.EN) { Id = 100010 };
            User assignedToUser = BuildUser();

            orderState.AddTranslation(translation);
            var ticket = new Ticket(orderState: orderState, assignedTo: assignedToUser, new DateTime(2019, 7, 10)) { Id = 10000 };

            expectedResult.Add(new TicketByOrderListDto(ticket));

            //Act
            var resp = await client.GetAsync($"api/tickets/orders/{new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887")}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();
                var result = await service.GetTicketsByOrderAsync(new Guid("418e7eaf-35c0-497f-8224-e2086cc0e887"));

                var stringresponse = await resp.Content.ReadAsStringAsync();
                jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                //Assert
                stringresponse.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));

                resp.StatusCode.Should().Be(HttpStatusCode.OK);
                result.Should().BeEquivalentTo(expectedResult);

            }
        }

        [Fact]
        public async Task Get_tickets_by_wrong_orderid_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var expectedResult = new System.Collections.Generic.List<TicketByOrderListDto>();
            var orderState = new OrderState(Domain.Enums.OrderStateEnum.WaitingForFirstPayment);
            var translation = new OrderStateTranslation(10004, "None", ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.EN) { Id = 100010 };
            User assignedToUser = BuildUser();

            orderState.AddTranslation(translation);
            var ticket = new Ticket(orderState: orderState, assignedTo: assignedToUser, new DateTime(2019, 7, 10)) { Id = 10000 };

            expectedResult.Add(new TicketByOrderListDto(ticket));

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                //Act
                var service = scope.ServiceProvider.GetRequiredService<ITicketAppService>();

                // Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetTicketsByOrderAsync(new Guid("418e7eaf-35c0-497f-8224-e2086cc1e887")));
            }
        }

        private static User BuildUser()
        {
            return new User("enco@enco.hu")
            {
                Id = 10000,
                CurrentVersion = new UserData("Yevgeny Zamjatin", "+362497489", Clock.Now, null) { Id = 10000 },
                CreationTime = new DateTime(2018, 05, 10),
                UserName = "enco",
                CompanyId = 10000,
                EmailConfirmed = true,
                ImageId = new Guid("2154e885-c52f-4e70-9408-f919db51fdae")
            };
        }
    }
}