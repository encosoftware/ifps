using FluentAssertions;
using IFPS.Sales.Application.Dto;
using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Application.Dto.Appointments;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.EF;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ENCO.DDD.Repositories;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class AppointmentsTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public AppointmentsTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_appointments_by_date_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var customer = new User("enco@enco.hu") { Id = 10000 };
            customer.AddVersion(new UserData("Kelemen Elemér", "00000", Clock.Now)
            {
                Id = 10000,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });
            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(category.Id, "Teszt alapanyagok", LanguageTypeEnum.HU) { Id = 10002, Core = category });
            var appointment = new Appointment(new DateRange(new DateTime(2019, 06, 05), new DateTime(2019, 06, 06)), 10000, "Hétfő reggel megbeszélés")
            {
                Id = 10000,
                Customer = customer,
                Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
                Category = category
            };
            var expectedResult = new AppointmentListDto(appointment);

            // Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAppointmentAppService>();
                // Assert
                var appointmentDateRangeDto = new AppointmentDateRangeDto() { From = new DateTime(2019, 06, 05), To = new DateTime(2019, 06, 06) };
                var appointments = await service.GetAppointmentsByDateRangeAsync(10000, appointmentDateRangeDto);
                appointments.Should().ContainEquivalentOf(expectedResult);
            }
        }


        [Fact]
        public async Task Get_appointment_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var customer = new User("enco@enco.hu") { Id = 10000 };
            customer.AddVersion(new UserData("Kelemen Elemér", "00000", Clock.Now)
            {
                Id = 10000,
                CoreId = 10000,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });
            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(category.Id, "Teszt alapanyagok", LanguageTypeEnum.HU) { Id = 10002, Core = category });

            var appointment = new Appointment(new DateRange(new DateTime(2019, 06, 05), new DateTime(2019, 06, 06)), 10000, "Hétfő reggel megbeszélés")
            {
                Id = 10000,
                Customer = customer,
                CustomerId = customer.Id,
                Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
                CategoryId = category.Id
            };
            var expectedResult = new AppointmentDetailsDto(appointment);

            // Act
            var resp = await client.GetAsync("api/appointments/10000");


            // Assert
            resp.EnsureSuccessStatusCode();
            var stringresp = await resp.Content.ReadAsStringAsync();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            stringresp.Should().Be(JsonConvert.SerializeObject(expectedResult, jsonSerializerSettings));
        }

        [Fact]
        public async Task Get_appointment_by_wrong_appointment_id_should_not_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var customer = new User("enco@enco.hu") { Id = 10000 };
            customer.AddVersion(new UserData("Kelemen Elemér", "00000", Clock.Now)
            {
                Id = 10000,
                CoreId = 10000,
                ContactAddress = new Address(1117, "Budapest", "Bocskai út 77-79.", 1)
            });
            var category = new GroupingCategory(GroupingCategoryEnum.MaterialType) { Id = 10000 };
            category.AddTranslation(new GroupingCategoryTranslation(category.Id, "Teszt alapanyagok", LanguageTypeEnum.HU) { Id = 10002, Core = category });

            var appointment = new Appointment(new DateRange(new DateTime(2019, 06, 05), new DateTime(2019, 06, 06)), 10000, "Hétfő reggel megbeszélés")
            {
                Id = 10000,
                Customer = customer,
                CustomerId = customer.Id,
                Address = new Address(6344, "Hajós", "Petőfi utca 52.", 1),
                CategoryId = category.Id
            };
            var expectedResult = new AppointmentDetailsDto(appointment);

            // Act
            var resp = await client.GetAsync("api/appointments/555");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAppointmentAppService>();

                // Assert
                resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetAppointmentDetailsAsync(555));
            }
        }

        [Fact]
        public async Task Delete_appointment_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            int id = 10001;

            //Act
            var resp = await client.DeleteAsync($"api/appointments/{id}");

            //Assert
            resp.EnsureSuccessStatusCode();
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IFPSSalesContext>();
                var deletedAppointment = context.Appointments.SingleOrDefault(ent => ent.Id == id);
                deletedAppointment.Should().Be(null);
            }
        }

        [Fact]
        public async Task Delete_appointment_works_with_bad_id()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            //Act
            var resp = await client.DeleteAsync($"api/appointments/10022");

            //Assert
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Put_appointment_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());

            var appointment = new AppointmentUpdateDto
            {
                PartnerId = 10000,
                CategoryId = 10001,
                CustomerId = 10006,
                From = new DateTime(2019, 05, 05),
                To = new DateTime(2019, 12, 31),
                Notes = "Hétfő reggel megbeszélés2",
                Address = new AddressUpdateDto
                {

                    PostCode = 9999,
                    City = "Hajós2",
                    Address = "Petőfi utca 55.",
                    CountryId = 1,
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/appointments/10000", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IAppointmentAppService>();
                var updatedAppointment = await service.GetAppointmentDetailsAsync(10000);

                // Assert
                resp.EnsureSuccessStatusCode();

                updatedAppointment.Notes.Should().BeEquivalentTo(appointment.Notes);
                updatedAppointment.Address.PostCode.Should().Be(appointment.Address.PostCode);
                updatedAppointment.Address.Address.Should().Be(appointment.Address.Address);
                updatedAppointment.Address.Address.Should().Be(appointment.Address.Address);
                updatedAppointment.PartnerId.Should().Be(appointment.PartnerId);
                updatedAppointment.CustomerId.Should().Be(appointment.CustomerId);
            }
        }
    }
}