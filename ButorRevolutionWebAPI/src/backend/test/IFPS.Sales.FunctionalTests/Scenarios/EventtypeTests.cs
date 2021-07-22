using ENCO.DDD.Domain.Model.Enums;
using FluentAssertions;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class EventtypeTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public EventtypeTests(IFPSSalesWebApplicationFactory factory)
        {
            this.factory = factory;
            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            CultureInfo.CurrentCulture = new CultureInfo("hu-HU");
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
        public async Task Get_Eventtypes_Work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = BuildEventtypes();
            // Act
            var resp = await client.GetAsync($"api/notifications/events");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<INotificationAppService>();
                var notifications = await service.GetNotificationEventTypes();
                // Assert
                expectedResult.Should().BeEquivalentTo(notifications.Where(ent => ent.Id > 9000));
                resp.EnsureSuccessStatusCode();
            }
        }

        private List<EventTypeDto> BuildEventtypes()
        {
            List<EventTypeDto> events = new List<EventTypeDto>();
            EventType type1 = new EventType(EventTypeEnum.NewAppointment) { Id = 10000 };
            type1.AddTranslation(new EventTypeTranslation(1, "Új időpont", "A felhasználó értesítést kap az új események létrehozásáról", LanguageTypeEnum.HU) { Id = 10000 });

            EventType type2 = new EventType(EventTypeEnum.AppointmentReminder) { Id = 10001 };
            type2.AddTranslation(new EventTypeTranslation(2, "Esemény emlékezetető", "A felhasználó értesítést kap a közelgő eseményekről", LanguageTypeEnum.HU) { Id = 10001 });

            EventType type3 = new EventType(EventTypeEnum.ChangedOrderState) { Id = 10002 };
            type3.AddTranslation(new EventTypeTranslation(3, "Állapot változás", "A felhasználó értesítést kap a hozzá tartozó megrendelés állapotváltozásáról", LanguageTypeEnum.HU) { Id = 10002 });

            EventType type4 = new EventType(EventTypeEnum.NewFilesUploaded) { Id = 10003 };
            type4.AddTranslation(new EventTypeTranslation(4, "Új dokumentumok", "A felhasználó értesítést kap, amennyiben számára új dokumentum lett feltöltve", LanguageTypeEnum.HU) { Id = 10003 });

            EventType type5 = new EventType(EventTypeEnum.NewMessages) { Id = 10004 };
            type5.AddTranslation(new EventTypeTranslation(5, "Új üzenet", "A felhasználó értesítést kap, amennyiben új üzenete érkezett", LanguageTypeEnum.HU) { Id = 10004 });

            EventType type6 = new EventType(EventTypeEnum.OrderEvaluation) { Id = 10005 };
            type6.AddTranslation(new EventTypeTranslation(6, "Értékelés", "A felhasználó értesítést kap, amennyiben közeleg a megrendelésének az értékelési határideje", LanguageTypeEnum.HU) { Id = 10005 });

            events.Add(new EventTypeDto(type1));
            events.Add(new EventTypeDto(type2));
            events.Add(new EventTypeDto(type3));
            events.Add(new EventTypeDto(type4));
            events.Add(new EventTypeDto(type5));
            events.Add(new EventTypeDto(type6));

            return events;
        }
    }
}