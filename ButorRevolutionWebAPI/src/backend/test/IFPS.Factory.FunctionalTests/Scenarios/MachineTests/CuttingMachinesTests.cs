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
    public class CuttingMachinesTests : IClassFixture<IFPSFactoryWebApplicationFactory>
    {
        private readonly IFPSFactoryWebApplicationFactory factory;
        private JsonSerializerSettings jsonSerializerSettings;

        public CuttingMachinesTests(IFPSFactoryWebApplicationFactory factory)
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
        public async Task Create_cutting_machine_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            var content = new StringContent(JsonConvert.SerializeObject(BuildCuttingMachineCreateDto()), Encoding.UTF8, "application/json");

            //Act
            var resp = await client.PostAsync("api/cuttingmachines", content);
            var asd = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();

            var stringresp = int.Parse(await resp.Content.ReadAsStringAsync());
            stringresp.Should().BeOfType(typeof(int));
        }

        private CuttingMachineCreateDto BuildCuttingMachineCreateDto()
        {
            return new CuttingMachineCreateDto()
            {
                MachineName = "Test Cutting machine with dragons",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000",
                BrandId = 10000
            };
        }

        [Fact]
        public async Task Get_cuttingmachines_by_id_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var expectedResult = BuildCuttingMachineDetails();
            var id = 10009;
            var resp = await client.GetAsync($"api/cuttingmachines/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();
                var result = await service.GetCuttingMachineByIdAsync(id);

                // Assert
                resp.EnsureSuccessStatusCode();
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public async Task Get_cuttingmachines_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var id = 10050;
            var resp = await client.GetAsync($"api/cuttingmachines/{id}");

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();

                // Assert
                //   resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
                await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetCuttingMachineByIdAsync(id));
            }
        }

        [Fact]
        public async Task Get_cuttingmachines_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            IPagedList<CuttingMachineListDto> pagedList = new PagedList<CuttingMachineListDto>()
            {
                Items = BuildCuttingMachineList(),
                PageIndex = 0,
                PageSize = 20,
                TotalCount = BuildCuttingMachineList().Count
            };
            var result = pagedList.ToPagedList();

            //Act
            var resp = await client.GetAsync("api/cuttingmachines");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();
                CuttingMachineFilterDto filterDto = BuildCuttingMachineFilterDto();
                var machines = await service.GetCuttingMachinesAsync(filterDto);

                // Assert
                resp.EnsureSuccessStatusCode();
                machines.Should().BeEquivalentTo(result);
            }
        }

        [Fact]
        public async Task Remove_cuttingmachine_by_id_works()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            int id = 10010;
            var resp = await client.DeleteAsync($"api/cuttingmachines/{id}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();
                Func<Task> act = async () => { await service.GetCuttingMachineByIdAsync(id); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Update_cuttingmachines_works()
        {
            // Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            int id = 10011;
            var cutting = BuildCuttingMachineUpdateDto();

            var content = new StringContent(JsonConvert.SerializeObject(cutting), Encoding.UTF8, "application/json");

            // Act
            var resp = await client.PutAsync($"api/cuttingmachines/{id}", content);
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();
                var updatedCutting = await service.GetCuttingMachineByIdAsync(id);

                // Assert
                cutting.Code.Should().Be(updatedCutting.Code);
                cutting.MachineName.Should().Be(updatedCutting.MachineName);
                cutting.SerialNumber.Should().Be(updatedCutting.SerialNumber);
                cutting.SoftwareVersion.Should().Be(updatedCutting.SoftwareVersion);
                cutting.YearOfManufacture.Should().Be(updatedCutting.YearOfManufacture);
            }
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Remove_cuttingmachine_by_wrong_id_should_not_work()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await getAccessToken());

            //Act
            var resp = await client.DeleteAsync($"api/cuttingmachines/{3000}");
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICuttingMachineAppService>();
                Func<Task> act = async () => { await service.GetCuttingMachineByIdAsync(3000); };

                // Assert
                await act.Should().ThrowAsync<EntityNotFoundException>();
                resp.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        #region BuildEntities
        private CuttingMachineDetailsDto BuildCuttingMachineDetails()
        {
            var cuttingMachine = new CuttingMachine("Test Cutting machine with frogs", brandId: BuildBrand().Id)
            {
                Brand = BuildBrand(),
                Id = 10009,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000",
                BrandId = 10000
            };

            return new CuttingMachineDetailsDto(cuttingMachine);
        }

        private CuttingMachineFilterDto BuildCuttingMachineFilterDto()
        {
            return new CuttingMachineFilterDto
            {
                Code = "10000",
                MachineName = "Test Cutting machine with dragons",
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020
            };
        }

        private CuttingMachineUpdateDto BuildCuttingMachineUpdateDto() => new CuttingMachineUpdateDto
        {
            Code = "10500",
            MachineName = "Test update cutting machine",
            SerialNumber = "15001",
            SoftwareVersion = "15002",
            YearOfManufacture = 2023,
            BrandId = 10000
        };

        private Company BuildBrand()
        {
            return new Company("Test EN-CO Software", 10000) { Id = 10000, CurrentVersionId = 10000 };
        }      

        private List<CuttingMachineListDto> BuildCuttingMachineList()
        {
            List<CuttingMachineListDto> dtos = new List<CuttingMachineListDto>();
            CuttingMachine machine = new CuttingMachine("Test Cutting machine with dragons", brandId: BuildBrand().Id)
            {
                Id = 10019,
                SerialNumber = "10000",
                SoftwareVersion = "10000",
                YearOfManufacture = 2020,
                Code = "10000",
                Brand = BuildBrand()
            };

            dtos.Add(new CuttingMachineListDto(machine));
            return dtos;
        }
        
        #endregion
    }
}
