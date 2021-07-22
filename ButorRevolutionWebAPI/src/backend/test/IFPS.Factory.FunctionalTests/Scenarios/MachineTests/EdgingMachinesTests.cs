using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using FluentAssertions;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IFPS.Factory.FunctionalTests.Scenarios
{
    public class EdgingMachinesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public EdgingMachinesTests(IFPSFactoryWebApplicationFactory factory)
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
            var loginDto = new LoginDto
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
        public async Task Get_edgingmachine_by_id_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = BuildEdgingMachineDetails();

            //Act
            var id = 10017;
            var resp = await client.GetAsync($"api/edgingmachines/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();
                var result = await service.GetEdgingMachineByIdAsync(id);

                // Assert
                resp.EnsureSuccessStatusCode();
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task Get_edgingmachine_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var id = 999;
            var resp = await client.GetAsync($"api/edgingmachine/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();

                // Assert
                resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetEdgingMachineByIdAsync(id));
            }
        }

        [Fact]
        public async Task Get_edgingmachines_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            IPagedList<EdgingMachineListDto> pagedList = new PagedList<EdgingMachineListDto>()
            {
                Items = BuildEdgingMachineList(),
                PageIndex = 0,
                PageSize = 20,
                TotalCount = BuildEdgingMachineList().Count
            };
            var result = pagedList.ToPagedList();

            //Act
            var resp = await client.GetAsync("api/edgingmachines");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();
                EdgingMachineFilterDto edgingmachineDto = BuildEdgingMachineFilterDto();
                var machines = await service.GetEdgingMachinesAsync(edgingmachineDto);

                // Assert
                resp.EnsureSuccessStatusCode();
                machines.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Remove_edgingmachine_by_id_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            int id = 10019;
            var resp = await client.DeleteAsync($"api/edgingmachines/{id}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();
                Func<Task> act = async () => { await service.GetEdgingMachineByIdAsync(id); };

                // Assert
                resp.EnsureSuccessStatusCode();
                await act.Should().ThrowAsync<EntityNotFoundException>();
            }
        }

        [Fact]
        public async Task Remove_edgingmachine_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var resp = await client.DeleteAsync($"api/edgingmachines/{3000}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();
                Func<Task> act = async () => { await service.GetEdgingMachineByIdAsync(3000); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Create_edging_machine_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var content = new StringContent(JsonConvert.SerializeObject(BuildEdgingMachineCreateDto()), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PostAsync("api/edgingmachines", content);

            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());

            //Assert
            resp.EnsureSuccessStatusCode();
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Update_edgingmachines_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10018;
            var edging = BuildEdgingMachineUpdateDto();

            var content = new StringContent(JsonConvert.SerializeObject(edging), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/edgingmachines/{id}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IEdgingMachineAppService>();
                var updatedEdging = await service.GetEdgingMachineByIdAsync(id);

                // Assert
                edging.Code.Should().Be(updatedEdging.Code);
                edging.MachineName.Should().Be(updatedEdging.MachineName);
                edging.SerialNumber.Should().Be(updatedEdging.SerialNumber);
                edging.SoftwareVersion.Should().Be(updatedEdging.SoftwareVersion);
                edging.YearOfManufacture.Should().Be(updatedEdging.YearOfManufacture);
            }
            resp.EnsureSuccessStatusCode();
        }

        #region BuildEntities

        private static EdgingMachineCreateDto BuildEdgingMachineCreateDto()
        {
            var edgingMachine = new EdgingMachineCreateDto
            {
                Code = "10000",
                MachineName = "Test Edging controller machine",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                BrandId = 10000
            };
            return edgingMachine;
        }

        private EdgingMachineUpdateDto BuildEdgingMachineUpdateDto()
        {
            return new EdgingMachineUpdateDto
            {
                Code = "10500",
                MachineName = "Test update edging machine",
                SerialNumber = "15001",
                SoftwareVersion = "15002",
                YearOfManufacture = 2023,
                BrandId = 10000
            };
        }


        private EdgingMachineFilterDto BuildEdgingMachineFilterDto() => new EdgingMachineFilterDto
        {
            SerialNumber = "10000",
            SoftwareVersion = "10000",
            YearOfManufacture = 2020,
            Code = "10000"
        };

        private List<EdgingMachineListDto> BuildEdgingMachineList()
        {
            List<EdgingMachineListDto> dtos = new List<EdgingMachineListDto>();
            var machine = new EdgingMachine("Test Edging machine with elf", brandId: BuildBrand().Id)
            {
                Id = 10016,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            var machine2 = new EdgingMachine("Test Edging controller machine", brandId: BuildBrand().Id)
            {
                Id = 10017,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            var machine3 = new EdgingMachine("Test Edging controller machine", brandId: BuildBrand().Id)
            {
                Id = 10019,
                Code = "10000",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Brand = BuildBrand()
            };

            dtos.Add(new EdgingMachineListDto(machine));
            dtos.Add(new EdgingMachineListDto(machine2));
            dtos.Add(new EdgingMachineListDto(machine3));
            return dtos;
        }

        private EdgingMachineDetailsDto BuildEdgingMachineDetails()
        {
            var machine = new EdgingMachine("Test Edging controller machine", brandId: BuildBrand().Id)
            {
                Id = 10017,
                Brand = BuildBrand(),
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000"
            };
            return new EdgingMachineDetailsDto(machine);
        }

        private Company BuildBrand()
        {
            return new Company("Test EN-CO Software", 10000) { Id = 10000, CurrentVersionId = 10000 };
        }

        #endregion
    }
}
