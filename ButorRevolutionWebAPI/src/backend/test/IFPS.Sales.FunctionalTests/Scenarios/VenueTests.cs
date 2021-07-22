using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
using ENCO.DDD.Paging;
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Sales.FunctionalTests.Scenarios
{
    public class VenueTests : IClassFixture<IFPSSalesWebApplicationFactory>
    {
        private readonly IFPSSalesWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public VenueTests(IFPSSalesWebApplicationFactory factory)
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
        public async Task Get_venue_by_id()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var venue = new Venue("Venue the Brave") { Id = 10052 };
            venue.OfficeAddress = new Address(5678, "Brave City", "Bravery Avenue 12", 1);

            var meetingroom = new MeetingRoom("Csodaszoba", "Csodák csodája", venue.Id);
            //meetingroom.AddTranslation(new MeetingRoomTranslation(meetingroom.Id, "Csodaszoba", "Csodák csodája", LanguageTypeEnum.HU) { Id = 10010 });
            //meetingroom.AddTranslation(new MeetingRoomTranslation(meetingroom.Id, "Room where the miracle happens", "Wizard of Oz", LanguageTypeEnum.EN) { Id = 10011 });
            venue.AddMeetingRoom(meetingroom);

            var dayType = new DayType(DayTypeEnum.Friday, 8) { Id = 10023 };

            var venueDateRange = new VenueDateRange(venue.Id, dayType.Id, new DateRange(new DateTime(2019, 7, 10), new DateTime(2019, 7, 20))) { DayType = dayType };

            venue.AddOpeningHour(venueDateRange);

            var expectedResult = new VenueDetailsDto(venue);
            //Act
            var response = await client.GetAsync("api/venues/10052");
            //Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task Get_venues()
        {
            //Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<Venue>();
            //Act

            var firstVenue = new Venue("Test V1") { Id = 10000, OfficeAddress = new Address(3456, "City", "Schön street 99.", 1) };
            var secondVenue = new Venue("Test V2") { Id = 10001, Email = "test@test.com", PhoneNumber = "+873483234", OfficeAddress = new Address(3455, "City", "Schön street 23.", 1) };

            expectedResult.Add(secondVenue);

            PagedList<Venue> pagedList = new PagedList<Venue>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };
            var result = pagedList.ToPagedList(VenueListDto.FromEntity);
            var venueFilterDto = new VenueFilterDto() { Name = "Test V2", OfficeAddress = "City" };

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVenuesAppService>();

                var venues = await service.GetVenuesListAsync(venueFilterDto);

                //Assert
                result.Should().BeEquivalentTo(venues);
            }
        }

        [Fact]
        public async Task Get_venues_filter_by_addres()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVenuesAppService>();
                var venueFilterDto = new VenueFilterDto() { OfficeAddress = "Candy Boulevard" };
                var venues = await service.GetVenuesListAsync(venueFilterDto);

                //Assert
                venues.Data.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_venues_filter_by_name()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVenuesAppService>();
                var venueFIlterDto = new VenueFilterDto() { Name = "Candy" };
                var venues = await service.GetVenuesListAsync(venueFIlterDto);

                //Assert
                venues.Data.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_venues_filter_by_email()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVenuesAppService>();
                var venueFilterDto = new VenueFilterDto() { Name = "Test", Email = "candy@candy.com" };
                var venues = await service.GetVenuesListAsync(venueFilterDto);

                //Assert
                venues.Data.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Post_venues()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var venue = new VenueCreateDto()
            {
                Name = "Mighty Venue",
                Email = "super@venue.com",
                PhoneNumber = "+725678735",
                CompanyId = 10000,
                OfficeAddress = new AddressCreateDto()
                {
                    Address = "Super Avenue 42.",
                    City = "SuperTown",
                    CountryId = 1,
                    PostCode = 1117
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(venue), Encoding.UTF8, "application/json");
            // Act
            var response = await client.PostAsync("api/venues", content);
            // Assert
            response.EnsureSuccessStatusCode();
            var stringresponse = int.Parse(await response.Content.ReadAsStringAsync());
            stringresponse.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Put_venues()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var venueUpdateDto = new VenueUpdateDto()
            {
                /*Id = 10042, */
                Name = "Super-Super Venue",
                Email = "test@test.com",
                PhoneNumber = "+838483288",
                OpeningHours = new List<VenueDateRangeUpdateDto>(),
                MeetingRooms = new List<MeetingRoomUpdateDto>()
            };
            venueUpdateDto.OfficeAddress = new AddressUpdateDto() { Address = "Bocskai út 77", City = "Town", CountryId = 1, PostCode = 4534 };

            var dayTypeForVenue = new DayType(DayTypeEnum.Wednesday, 4) { Id = 10007 };
            var newMeetinRoomUpdateDto = new MeetingRoomUpdateDto() 
            { 
                Id = 10020,
                Name = "Nagyon Pici szoba",
                Description = "Picike. Szóltam."
            };

            venueUpdateDto.OpeningHours.Add(new VenueDateRangeUpdateDto() { DayTypeId = dayTypeForVenue.Id, From = new DateTime(2019, 7, 25), To = new DateTime(2019, 7, 30) });
            venueUpdateDto.MeetingRooms.Add(newMeetinRoomUpdateDto);

            var content = new StringContent(JsonConvert.SerializeObject(venueUpdateDto), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/venues/10042", content);

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IVenuesAppService>();
                var updatedVenue = await service.GetVenueDetailsAsync(10042);
                // Assert
                venueUpdateDto.Name.Should().BeEquivalentTo(updatedVenue.Name);

                var openingHourFromDto = venueUpdateDto.OpeningHours.Single(ent => ent.DayTypeId == dayTypeForVenue.Id);
                var openingHourFromModel = updatedVenue.OpeningHours.Single(ent => ent.DayType.Id == dayTypeForVenue.Id);

                openingHourFromDto.From.Should().BeSameDateAs(openingHourFromModel.From);

                var meetingRoomFromDto = venueUpdateDto.MeetingRooms.Single(ent => ent.Id == newMeetinRoomUpdateDto.Id);
                var meetingRoomFromModel = updatedVenue.MeetingRooms.Single(ent => ent.Id == newMeetinRoomUpdateDto.Id);

                meetingRoomFromDto.Name.Should().BeEquivalentTo(meetingRoomFromModel.Name);
                meetingRoomFromDto.Description.Should().BeEquivalentTo(meetingRoomFromModel.Description);

            }
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task Get_venue_by_wrong_id_shouldnt_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var response = await client.GetAsync("api/venues/5");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_venue_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var response = await client.DeleteAsync("api/venues/1");
            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_venue_with_wrong_id_shouldnt_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10099;
            //Act
            var response = await client.DeleteAsync($"api/venues/{id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}