using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
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

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class WorkStationsTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public WorkStationsTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Create_workstation_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var ws = new WorkStationCreateDto()
            {
                Name = "New WS",
                OptimalCrew = 13,
                IsActive = true,
                WorkStationTypeId = 10000,
                MachineId = 10002
            };
            var content = new StringContent(JsonConvert.SerializeObject(ws), Encoding.UTF8, "application/json");
            // Act
            var response = await client.PostAsync("api/workstations", content);
            // Assert
            response.EnsureSuccessStatusCode();
            var stringresponse = int.Parse(await response.Content.ReadAsStringAsync());
            stringresponse.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Get_workstation_by_id_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var ws = new WorkStation("First Test WS", 4, true, 10000) { MachineId = 10002 };

            var expectedResult = new WorkStationDetailsDto(ws);

            // Act
            var response = await client.GetAsync("api/workstations/10000");

            // Assert
            response.EnsureSuccessStatusCode();
            response.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task Update_workstation_by_id_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var ws = new WorkStationUpdateDto()
            {
                Name = "Modified WS Name",
                OptimalCrew = 5,
                MachineId = 10008,
                WorkStationTypeId = 10001
            };

            var content = new StringContent(JsonConvert.SerializeObject(ws), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync("api/workstations/10005", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorkStationsAppService>();
                var updatedWs = await service.GetWorkStationAsync(10005);
                // Assert
                ws.Name.Should().Be(updatedWs.Name);
                ws.OptimalCrew.Should().Be(updatedWs.OptimalCrew);
                ws.MachineId.Should().Be(updatedWs.MachineId);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_workstations_should_work()
        {
            // Arrange
            var client = factory.CreateClient();
            var expectedResult = new List<WorkStationListDto>();

            expectedResult.AddRange(BuildWorkStations());
            IPagedList<WorkStationListDto> pagedList = new PagedList<WorkStationListDto>()
            {
                Items = expectedResult,
                PageIndex = 0,
                PageSize = 20,
                TotalCount = 1
            };

            var result = pagedList.ToPagedList();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorkStationsAppService>();
                var wsFilterDto = new WorkStationFilterDto() { Name = "Test" };
                var workStations = await service.GetWorkStationsAsync(wsFilterDto);

                // Assert
                result.Data.Count().Should().Be(workStations.Data.Count());
            }
        }

        [Fact]
        public async Task Get_workstations_with_wrong_name_should_return_empty_list()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorkStationsAppService>();
                var wsFilterDto = new WorkStationFilterDto() { Name = "Oh, that's not right, sir" };
                var workStations = await service.GetWorkStationsAsync(wsFilterDto);

                //Assert
                workStations.Data.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Get_workstations_with_wrong_optimal_crew_should_return_empty_list()
        {
            //Arrange
            var client = factory.CreateClient();
            //Act
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IWorkStationsAppService>();
                var wsFilterDto = new WorkStationFilterDto() { OptimalCrew = 20000 };
                var workStations = await service.GetWorkStationsAsync(wsFilterDto);

                //Assert
                workStations.Data.Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Delete_workstation_by_id_should_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            //Act
            var response = await client.DeleteAsync("api/workstations/10004");
            //Assert
            response.EnsureSuccessStatusCode();
        }

        private List<WorkStationListDto> BuildWorkStations()
        {
            return new List<WorkStationListDto>()
            {
                new WorkStationListDto()
                {
                    Id = 10000,
                    Name = "First Test WS",
                    OptimalCrew = 11,
                    Status = true,
                    WorkStationType = new WorkStationTypeListDto(new WorkStationType(WorkStationTypeEnum.Cnc) { Id = 10000 }) { Id = 10000 },
                    MachineName = new MachineWorkstationListDto(new Machine("Test CNC machine", 10000) { Id = 10002 }) { Id = 10002 }
                },
                new WorkStationListDto()
                {
                    Id = 10001,
                    Name = "Second Test WS",
                    OptimalCrew = 11,
                    Status = true,
                    WorkStationType = new WorkStationTypeListDto(new WorkStationType(WorkStationTypeEnum.Layout) { Id = 10001 }) { Id = 10001 },
                    MachineName = new MachineWorkstationListDto(new Machine("Test Cutting machine", 10000) { Id = 10006 }) { Id = 10006 }
                },
                new WorkStationListDto()
                {
                    Id = 10002,
                    Name = "Third Test WS",
                    OptimalCrew = 42,
                    Status = true,
                    WorkStationType = new WorkStationTypeListDto(new WorkStationType(WorkStationTypeEnum.Cnc) { Id = 10001 }) { Id = 10001 },
                    MachineName = new MachineWorkstationListDto(new Machine("Test CNC machine with fairy", 10000) { Id = 10003 }) { Id = 10003 }
                },
                new WorkStationListDto()
                {
                    Id = 10003,
                    Name = "Fourth Test WS",
                    OptimalCrew = 8,
                    Status = true,
                    WorkStationType = new WorkStationTypeListDto(new WorkStationType(WorkStationTypeEnum.Layout) { Id = 10001 }) { Id = 10001 },
                    MachineName = new MachineWorkstationListDto(new Machine("Test Cutting machine with unicorn", 10000) { Id = 10007 }) { Id = 10007 }
                }
            };
        }
    }
}
