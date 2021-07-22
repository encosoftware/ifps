using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Domain.Model.Enums;
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
    public class CncMachinesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public CncMachinesTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Get_cncmachine_by_id_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var expectedResult = BuildCncMachineDetails();

            //Act
            var id = 10010;
            var resp = await client.GetAsync($"api/cncmachines/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();
                var result = await service.GetCncMachineByIdAsync(id);

                // Assert
                resp.EnsureSuccessStatusCode();
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task Get_cncmachine_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var id = 999;
            var resp = await client.GetAsync($"api/cncmachine/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();

                // Assert
                resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetCncMachineByIdAsync(id));
            }
        }

        [Fact]
        public async Task Get_cncmachines_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            IPagedList<CncMachineListDto> pagedList = new PagedList<CncMachineListDto>()
            {
                Items = BuildCncMachineList(),
                PageIndex = 0,
                PageSize = 20,
                TotalCount = BuildCncMachineList().Count
            };
            var result = pagedList.ToPagedList();

            //Act
            var resp = await client.GetAsync("api/cncmachines");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();
                CncMachineFilterDto cncmachineDto = BuildCncMachineFilterDto();
                var machines = await service.GetCncMachinesAsync(cncmachineDto);

                // Assert
                resp.EnsureSuccessStatusCode();
                machines.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Remove_cncmachine_by_id_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10019;
            // Act
            var resp = await client.DeleteAsync($"api/cncmachines/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();
                Func<Task> act = async () => { await service.GetCncMachineByIdAsync(id); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.EnsureSuccessStatusCode();
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Remove_cncmachine_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());
            int id = 3000;

            //Act
            var resp = await client.DeleteAsync($"api/cncmachines/{id}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();
                Func<Task> act = async () => { await service.GetCncMachineByIdAsync(id); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Create_cnc_machine_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var content = new StringContent(JsonConvert.SerializeObject(BuildCncMachineCreateDto()), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PostAsync("api/cncmachines", content);

            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());

            //Assert
            resp.EnsureSuccessStatusCode();
            stringresp.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task Update_cncmachines_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10012;
            var cnc = BuildcncUpdateDto();

            var content = new StringContent(JsonConvert.SerializeObject(cnc), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/cncmachines/{id}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICncMachineAppService>();
                var updatedCnc = await service.GetCncMachineByIdAsync(id);

                // Assert
                cnc.Code.Should().Be(updatedCnc.Code);
                cnc.MachineName.Should().Be(updatedCnc.MachineName);
                cnc.SerialNumber.Should().Be(updatedCnc.SerialNumber);
                cnc.SoftwareVersion.Should().Be(updatedCnc.SoftwareVersion);
                cnc.YearOfManufacture.Should().Be(updatedCnc.YearOfManufacture);
            }
            resp.EnsureSuccessStatusCode();
        }

        #region BuildEntities

        private static CncMachineCreateDto BuildCncMachineCreateDto() => new CncMachineCreateDto
        {
            Code = "10000",
            MachineName = "Test CNC controller machine",
            SerialNumber = "10000",
            SoftwareVersion = "10000",
            YearOfManufacture = 2020,
            BrandId = 10000
        };

        private static CncMachineUpdateDto BuildcncUpdateDto()
        {
            return new CncMachineUpdateDto
            {
                Code = "10000",
                MachineName = "New test cnc machine name",
                SerialNumber = "15000",
                SoftwareVersion = "15001",
                YearOfManufacture = 2022,
                BrandId = 10000
            };
        }

        private static CncMachineFilterDto BuildCncMachineFilterDto()
        {
            return new CncMachineFilterDto
            {
                Code = "10000",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020
            };
        }

        private List<CncMachineListDto> BuildCncMachineList()
        {
            List<CncMachineListDto> dtos = new List<CncMachineListDto>();
            var machine = new CncMachine("Test CNC machine", brandId: BuildBrand().Id)
            {
                Id = 10002,
                Code = "10000",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Brand = BuildBrand()
            };

            var machine2 = new CncMachine("Test CNC machine with fairy", brandId: BuildBrand().Id)
            {
                Id = 10003,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            var machine3 = new CncMachine("Test CNC controller machine", brandId: BuildBrand().Id)
            {
                Id = 10010,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            var machine4 = new CncMachine("Test CNC controller machine", brandId: BuildBrand().Id)
            {
                Id = 10012,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            var machine5 = new CncMachine("Test CNC controller machine", brandId: BuildBrand().Id)
            {
                Id = 10019,
                Code = "10000",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                ReturnHeight = 10000.0,
                Brand = BuildBrand()
            };

            dtos.Add(new CncMachineListDto(machine));
            dtos.Add(new CncMachineListDto(machine2));
            dtos.Add(new CncMachineListDto(machine3));
            dtos.Add(new CncMachineListDto(machine4));
            dtos.Add(new CncMachineListDto(machine5));
            return dtos;
        }

        private CncMachineDetailsDto BuildCncMachineDetails()
        {
            var machine = new CncMachine("Test CNC controller machine", brandId: BuildBrand().Id)
            {
                Id = 10010,
                Brand = BuildBrand(),
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                ReturnHeight = 10000.0,
                YearOfManufacture = 2020,
                Code = "10000"
            };
            return new CncMachineDetailsDto(machine);
        }        

        private Company BuildBrand()
        {
            return new Company("Test EN-CO Software", 10000) { Id = 10000, CurrentVersionId = 10000 };
        }

        #endregion
    }
}
